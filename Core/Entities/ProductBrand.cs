using Core.Specifications;

namespace Core.Entities
{
    public class ProductBrand:BaseEntity
    {
        public string Name { get; set; }

        public static implicit operator ProductBrand(BrandsWithFilterId v)
        {
            throw new NotImplementedException();
        }
    }
}