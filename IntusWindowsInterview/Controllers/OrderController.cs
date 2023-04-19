using IntusWindowsInterview.Common;
using IntusWindowsInterview.Model.CommonModel;
using IntusWindowsInterview.Model.DBModel;
using IntusWindowsInterview.Model.ViewModel;
using IntusWindowsInterview.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntusWindowsInterview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _services;
        private readonly string requestTime = Utilities.GetRequestResponseTime();
        public OrderController(IOrderServices services)
        {
            _services = services;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(OrderViewModel), 200)]
        [Route("")]
        public async Task<IActionResult> GetOrders()
        {
            var result = await _services.GetOrders();

            return Ok(new PayloadResponse<List<OrderViewModel>>
            {
                message = result != null ? result.message : null,
                payload = result.data,
                payload_type = "Order List",
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime(),
                success = result != null ? result.success : false
            });
        }

        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(OrderViewModel), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            if (id == 0)
            {
                return ErrorResponse.BadRequest(id);
            }

            var result = await _services.GetOrderById(id);

            return Ok(new PayloadResponse<OrderViewModel>
            {
                message = result != null ? result.message : null,
                payload = result.data,
                payload_type = "Order Details",
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime(),
                success = result != null ? result.success : false
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(CreatedResult), 201)]
        [Route("")]
        public async Task<IActionResult> CreateOrder(OrderViewModel order)
        {
            if (!ModelState.IsValid)
            {
                return ErrorResponse.BadRequest(order);
            }

            var result = await _services.CreateOrder(order);

            if(!result.success)
            {
                return ErrorResponse.BadRequest(result);
            }

            var response = new PayloadResponse<OrderViewModel>
            {
                message = result != null ? result.message : null,
                payload = result.data,
                payload_type = "Order Details",
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime(),
                success = result != null ? result.success : false
            };

            return Created($"{response.request_url}/{response.payload.Id}", response);

        }

        [HttpPut]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(OrderViewModel), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderViewModel order)
        {
            if (!ModelState.IsValid)
            {
                return ErrorResponse.BadRequest(order);
            }

            if (id == 0)
            {
                return ErrorResponse.BadRequest(id);
            }

            var result = await _services.UpdateOrder(id, order);

            if (!result.success)
            {
                return ErrorResponse.BadRequest(result);
            }

            return Ok(new PayloadResponse<OrderViewModel>
            {
                message = result != null ? result.message : null,
                payload = result.data,
                payload_type = "Order Details",
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime(),
                success = result != null ? result.success : false
            });

        }

        [HttpDelete]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(OrderViewModel), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteOrderById(int id)
        {
            if (id == 0)
            {
                return ErrorResponse.BadRequest(id);
            }

            var result = await _services.DeleteOrder(id);

            return Ok(new PayloadResponse<OrderViewModel>
            {
                message = result != null ? result.message : null,
                payload = result.data,
                payload_type = "Order Delete",
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime(),
                success = result != null ? result.success : false
            });
        }

    }
}
