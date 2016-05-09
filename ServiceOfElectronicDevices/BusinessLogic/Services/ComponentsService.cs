using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.Models;
using DataAccess.Models;

namespace BusinessLogic.Services
{
    public class ComponentsService
    {
        public IEnumerable<PartTypeDto> GetComponentsTypes()
        {
            var mapper = new MapperConfiguration(m => m.CreateMap<PartTypes, PartTypeDto>()).CreateMapper();

            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                return context.PartTypes
                    .ToList()
                    .Select(p => mapper.Map<PartTypeDto>(p));
            }
        }

        public void AddComponentType(string componentType)
        {
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                var type = new PartTypes {PartName = componentType};
                context.PartTypes.Add(type);
                context.SaveChanges();
            }
        }

        public void AddComponent(PartDto partDto)
        {
            var mapper = new MapperConfiguration(m => m.CreateMap<PartDto, Parts>()).CreateMapper();
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                context.Parts.Add(mapper.Map<Parts>(partDto));
                context.SaveChanges();
            }
        }

        public IEnumerable<PartDto> GetComponents()
        {
            var mapper = new MapperConfiguration(m => m.CreateMap<Parts, PartDto>()).CreateMapper();
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                return mapper.Map<IEnumerable<PartDto>>(context.Parts.ToList());
            }
        }

        public IEnumerable<PartDto> GetComponents(int id)
        {
            var mapper = new MapperConfiguration(m => m.CreateMap<Parts, PartDto>()).CreateMapper();
            using (var context = new ServiceOfElectronicDevicesDataBaseEntities())
            {
                return mapper.Map<IEnumerable<PartDto>>(context.PartTypes.Find(id).Parts.ToList());
            }
        }
    }
}
