using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Courses.Util
{
    /// <summary>
    /// Содержит статические методы для работы с классами, свойствами
    /// </summary>
    public static class ClassHandler
    {
        /// <summary>
        /// Получает список значений свойств инстанса класса U,
        /// которые соответствуют типу T.
        /// </summary>
        /// <typeparam name="U">Класс, свойства которого хотим получить</typeparam>
        /// <typeparam name="T">Тип свойств, которые хотим получить</typeparam>
        /// <param name="instance"> Экземпляр класса U</param>
        /// <returns></returns>
        public static List<T> GetPropertyValuesOfType<U, T>(U instance)
        {
            PropertyInfo[] props = typeof(U).GetProperties();

            Type targetType = typeof(T);

            var propsOfType = props
                .Where(p => p.PropertyType == targetType)
                .ToList();

            List<T> valuesOfType = propsOfType
                .Select(p => (T)p.GetValue(instance)!)
                .ToList();

            return valuesOfType;
        }
    }
}
