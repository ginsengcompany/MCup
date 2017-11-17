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
    public class REST<E>
    {
        public string warning;
        public async Task<List<E>> GetJson(string url)
        {
            List<E> Items = new List<E>();
            HttpClient client = new HttpClient();
            var uri = new Uri(string.Format(url, string.Empty));
            var response = await client.GetStringAsync(uri);
            warning = response;
            try
            {
                var isValid = JToken.Parse(response);
                Items = JsonConvert.DeserializeObject<List<E>>(response);
                return Items;
            }
            catch (Exception)
            {
                return new List<E>();
            }
        }


        public async Task<List<E>> PostJson(string url, E dati)
        {
            List<E> Items = new List<E>();
            HttpClient client = new HttpClient();
            string ContentType = "application/json"; // or application/xml
            string json = JsonConvert.SerializeObject(dati);
            var uri = new Uri(string.Format(url, String.Empty));
            var values = new List<E>();
            values.Add(dati);
            var result = await client.PostAsync(url, new StringContent(json.ToString(), Encoding.UTF8, ContentType));
            var response = await result.Content.ReadAsStringAsync();
            warning = response;
            try
            {
                var isValid = JToken.Parse(response);
                Items = JsonConvert.DeserializeObject<List<E>>(response);
                return Items;
            }
            catch (Exception a)
            {
                return new List<E>();
            }
        }

    }
}
