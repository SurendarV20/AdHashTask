using Newtonsoft.Json;
using System.Data;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

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


        public static async Task<string> PostDataAsync<R>(string requestUrl, R requestData)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(requestUrl);

                    var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.PostAsync(requestUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        return responseData;    
                    }
                    else
                    {
                        throw new Exception("Error occurred while making request.");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error occurred while making request.", ex);
                }
            }
        }

    }
}
