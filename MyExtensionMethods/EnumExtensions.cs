using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MyExtensionMethods
{
    /// <summary>
    /// Extension methods para enumeradores
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Obtiene el atributo Description de un enumerador
        /// </summary>
        public static string GetDescription(this Enum enumMember)
        {
            var attr = GetDescriptionAttribute(enumMember);
            
            if (attr != null) return attr.Description;

            return enumMember.ToString();
        }

        /// <summary>
        /// Comprueba si un enumerador tiene un determinado Flag
        /// </summary>
        public static bool IsSet(this Enum self, Enum valueMemberToCheck)
        {
            return (Convert.ToUInt32(self) & Convert.ToUInt32(valueMemberToCheck)) != 0;
        }

        #region HasDescription
        /// <summary>
        /// Comprueba si un enumerador tiene el atributo Description
        /// </summary>
        public static bool HasDescription(this Enum enumMember)
        {
            return GetDescriptionAttribute(enumMember) != null;
        }

        /// <summary>
        /// Comprueba si un enumerador tiene el atributo Description
        /// </summary>
        public static bool HasDescription(this Enum enumMember, string description)
        {
            var attr = GetDescriptionAttribute(enumMember);
            return attr != null && attr.Description.Equals(description);
        }

        /// <summary>
        /// Comprueba si un enumerador tiene el atributo Description
        /// </summary>
        public static bool HasDescription(this Enum enumMember, string description, StringComparison comparisionType)
        {
            var attr = GetDescriptionAttribute(enumMember);
            return attr != null && attr.Description.Equals(description, comparisionType);
        }
        #endregion HasDescription

        /// <summary>
        /// Parsea el valor string. Si no existe, devuelve el valor por defecto
        /// </summary>
        public static T ParseFromString<T>(this Enum enumMember, string value) where T : struct
        {
            return enumMember.ParseFromString(value, default(T));
        }

        /// <summary>
        /// Parsea el valor string. Si no existe, devuelve el valor por defecto indicado
        /// </summary>
        public static T ParseFromString<T>(this Enum enumMember, string value, T defaultValue = default) where T : struct
        {
            if (enumMember is null) throw new ArgumentNullException(nameof(enumMember));

            var done = Enum.TryParse(value, true, out T res);

            return done ? res : defaultValue;
        }

        /// <summary>
        /// Obtiene un diccionario con todos los valores del enumerador y su descripción
        /// </summary>
        public static Dictionary<string, string> ToDictionary(this Type enumType)
        {
            if (enumType.IsEnum)
            {
                var rtdo = new Dictionary<string, string>();

                Enum.GetValues(enumType)
                    .Cast<int>()
                    .Where(x => !x.Equals(0))
                    .ToList()
                    .ForEach(x =>
                    {
                        var aux = (Enum)Enum.Parse(enumType, x.ToString());
                        rtdo.Add(aux.ToString(), aux.GetDescription());
                    });

                return rtdo;
            }

            return null;
        }

        /// <summary>
        /// Parsea un valor string a un enumerador. Si no existe, devuelve el valor por defecto del enumerador.
        /// </summary>
        public static T? ToEnum<T>(this string value) where T : struct
        {
            if (string.IsNullOrEmpty(value)) return default(T);
            return Enum.TryParse(value, true, out T result) ? result : default;
        }

        #region private methods
        /// <summary>
        /// Obtiene el atributo Description de un enumerador. Null, si no existe
        /// </summary>
        private static DescriptionAttribute GetDescriptionAttribute(Enum enumMember)
        {
            Type type = enumMember.GetType();
            MemberInfo[] memInfo = type.GetMember(enumMember.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                    return (DescriptionAttribute)attrs[0];
            }

            return null;
        }
        #endregion private methods
    }
}