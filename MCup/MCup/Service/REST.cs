using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MCup.Service
{
    public class REST<E, T>
    {
        public string warning;
        public HttpStatusCode responseMessage;

        public async Task<T> GetSingleJson(string url)
        {
            T Item;
            HttpClient client = new HttpClient();
            var uri  = new Uri(string.Format(url, String.Empty));
            try
            {

                var result = await client.GetAsync(uri);
                var response = await result.Content.ReadAsStringAsync();
                responseMessage = result.StatusCode;
                if (responseMessage == HttpStatusCode.ServiceUnavailable)
                    warning = result.ReasonPhrase;
                else
                    warning = response;
                var isValid = JToken.Parse(response);
                Item = JsonConvert.DeserializeObject<T>(response);
                return Item;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public async Task<T> GetSingleJson(string url,List<Header> headers)
        {
            T Item;
            HttpClient client = new HttpClient();
            var uri = new Uri(string.Format(url, String.Empty));
            for (int i = 0; i < headers.Count; i++)
                client.DefaultRequestHeaders.Add(headers[i].header, headers[i].value);
            try
            {
                var result = await client.GetAsync(uri);
                var response = await result.Content.ReadAsStringAsync();
                responseMessage = result.StatusCode;
                if (responseMessage == HttpStatusCode.ServiceUnavailable)
                    warning = result.ReasonPhrase;
                else
                    warning = response;
                var isValid = JToken.Parse(response);
                Item = JsonConvert.DeserializeObject<T>(response);
                return Item;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public async Task<string> getString(string url)
        {
            HttpClient client = new HttpClient();
            var uri = new Uri(string.Format(url, string.Empty));
            var result = await client.GetAsync(uri);
            var response = await result.Content.ReadAsStringAsync();
            responseMessage = result.StatusCode;
            if (responseMessage == HttpStatusCode.ServiceUnavailable)
                warning = result.ReasonPhrase;
            else
                warning = response;
            return response;
        }

        public async Task<string> getString(string url, List<Header> headers)
        {
            HttpClient client = new HttpClient();
            for (int i = 0; i < headers.Count; i++)
                client.DefaultRequestHeaders.Add(headers[i].header, headers[i].value);
            var uri = new Uri(string.Format(url, string.Empty));
            var result = await client.GetAsync(uri);
            var response = await result.Content.ReadAsStringAsync();
            responseMessage = result.StatusCode;
            if (responseMessage == HttpStatusCode.ServiceUnavailable)
                warning = result.ReasonPhrase;
            else
                warning = response;
            return response;
        }

        public async Task<List<T>> GetListJson(string url)
        {
            List<T> Items = new List<T>();
            HttpClient client = new HttpClient();
            var uri = new Uri(string.Format(url, string.Empty));
            try
            {
                var result = await client.GetAsync(uri);
                var response = await result.Content.ReadAsStringAsync();
                responseMessage = result.StatusCode;
                if (responseMessage == HttpStatusCode.ServiceUnavailable)
                    warning = result.ReasonPhrase;
                else
                    warning = response;
                var isValid = JToken.Parse(response);
                Items = JsonConvert.DeserializeObject<List<T>>(response);
                return Items;
            }
            catch (Exception)
            {
                return new List<T>();
            }
        }

        public async Task<List<T>> GetListJson(string url, List<Header> headers)
        {
            List<T> Items = new List<T>();
            HttpClient client = new HttpClient();
            var uri = new Uri(string.Format(url, string.Empty));
            for (int i = 0; i < headers.Count; i++)
                client.DefaultRequestHeaders.Add(headers[i].header, headers[i].value);
            try
            {
                var result = await client.GetAsync(uri);
                var response = await result.Content.ReadAsStringAsync();
                responseMessage = result.StatusCode;
                if (responseMessage == HttpStatusCode.ServiceUnavailable)
                    warning = result.ReasonPhrase;
                else
                    warning = response;
                var isValid = JToken.Parse(response);
                Items = JsonConvert.DeserializeObject<List<T>>(response);
                return Items;
            }
            catch (Exception)
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
            try
            {
                var result = await client.PostAsync(url, new StringContent(json.ToString(), Encoding.UTF8, ContentType));
                var response = await result.Content.ReadAsStringAsync();
                responseMessage = result.StatusCode;
                if (responseMessage == HttpStatusCode.ServiceUnavailable)
                    warning = result.ReasonPhrase;
                else
                    warning = response;
                var isValid = JToken.Parse(response);
                Item = JsonConvert.DeserializeObject<T>(response);
                return Item;
            }
            catch (Exception)
            {

            }
            return default(T);
        }

        public async Task<T> PostJson(string url, E dati, List<Header> header)
        {
            T Item;
            HttpClient client = new HttpClient();
            string ContentType = "application/json"; // or application/xml
            string json = JsonConvert.SerializeObject(dati);
            var uri = new Uri(string.Format(url, String.Empty));
            HttpContent httpContent = new StringContent(json.ToString());
            httpContent.Headers.ContentType = new MediaTypeHeaderValue(ContentType);
            for (int i = 0; i < header.Count; i++)
            {
                httpContent.Headers.Add(header[i].header, header[i].value);
            }
            try
            {
                client.Timeout = System.Threading.Timeout.InfiniteTimeSpan;
                var result = await client.PostAsync(url, httpContent);
                var response = await result.Content.ReadAsStringAsync();
                responseMessage = result.StatusCode;
                if (responseMessage == HttpStatusCode.ServiceUnavailable)
                    warning = result.ReasonPhrase;
                else
                    warning = response;
                var isValid = JToken.Parse(response);
                Item = JsonConvert.DeserializeObject<T>(response);
                return Item;
            }
            catch (Exception)
            {

            }
            return default(T);
        }

        public async Task<T> PostJson(string url, List<Header> header)
        {
            T Item;
            HttpClient client = new HttpClient();
            string ContentType = "application/json"; // or application/xml
            var uri = new Uri(string.Format(url, String.Empty));
            HttpContent httpContent = new StringContent("");
            httpContent.Headers.ContentType = new MediaTypeHeaderValue(ContentType);
            for (int i = 0; i < header.Count; i++)
            {
                httpContent.Headers.Add(header[i].header, header[i].value);
            }
            try
            {
                client.Timeout = System.Threading.Timeout.InfiniteTimeSpan;
                var result = await client.PostAsync(url, httpContent);
                var response = await result.Content.ReadAsStringAsync();
                responseMessage = result.StatusCode;
                if (responseMessage == HttpStatusCode.ServiceUnavailable)
                    warning = result.ReasonPhrase;
                else
                    warning = response;
                var isValid = JToken.Parse(response);
                Item = JsonConvert.DeserializeObject<T>(response);
                return Item;
            }
            catch (Exception)
            {

            }
            return default(T);
        }

        public async Task<List<T>> PostJsonList(string url, E dati)
        {
            List<T> Items = new List<T>();
            HttpClient client = new HttpClient();
            string ContentType = "application/json"; // or application/xml
            string json = JsonConvert.SerializeObject(dati);
            var uri = new Uri(string.Format(url, String.Empty));
            try
            {
                var result = await client.PostAsync(url, new StringContent(json.ToString(), Encoding.UTF8, ContentType));
                var response = await result.Content.ReadAsStringAsync();
                responseMessage = result.StatusCode;
                if (responseMessage == HttpStatusCode.ServiceUnavailable)
                    warning = result.ReasonPhrase;
                else
                    warning = response;
                var isValid = JToken.Parse(response);
                Items = JsonConvert.DeserializeObject<List<T>>(response);
                return Items;
            }
            catch (Exception e)
            {
                return new List<T>();
            }
        }

        public async Task<List<T>> PostJsonList(string url, E dati, List<Header> header)
                {
            List<T> Items = new List<T>();
            HttpClient client = new HttpClient();
            string ContentType = "application/json"; // or application/xml
            string json = JsonConvert.SerializeObject(dati);
            var uri = new Uri(string.Format(url, String.Empty));
            HttpContent httpContent = new StringContent(json.ToString());
            httpContent.Headers.ContentType = new MediaTypeHeaderValue(ContentType);
            for (int i = 0; i < header.Count; i++)
            {
                httpContent.Headers.Add(header[i].header, header[i].value);
            }
            try
            {
                var result = await client.PostAsync(url, httpContent);
                var response = await result.Content.ReadAsStringAsync();
                responseMessage = result.StatusCode;
                if (responseMessage == HttpStatusCode.ServiceUnavailable)
                    warning = result.ReasonPhrase;
                else
                    warning = response;
                var isValid = JToken.Parse(response);
                Items = JsonConvert.DeserializeObject<List<T>>(response);
                return Items;
            }
            catch (Exception e)
            {
                return new List<T>();
            }
        }

        public async Task<List<T>> PostJsonListSenzaBody(string url, List<Header> header)
        {
            List<T> Items = new List<T>();
            HttpClient client = new HttpClient();
            string ContentType = "application/json"; // or application/xml
            var uri = new Uri(string.Format(url, String.Empty));
            HttpContent httpContent = new StringContent("");
            httpContent.Headers.ContentType = new MediaTypeHeaderValue(ContentType);
            for (int i = 0; i < header.Count; i++)
            {
                httpContent.Headers.Add(header[i].header, header[i].value);
            }
            try
            {
                var result = await client.PostAsync(url, httpContent);
                var response = await result.Content.ReadAsStringAsync();
                responseMessage = result.StatusCode;
                if (responseMessage == HttpStatusCode.ServiceUnavailable)
                    warning = result.ReasonPhrase;
                else
                    warning = response;
                var isValid = JToken.Parse(response);
                Items = JsonConvert.DeserializeObject<List<T>>(response);
                return Items;
            }
            catch (Exception e)
            {
                return new List<T>();
            }
        }

    }
}
