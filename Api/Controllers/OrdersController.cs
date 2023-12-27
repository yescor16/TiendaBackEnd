using Api.Dtos;
using Api.Errors;
using Api.Extensions;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();
            var address = mapper.Map<AddressDto, Address>(orderDto.ShipToAddress);
            var order = await orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, address);

            if(order == null) return BadRequest(new ApiResponse(400, "Problema creando la orden"));

            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var email = User.RetrieveEmailFromPrincipal();
            var orders = await orderService.GetOrderForUserAsync(email);
            return Ok(mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersByIdForUser(int id)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();
            var order = await orderService.GetOrdeerByIdAsync(id, email);

            if (order == null) return NotFound(new ApiResponse(400));
            return Ok(mapper.Map<Order,OrderToReturnDto>(order));
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await orderService.GetDeliveryMethodAsync());
        }
    }
}
