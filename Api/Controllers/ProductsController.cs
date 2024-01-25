using Api.Dtos;
using Api.Errors;
using Api.Helpers;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> productRepo;
        private readonly IGenericRepository<ProductBrand> brandRepo;
        public IGenericRepository<ProductType> typeRepo { get; }
        public IMapper Mapper { get; }

        public ProductsController(IGenericRepository<Product> productRepo,
            IGenericRepository<ProductType> typeRepo,
            IGenericRepository<ProductBrand> brandRepo,
            IMapper mapper
            )
        {
            this.productRepo = productRepo;
            this.typeRepo = typeRepo;
            this.brandRepo = brandRepo;
            Mapper = mapper;
        }

        [Cached(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
             [FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var countSpec = new ProductsWithFiltersForCountSpecification(productParams);

            var totalItems = await productRepo.CountAsync(countSpec);
            var products = await productRepo.ListAsync(spec);

            var data = Mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex,
                productParams.PageSize, totalItems, data));
        }

        [Cached(600)]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await productRepo.GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));
            return Mapper.Map<Product, ProductToReturnDto>(product);
        }


        [Cached(600)]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands(int id)
        {
            return Ok(await brandRepo.ListAllAsync());
        }

        [Cached(600)]
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes(int id)
        {
            return Ok(await typeRepo.ListAllAsync());

        }
    }
}
