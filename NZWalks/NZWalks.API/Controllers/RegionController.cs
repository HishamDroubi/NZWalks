using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilter;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;
using System.Data;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionController> logger;

        public RegionController( IRegionRepository regionRepository,
            IMapper mapper,
            ILogger<RegionController> logger)
        {        
            this.logger = logger;
            this.regionRepository  = regionRepository;
            this.mapper = mapper;
        }


        [HttpGet]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetAll()
        {

            logger.LogInformation("Fetching Regions From DB");

            //fetch the Regions As Domain Model List
            var regionsDM = await regionRepository.GetAllAsync();

            logger.LogDebug($"These are all the Regions : {JsonSerializer.Serialize(regionsDM)}");

            //Map the Domain Models to DTOs Using AutoMapper
            var regionsDTO=mapper.Map<List<RegionDTO>>(regionsDM);

            //Return the DTOs
            return Ok(regionsDTO);

        }

        [HttpGet]
        [Route("{Id}")]
        [Authorize(Roles = "Reader,Writer")]
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
        [ValidateModel]
        [Authorize(Roles = "Writer")]
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
        [ValidateModel]
        [Route("{Id:Guid}")]
        [Authorize(Roles = "Writer")]
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
        [Authorize(Roles = "Writer")]
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
