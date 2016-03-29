using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BusinessLogic.Models;
using DataAccess.Models;

namespace BusinessLogic.Services
{
    public class UserRolesService
    {
        public IEnumerable<AspNetRolesDto> GetRoleList()
        {
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                var mapper = new MapperConfiguration(m => m.CreateMap<AspNetRoles, AspNetRolesDto>()).CreateMapper();

                var roles = context.AspNetRoles.ToList();
                return mapper.Map<IEnumerable<AspNetRolesDto>>(roles);
            }
        }


        public void AddRole(string roleName)
        {
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                context.AspNetRoles.Add(new AspNetRoles {Name = roleName});
                context.SaveChanges();
            }
        }
    }
}
