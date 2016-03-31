using System;
using System.Linq;
using AutoMapper;
using BusinessLogic.Models;
using DataAccess.Models;

namespace BusinessLogic.Services
{
    public class TokenService
    {
        public object SetToken(string userId)
        {
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                var user = context.AspNetUsers.SingleOrDefault(u => u.Id == userId);
                if (user == null)
                    return null;

                user.Token = Guid.NewGuid().ToString().ToLower();
                context.SaveChanges();
                return user.Token;
            }
        }

        public AspNetUsersDto CheckToken(string token)
        {
            var mapper = new MapperConfiguration(m => m.CreateMap<AspNetUsers, AspNetUsersDto>()).CreateMapper();
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                var user = context.AspNetUsers
                            .SingleOrDefault(u => u.Token == token);

                return mapper.Map<AspNetUsersDto>(user);
            }
        }
    }
}
