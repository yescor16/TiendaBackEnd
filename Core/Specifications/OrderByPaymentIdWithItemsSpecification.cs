using Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class OrderByPaymentIdWithItemsSpecification:BaseSpecification<Order>
    {
        public OrderByPaymentIdWithItemsSpecification(string paymentIntentId):base(o => o.PaymentIntentId == paymentIntentId)
        {
            
        }
    }
}
