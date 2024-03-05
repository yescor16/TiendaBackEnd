using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductsWithStoreSpecification : BaseSpecification<Product>
    {

        public ProductsWithStoreSpecification(ProductSpecParams productParams) : base(x => 
           (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
            (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId) &&
            (!productParams.StoreId.HasValue || x.StoreId == productParams.StoreId)
        )
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
            AddInclude(x => x.Store);
            AddOrderBy(x => x.Name);
            //ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1),
            //    productParams.PageSize);

          
        }
    }
}
