using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BusinessLogic.Enums;
using BusinessLogic.Models;
using DataAccess.Models;
using ServiceOfElectronicDevices.Helpers;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class OrderService
    {
        private SendEmailService emailService;

        public OrderService()
        {
            emailService = new SendEmailService();
        }

        public void AddOrder(string userId, int deviceId, string description)
        {
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                Orders order = new Orders
                {
                    UserId = userId,
                    DeviceId = deviceId,
                    Description = description
                };
                context.Orders.Add(order);
                TaskProgress taskProgress = new TaskProgress
                {
                    DateFrom = DateTime.Now,
                    OrderId = order.Id,
                    State = (int)OrderStates.Accepted
                };
                context.TaskProgress.Add(taskProgress);
                context.SaveChanges();
            }
        }

        public IEnumerable<DevicesDto> GetDevicesList()
        {
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                var mapper = new MapperConfiguration(m => m.CreateMap<Devices, DevicesDto>()).CreateMapper();
                return mapper.Map<IEnumerable<DevicesDto>>(context.Devices.ToList());
            }
        }

        public IEnumerable<AspNetUsersDto> GetClientsList()
        {
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                var mapper = new MapperConfiguration(m => m.CreateMap<AspNetUsers, AspNetUsersDto>()).CreateMapper();

                var users = context.AspNetUsers.ToList();
                return mapper.Map<IEnumerable<AspNetUsersDto>>(users);
            }
        }

        public void AddDevice(DevicesDto deviceDto)
        {
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                var mapper = new MapperConfiguration(m => m.CreateMap<DevicesDto, Devices>()).CreateMapper();

                var device = mapper.Map<Devices>(deviceDto);
                context.Devices.Add(device);
                context.SaveChanges();
            }
        }

        public bool AuthorizeOrderOwner(string userId, int orderId)
        {
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                return context.Orders.Find(orderId).AspNetUsers.Id == userId;
            }
        }

        public OrderViewModel GetOrderList(OrderStates state, string clientEmail = null)
        {
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                var mapper = new MapperConfiguration(m => m.CreateMap<TaskProgress, TaskProgressDto>()).CreateMapper();
                var orderList = context
                    .Orders
                    .Where(o => string.IsNullOrEmpty(clientEmail) || o.AspNetUsers.Email == clientEmail)
                    .OrderByDescending(o => o.TaskProgress.Min(t => t.DateFrom))
                    .ToList()
                    .Select(order => new OrderViewModel.Order
                    {
                        Id = order.Id,
                        ClientName = order.AspNetUsers.UserName,
                        DeviceModel = order.Devices.Model,
                        DeviceBrand = order.Devices.Brand,
                        CurrentState = mapper.Map<TaskProgressDto>(order.TaskProgress.OrderBy(t => t.DateFrom).Last())
                    })
                    .Where(o => (int)state == -1 || o.CurrentState.State == state)
                    .ToList();
                return new OrderViewModel { Orders = orderList };
            }
        }

        public OrderViewModel GetUserOrders(string userId)
        {
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                var mapper = new MapperConfiguration(m => m.CreateMap<TaskProgress, TaskProgressDto>()).CreateMapper();

                var orderList = context
                    .AspNetUsers
                    .Find(userId)
                    .Orders
                    .OrderByDescending(o => o.TaskProgress.Min(t => t.DateFrom))
                    .Select(order => new OrderViewModel.Order()
                    {
                        Id = order.Id,
                        ClientName = order.AspNetUsers.UserName,
                        DeviceModel = order.Devices.Model,
                        DeviceBrand = order.Devices.Brand,
                        CurrentState = mapper.Map<TaskProgressDto>(order.TaskProgress.OrderBy(t => t.DateFrom).Last())
                    })
                    .ToList();
                return new OrderViewModel { Orders = orderList };
            }
        }

        public OrderDetailsViewModel GetOrderDetails(int id)
        {
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                var mapper = new MapperConfiguration(m => m.CreateMap<TaskProgress, TaskProgressDto>()).CreateMapper();

                var order = context
                    .Orders
                    .Find(id);

                var model = new OrderDetailsViewModel()
                {
                    Order = new OrderViewModel.Order()
                    {
                        Id = order.Id,
                        ClientEmail = order.AspNetUsers.UserName,
                        ClientName = order.AspNetUsers.Name,
                        ClientPhone = order.AspNetUsers.PhoneNumber,
                        DeviceModel = order.Devices.Model,
                        DeviceBrand = order.Devices.Brand
                    },
                    Description = order.Description,
                    Tasks = order.TaskProgress
                            .Select(t => new TaskProgressViewModel
                            {
                                Task = new TaskProgressDto
                                {
                                    DateFrom = t.DateFrom,
                                    DateTo = t.DateTo,
                                    Description = t.Description,
                                    Id = t.Id,
                                    OrderId = t.OrderId,
                                    Price = t.Price ?? 0.0,
                                    State = (OrderStates)t.State
                                },
                                ComponentsList = t.Task_Part.Select(d =>  new PartDto
                                {
                                    Brand = d.Parts.Brand,
                                    Id = d.Parts.Id,
                                    Model = d.Parts.Model,
                                    PartId = d.Parts.PartId,
                                    Price = d.Parts.Price
                                }).ToList()
                            }).ToList(),
                    TotalCost = order.TaskProgress.Sum(t => t.Price) + order.TaskProgress.Where(t => t.State == (int)OrderStates.WaitingForParts).Sum(t => t.Task_Part.SingleOrDefault()?.Parts.Price)
                };

                return model;
            }
        }

        public async Task AddTaskWithComponentsList(TaskProgressDto task, int[] componentsIds)
        {
            task.State = OrderStates.WaitingForClient;
            await AddTask(task);
            var taskId = GetLastTask(task.OrderId).Id;

            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                foreach (var componentId in componentsIds)
                {
                    context.TaskProgress
                           .Find(taskId)
                           .Task_Part
                           .Add(new Task_Part { PartId = componentId});
                }

                context.SaveChanges();
            }
        }

        public TaskProgressDto GetLastTask(int orderId)
        {
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                var mapper = new MapperConfiguration(m => m.CreateMap<TaskProgress, TaskProgressDto>()).CreateMapper();

                var task = context.Orders
                                  .Find(orderId)
                                  .TaskProgress
                                  .OrderBy(t => t.DateFrom)
                                  .Last();

                return mapper.Map<TaskProgressDto>(task);
            }
        }

        public async Task AddTask(TaskProgressDto task)
        {
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                var mapper = new MapperConfiguration(m => m.CreateMap<TaskProgressDto, TaskProgress>()).CreateMapper();

                var order = context.Orders
                    .Find(task.OrderId);

                order.TaskProgress
                     .OrderBy(t => t.DateFrom)
                     .Last()
                     .DateTo = DateTime.Now;

                task.DateFrom = DateTime.Now;
                if (task.State == OrderStates.Done || task.State == OrderStates.Deleted)
                    task.DateTo = DateTime.Now;

                context.TaskProgress
                .Add(mapper.Map<TaskProgress>(task));

                context.SaveChanges();

                if (task.State == OrderStates.Done)
                    await emailService.SendAsync(order.AspNetUsers.Email, "Zakończono naprawę", $"Status zgłoszenia został zmieniony na {task.State.GetAttribute()}. Łączny koszt wynosi: {order.TaskProgress.Sum(t => t.Price) + order.TaskProgress.Where(t => t.State == (int)OrderStates.WaitingForParts).Sum(t => t.Task_Part.Single().Parts.Price)}");
                else
                    await
                        emailService.SendAsync(order.AspNetUsers.Email, "Zmiana statusu zgłoszenia", $"Status zgłoszenia został zmieniony na {task.State.GetAttribute()}.");
            }
        }

        public void ChooseComponent(ChooseTaskModel model)
        {
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                 var order = context.Orders
                                    .SingleOrDefault(o => o.Id == model.OrderId);

                if(order.TaskProgress.SingleOrDefault(t => t.Id == model.TaskId).DateTo != null)
                    return;

                 order.TaskProgress
                      .SingleOrDefault(t => t.Id == model.TaskId)
                      .DateTo = DateTime.Now;

                var task = new TaskProgress
                {
                    DateFrom = DateTime.Now,
                    State = (int) OrderStates.WaitingForParts
                };
                order.TaskProgress
                     .Add(task);

                context.Task_Part.Add(new Task_Part
                {
                    TaskId  = task.Id,
                    PartId = model.ComponentId
                });

                context.SaveChanges();
            }
        }
    }
}
