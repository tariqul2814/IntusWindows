using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntusWindowsInterview.Client.Models.CommonModels
{
    public class PayloadResponse<TEntity> where TEntity : class
    {
        public PayloadResponse()
        {
           
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
