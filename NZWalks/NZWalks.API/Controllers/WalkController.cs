using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilter;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class WalkController : ControllerBase
    {

       private readonly IWalkRepository walkRepository;
       private readonly IMapper mapper;

        public WalkController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles="Reader,Writer")]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber, [FromQuery]  int pageSize) {


            var x = Request.Headers.Authorization;
            Console.WriteLine("this is the JWT");
            Console.WriteLine(x);


            var walks= await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy,isAscending?? true, pageNumber, pageSize);
          return Ok(mapper.Map<List<WalkDTO>>(walks));

        }

        [HttpGet]
        [Route("{Id}")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var walk = await walkRepository.GetByIdAsync(Id);

            if(walk == null)
            {
                return NotFound();
            }
 
            return Ok( mapper.Map<WalkDTO>(walk));
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create (AddWalkDTO addDTO)
        {

            var walk=mapper.Map<Walk>(addDTO);

           await walkRepository.CreateAsync(walk);

            return CreatedAtAction(nameof(GetById), new { Id = walk.Id }, mapper.Map<WalkDTO>(walk));

        }
        [HttpPut]
        [ValidateModel]
        [Route("{Id}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid Id,  UpdateWalkDTO updateDTO)
        {

            var walk = mapper.Map<Walk>(updateDTO);
            if (walk == null)
            {
                return NotFound();
            }

            await walkRepository.UpdateAsync(Id,walk);

            return Ok(mapper.Map<WalkDTO> (walk));

        }


        [HttpDelete]
        [Route("{Id}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid Id)
        {

            var walk=await walkRepository.DeleteAsync(Id);
            if(walk == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<WalkDTO>(walk));

        }
    }
}
