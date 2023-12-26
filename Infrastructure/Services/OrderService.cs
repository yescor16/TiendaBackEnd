using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> orderRepo;
        private readonly IGenericRepository<DeliveryMethod> deliveryMethodRepo;
        private readonly IGenericRepository<Product> productRepo;
        private readonly IBasketRepository basketRepo;

        public OrderService(IGenericRepository<Order> orderRepo, IGenericRepository<DeliveryMethod> deliveryMethodRepo, IGenericRepository<Product> productRepo, IBasketRepository basketRepo ) 
        {
            this.orderRepo = orderRepo;
            this.deliveryMethodRepo = deliveryMethodRepo;
            this.productRepo = productRepo;
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
                var productItem = await productRepo.GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }
            //get deliveryMethod
            var deliveryMethod = await deliveryMethodRepo.GetByIdAsync(deliveryMethodId);
            
            //calc subtotal
            var subtotal = items.Sum(item => item.Price * item.Quantity);

            //createOrder
            var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal);

            //Save db

            //Return order
            return order;

        }

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrdeerByIdAsync(int id, string buyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Order>> GetOrderForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
