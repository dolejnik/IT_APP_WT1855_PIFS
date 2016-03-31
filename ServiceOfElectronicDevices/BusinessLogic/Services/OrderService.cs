using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BusinessLogic.Models;
using DataAccess.Models;

namespace BusinessLogic.Services
{
    public class OrderService
    {
        public void AddOrder(string userId, int deviceId)
        {
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                Orders order = new Orders { UserId = userId, DeviceId = deviceId };
                context.Orders.Add(order);
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

        public OrderViewModel GetOrderList()
        {
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                var orderList = context
                    .Orders
                    .Select(order => new OrderViewModel.Order
                    {
                        ClientName = order.AspNetUsers.UserName,
                        DeviceModel = order.Devices.Model,
                        DeviceBrand = order.Devices.Brand
                    })
                    .ToList();
                return new OrderViewModel { Orders = orderList };
            }
        }

        public OrderViewModel GetUserOrders(string userId)
        {
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                var orderList = context
                    .AspNetUsers
                    .Find(userId)
                    .Orders
                    .Select(order => new OrderViewModel.Order()
                    {
                        DeviceModel = order.Devices.Model,
                        DeviceBrand = order.Devices.Brand
                    })
                    .ToList();
                return new OrderViewModel {Orders = orderList};
            }
        }

    }
}
