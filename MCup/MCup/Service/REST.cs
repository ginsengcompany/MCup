using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Service
{
    public class REST<E,T>
    {
        public string warning;
        public async Task<List<T>> GetJson(string url)
        {
            List<T> Items = new List<T>();
            HttpClient client = new HttpClient();
            var uri = new Uri(string.Format(url, string.Empty));
            var response = await client.GetStringAsync(uri);
            warning = response;
            try
            {
                var isValid = JToken.Parse(response);
                Items = JsonConvert.DeserializeObject<List<T>>(response);
                return Items;
            }
            catch (Exception)
            {
                return new List<T>();
            }
        }


        public async Task<List<T>> PostJsonList(string url, E dati)
        {
            List<T> Items = new List<T>();
            HttpClient client = new HttpClient();
            string ContentType = "application/json"; // or application/xml
            string json = JsonConvert.SerializeObject(dati);
            var uri = new Uri(string.Format(url, String.Empty));
            var result = await client.PostAsync(url, new StringContent(json.ToString(), Encoding.UTF8, ContentType));
            var response = await result.Content.ReadAsStringAsync();
            warning = response;
            try
            {
                var isValid = JToken.Parse(response);
                Items = JsonConvert.DeserializeObject<List<T>>(response);
                return Items;
            }
            catch (Exception a)
            {
                return new List<T>();
            }
        }

        public async Task<T> PostJson(string url, E dati)
        {
            T Item;
            HttpClient client = new HttpClient();
            string ContentType = "application/json"; // or application/xml
            string json = JsonConvert.SerializeObject(dati);
            var uri = new Uri(string.Format(url, String.Empty));
            var result = await client.PostAsync(url, new StringContent(json.ToString(), Encoding.UTF8, ContentType));
            var response = await result.Content.ReadAsStringAsync();
            warning = response;
            try
            {
                var isValid = JToken.Parse(response);
                Item = JsonConvert.DeserializeObject<T>(response);
                return Item;
            }
            catch (Exception a)
            {
                return default(T);
            }
        }


        public async Task<string> getString(string url)
        {
            HttpClient client = new HttpClient();
            var uri = new Uri(string.Format(url, string.Empty));
            string response = await client.GetStringAsync(uri);
            return response;
        }
    }
}
