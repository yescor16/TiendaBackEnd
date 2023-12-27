using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBasketRepository basketRepo;

        public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepo ) 
        {
            this.unitOfWork = unitOfWork;
            this.basketRepo = basketRepo;

        }


        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
           

            // get basket from the basket repo
            var basket = await basketRepo.GetBasketAsync(basketId);
            //get items from repo
            var items = new List<OrderItem>();
             foreach (var item in basket.Items)
            {
                var productItem = await this.unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }
            //get deliveryMethod
            var deliveryMethod = await this.unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            
            //calc subtotal
            var subtotal = items.Sum(item => item.Price * item.Quantity);

            //createOrder
            var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal);
            this.unitOfWork.Repository<Order>().Add(order);

            //Save db
            var result = await unitOfWork.Complete();
            if (result <= 0) return null;

            //delete basket
            await basketRepo.DeleteBasketAsync(basketId);

            //Return order
            return order;

        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        {
            return await unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrdeerByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);
            return await unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrderForUserAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);
            return await unitOfWork.Repository<Order>().ListAsync(spec);
        }
    }
}
