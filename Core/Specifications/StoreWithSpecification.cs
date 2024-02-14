using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class StoreWithSpecification:BaseSpecification<Store> 
    {
        public StoreWithSpecification( int? storeId) : base(x => x.Id == storeId)
        {
            
        }
    }
}
