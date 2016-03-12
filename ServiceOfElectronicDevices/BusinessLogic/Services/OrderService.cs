using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Internal;
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
                Mapper.CreateMap<Devices, DevicesDto>();
                return Mapper.Map<IEnumerable<DevicesDto>>(context.Devices.ToList());
            }
        }

        public IEnumerable<AspNetUsersDto> GetClientsList()
        {
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                Mapper.CreateMap<AspNetUsers, AspNetUsersDto>();
                var users = context.AspNetUsers.ToList();
                return Mapper.Map<IEnumerable<AspNetUsersDto>>(users);
            }
        }

        public void AddDevice(DevicesDto deviceDto)
        {
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                //                Mapper.Initialize();
                Mapper.CreateMap<DevicesDto, Devices>();
                var device = Mapper.Map<Devices>(deviceDto);
                context.Devices.Add(device);
                context.SaveChanges();
            }
        }

        public OrderViewModel GetOrderList(string userId)
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
                return new OrderViewModel {Orders = orderList};
            }
        }
    }
}
