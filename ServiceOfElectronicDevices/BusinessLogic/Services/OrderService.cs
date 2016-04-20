using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BusinessLogic.Models;
using DataAccess.Models;

namespace BusinessLogic.Services
{
    public class OrderService
    {
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
                    State = "Oczekuje"
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

        public OrderViewModel GetOrderList()
        {
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                var mapper = new MapperConfiguration(m => m.CreateMap<TaskProgress, TaskProgressDto>()).CreateMapper();
                var orderList = context
                    .Orders
                    .ToList()
                    .Select(order => new OrderViewModel.Order
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

        public OrderViewModel GetUserOrders(string userId)
        {
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                var mapper = new MapperConfiguration(m => m.CreateMap<TaskProgress, TaskProgressDto>()).CreateMapper();

                var orderList = context
                    .AspNetUsers
                    .Find(userId)
                    .Orders
                    .Select(order => new OrderViewModel.Order()
                    {
                        Id = order.Id,
                        ClientName = order.AspNetUsers.UserName,
                        DeviceModel = order.Devices.Model,
                        DeviceBrand = order.Devices.Brand,
                        CurrentState = mapper.Map<TaskProgressDto>(order.TaskProgress.OrderBy(t => t.DateFrom).Last())
                    })
                    .ToList();
                return new OrderViewModel {Orders = orderList};
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
                    Tasks = mapper.Map<IEnumerable<TaskProgressDto>>(order.TaskProgress.ToList())
                };

                return model;
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

        public void AddTask(TaskProgressDto task)
        {
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                var mapper = new MapperConfiguration(m => m.CreateMap<TaskProgressDto, TaskProgress>()).CreateMapper();

                context.Orders
                    .Find(task.OrderId)
                    .TaskProgress
                    .OrderBy(t => t.DateFrom)
                    .Last()
                    .DateTo = DateTime.Now;

                task.DateFrom = DateTime.Now;
                context.TaskProgress
                    .Add(mapper.Map<TaskProgress>(task));

                context.SaveChanges();
            }
        }
    }
}
