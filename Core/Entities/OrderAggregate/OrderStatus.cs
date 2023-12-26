using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrderAggregate
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pendiente")]
        Pending,
        [EnumMember(Value = "Pago Recibido")]
        PaymentReceived,
        [EnumMember(Value = "Pago Rechazado")]
        PaymentFailed
        
    }
}
