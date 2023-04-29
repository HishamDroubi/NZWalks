using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        public RegionController(NZWalksDbContext dbContext,IRegionRepository regionRepository,IMapper mapper) 
        {

            this.regionRepository = regionRepository;
            this.mapper = mapper;

        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //fetch the Regions As Domain Model List
            var regionsDM = await regionRepository.GetAllAsync();

            //Map the Domain Models to DTOs Using AutoMapper
            var regionsDTO=mapper.Map<List<RegionDTO>>(regionsDM);

            //Return the DTOs
            return Ok(regionsDTO);

        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid Id)
        {
            var region = await regionRepository.GetByIdAsync(Id);
            if (region == null)
            {
                return NotFound();
            }

            var regionDTO = mapper.Map<RegionDTO>(region);

            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionDTO addRegionDTO)
        {
           //Map the DTO to a DomainModel
            var regionDM = mapper.Map<Region>(addRegionDTO);

            var regionModel=await regionRepository.CreateAsync(regionDM); 

            //Map the Domain Model to DTO
            var regionDTOResponse = mapper.Map<RegionDTO>(regionDM);

            return CreatedAtAction(nameof(GetById), new { Id = regionDM.Id }, regionDTOResponse);
        }

        [HttpPut]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid Id, [FromBody] UpdateRegionDTO updateRegionDTO) {

            var region=mapper.Map<Region>(updateRegionDTO);

            var regionDM = await regionRepository.UpdateAsync(Id, region);
            if (regionDM == null)
            {
                return NotFound();
            }

           var regionDTOResponse= mapper.Map<RegionDTO>(regionDM);

            return Ok(regionDTOResponse);
            //return CreatedAtAction(nameof(GetById),new {Id=regionDTOResponse.Id}, regionDTOResponse);
        }

        [HttpDelete]
        [Route("{Id:Guid}")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid Id)
        {
            var RegionToDelete=await regionRepository.DeleteAsync(Id);
            if(RegionToDelete == null)
            {
                return NotFound();
            }

            //map the DM to a DTO
            var regionDTO = mapper.Map<Region>(RegionToDelete);

            //return the deleted region
            return Ok(regionDTO);
        }
    }
}
