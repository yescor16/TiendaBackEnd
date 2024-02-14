using Api.Dtos;
using Api.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class StoreController : BaseApiController
    {
        private readonly IGenericRepository<Store> storeRepo;
        public IMapper Mapper { get; }
        public StoreController(IGenericRepository<Store> storeRepo, IMapper mapper)
        {
            this.storeRepo = storeRepo;
            Mapper = mapper;
        }


        //Get stores
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Store>>> GetStores()
        {
            return Ok(await storeRepo.ListAllAsync());
        }

        //store by Id
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Store>> GetStore(int id)
        {
            var spec = new StoreWithSpecification(id);
            var store = await storeRepo.GetEntityWithSpec(spec);

            if (store == null) return NotFound(new ApiResponse(404));
            return store;
        }


    }
}
