using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class BrandsWithFilterId:BaseSpecification<Product>
    {
        public BrandsWithFilterId(int? storeId) : base(x => x.StoreId == storeId)
        {
            AddInclude(x => x.ProductBrand);
            AddOrderBy(x => x.Name);
        }
    }
}
