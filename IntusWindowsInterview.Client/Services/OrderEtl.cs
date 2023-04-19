using IntusWindowsInterview.Client.Models;
using IntusWindowsInterview.Client.Models.CommonModels;
using Newtonsoft.Json;

namespace IntusWindowsInterview.Client.Services
{
    public class OrderEtl : IOrderEtl
    {
        private readonly IConfiguration _configuration;
        public OrderEtl(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<PayloadResponse<OrderViewModel>> GetOrderById(long order_id)
        {
            HttpClient httpClient = new HttpClient() { MaxResponseContentBufferSize = 536870912, Timeout = TimeSpan.FromSeconds(100000) };
            //httpClient.MaxResponseContentBufferSize = 2560000;
            HttpResponseMessage response = new HttpResponseMessage();
            string ApiLocation = $"{_configuration["ApiPath"]}/Order/{order_id}";
            DateTime requestTime = DateTime.Now;

            try
            {
                response = await httpClient.GetAsync(ApiLocation);
            }
            catch (Exception ex)
            {
                return new PayloadResponse<OrderViewModel>
                {
                    message = new List<string>() { ex.Message },
                    payload_type = "Order",
                    payload = new OrderViewModel(),
                    success = false,
                    request_url = ApiLocation,
                    request_time = requestTime.ToString(),
                    response_time = DateTime.Now.ToString()
                };
            }

            return JsonConvert.DeserializeObject<PayloadResponse<OrderViewModel>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<PayloadResponse<List<OrderViewModel>>> GetOrders()
        {
            HttpClient httpClient = new HttpClient() { MaxResponseContentBufferSize = 536870912, Timeout = TimeSpan.FromSeconds(100000) };
            //httpClient.MaxResponseContentBufferSize = 2560000;
            HttpResponseMessage response = new HttpResponseMessage();
            string ApiLocation = $"{_configuration["ApiPath"]}/Order";
            DateTime requestTime = DateTime.Now;

            try
            {
                response = await httpClient.GetAsync(ApiLocation);
            }
            catch (Exception ex)
            {
                return new PayloadResponse<List<OrderViewModel>>
                {
                    message = new List<string>() { ex.Message },
                    payload_type = "Order",
                    payload = new List<OrderViewModel>(),
                    success = false,
                    request_url = ApiLocation,
                    request_time = requestTime.ToString(),
                    response_time = DateTime.Now.ToString()
                };
            }

            return JsonConvert.DeserializeObject<PayloadResponse<List<OrderViewModel>>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<PayloadResponse<OrderViewModel>> CreateOrder(OrderViewModel order)
        {
            HttpClient httpClient = new HttpClient() { MaxResponseContentBufferSize = 536870912, Timeout = TimeSpan.FromSeconds(100000) };
            //httpClient.MaxResponseContentBufferSize = 2560000;
            HttpResponseMessage response = new HttpResponseMessage();
            string ApiLocation = $"{_configuration["ApiPath"]}/Order";
            DateTime requestTime = DateTime.Now;

            try
            {
                var transtormToJson = JsonConvert.SerializeObject(order);
                response = await httpClient.PostAsync(ApiLocation, new StringContent(transtormToJson, System.Text.Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
                return new PayloadResponse<OrderViewModel>
                {
                    message = new List<string>() { ex.Message },
                    payload_type = "Order",
                    payload = new OrderViewModel(),
                    success = false,
                    request_url = ApiLocation,
                    request_time = requestTime.ToString(),
                    response_time = DateTime.Now.ToString()
                };
            }

            return JsonConvert.DeserializeObject<PayloadResponse<OrderViewModel>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<PayloadResponse<OrderViewModel>> UpdateOrder(OrderViewModel order)
        {
            HttpClient httpClient = new HttpClient() { MaxResponseContentBufferSize = 536870912, Timeout = TimeSpan.FromSeconds(100000) };
            //httpClient.MaxResponseContentBufferSize = 2560000;
            HttpResponseMessage response = new HttpResponseMessage();
            string ApiLocation = $"{_configuration["ApiPath"]}/Order/{order.Id}";
            DateTime requestTime = DateTime.Now;

            try
            {
                var transtormToJson = JsonConvert.SerializeObject(order);
                response = await httpClient.PutAsync(ApiLocation, new StringContent(transtormToJson, System.Text.Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
                return new PayloadResponse<OrderViewModel>
                {
                    message = new List<string>() { ex.Message },
                    payload_type = "Order",
                    payload = new OrderViewModel(),
                    success = false,
                    request_url = ApiLocation,
                    request_time = requestTime.ToString(),
                    response_time = DateTime.Now.ToString()
                };
            }

            return JsonConvert.DeserializeObject<PayloadResponse<OrderViewModel>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<PayloadResponse<OrderViewModel>> DeleteOrder(long order_id)
        {
            HttpClient httpClient = new HttpClient() { MaxResponseContentBufferSize = 536870912, Timeout = TimeSpan.FromSeconds(100000) };
            //httpClient.MaxResponseContentBufferSize = 2560000;
            HttpResponseMessage response = new HttpResponseMessage();
            string ApiLocation = $"{_configuration["ApiPath"]}/Order/{order_id}";
            DateTime requestTime = DateTime.Now;

            try
            {
                response = await httpClient.DeleteAsync(ApiLocation);
            }
            catch (Exception ex)
            {
                return new PayloadResponse<OrderViewModel>
                {
                    message = new List<string>() { ex.Message },
                    payload_type = "Order",
                    payload = new OrderViewModel(),
                    success = false,
                    request_url = ApiLocation,
                    request_time = requestTime.ToString(),
                    response_time = DateTime.Now.ToString()
                };
            }

            return JsonConvert.DeserializeObject<PayloadResponse<OrderViewModel>>(await response.Content.ReadAsStringAsync());
        }
    }

    public interface IOrderEtl
    {
        Task<PayloadResponse<OrderViewModel>> GetOrderById(long order_id);
        Task<PayloadResponse<List<OrderViewModel>>> GetOrders();
        Task<PayloadResponse<OrderViewModel>> CreateOrder(OrderViewModel order);
        Task<PayloadResponse<OrderViewModel>> UpdateOrder(OrderViewModel order);
        Task<PayloadResponse<OrderViewModel>> DeleteOrder(long order_id);
    }
}
