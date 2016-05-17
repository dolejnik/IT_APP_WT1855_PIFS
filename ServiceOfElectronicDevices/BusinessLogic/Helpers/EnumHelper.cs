using System;
using System.ComponentModel;

namespace ServiceOfElectronicDevices.Helpers
{
    public static class EnumHelper
    {
        public static string GetAttribute(this Enum enumVal)
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(Attribute), false);
            return (attributes.Length > 0) ? ((DescriptionAttribute)attributes[0]).Description : null;
        }
    }
}