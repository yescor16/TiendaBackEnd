using Api.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
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
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await productRepo.ListAsync(spec);
            return Ok(Mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await productRepo.GetEntityWithSpec(spec);
            return Mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands(int id)
        {
            return Ok(await brandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes(int id)
        {
            return Ok(await typeRepo.ListAllAsync());

        }
    }
}
