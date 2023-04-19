using IntusWindowsInterview.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntusWindowsInterview.Model.CommonModel
{
    public class PayloadResponse<TEntity> where TEntity : class
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PayloadResponse()
        {
            _httpContextAccessor = new HttpContextAccessor();
            this.request_url = _httpContextAccessor.HttpContext != null ? $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}{_httpContextAccessor.HttpContext.Request.Path}" : "";
            this.response_time = Utilities.GetRequestResponseTime();
        }
        public bool success { get; set; }
        public string request_time { get; set; }
        public string response_time { get; set; }
        public string request_url { get; set; }
        public List<string> message { get; set; }
        public TEntity payload { get; set; }
        public string payload_type { get; set; }
    }
}
