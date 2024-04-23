using Courier_lockers.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace Courier_lockers.Helper
{
    public class WPFHttpClient : IWPFHttpClient
    {
        public HttpClient _client { get; private set; }

        public WPFHttpClient(HttpClient httpClient, IOptions<OtherServerModel> otherServerModel)
        {
            httpClient.BaseAddress = new Uri(otherServerModel.Value.WPFURL);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _client = httpClient;
        }
        /// <summary>
        /// Get请求获取
        /// </summary>
        /// <param name="action"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<(bool, string)> GetData(string action, dynamic param)
        {
            bool bResult = true;
            string sResult = string.Empty;

            try
            {
                HttpResponseMessage response;
                using (HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8))
                {
                    httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                    response = await _client.GetAsync($"{action}{param}").ContinueWith(res =>
                    {
                        return res;
                    }).Result;
                }
                if (response != null && response.IsSuccessStatusCode)
                {
                    sResult = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    bResult = false;
                    sResult = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                bResult = false;
                sResult = ex.Message;
            }

            return (bResult, sResult);
        }

        /// <summary>
        /// Post请求获
        /// </summary>
        /// <param name="action"></param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public async Task<(bool, string)> PostData(string action, dynamic param)
        {
            bool bResult = true;
            string sResult = string.Empty;

            try
            {
                HttpResponseMessage response;
                using (HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8))
                {
                    httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    response = await _client.PostAsync(action, httpContent).ContinueWith(res =>
                    {
                        return res;
                    }).Result;
                }
                if (response != null && response.IsSuccessStatusCode)
                {
                    sResult = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    bResult = false;
                    sResult = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                bResult = false;
                sResult = ex.Message;
            }

            return (bResult, sResult);
        }
    }
}
