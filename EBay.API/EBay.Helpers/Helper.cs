using System.Data;
using System.Reflection;

namespace EBay.Helpers
{
    public class Helper
    {
        public static DataTable ToDataTable<T>(IEnumerable<T> data)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            foreach (T item in data)
            {
                var values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

    }
}
