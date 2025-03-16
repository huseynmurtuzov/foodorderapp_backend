using System.Reflection;
using System.Text.RegularExpressions;
using YemekSepeti.DTO;

namespace YemekSepeti.Functions
{
    public abstract class HelperFunctions
    {
        public static bool Check(object obj)
        {
            if (obj == null) return true;

            PropertyInfo[] properties = obj.GetType().GetProperties();

            
            return !properties
                .Where(p => p.PropertyType == typeof(string)) 
                .Select(p => (string)p.GetValue(obj))
                .Any(value => value.Trim() == "");
        }
    }
}
