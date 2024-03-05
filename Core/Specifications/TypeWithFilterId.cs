using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class TypeWithFilterId: BaseSpecification<Product>
    {
        public TypeWithFilterId(int? storeId) : base(x => x.StoreId == storeId)
        {
            AddInclude(x => x.ProductType);
            AddOrderBy(x => x.Name);
        }
    }
}
