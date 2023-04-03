using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly NZWalksDbContext dbConext;
        public RegionController(NZWalksDbContext dbContext) {

            this.dbConext = dbContext;

        }


        [HttpGet]
        public IActionResult GetAll()
        {
            //fetch the Regions As Domain Model List
            var regionsDM = dbConext.Regions.ToList();

            //Map the Domain Models to DTOs
            var regionsDTO = new List<RegionDTO>();
            foreach (var region in regionsDM)
            {
                regionsDTO.Add(new RegionDTO
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl,
                });
            }

            //Return the DTOs
            return Ok(regionsDTO);

        }

        [HttpGet]
        [Route("{Id}")]
        public IActionResult GetById([FromRoute] Guid Id)
        {
            //var region = dbConext.Regions.Find(Id);
            var region = dbConext.Regions.FirstOrDefault(x => x.Id == Id);
            if (region == null)
            {
                return NotFound();
            }
            //Map the Domain Model to DTO
            var regionDTO = new RegionDTO
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl,
            };

            return Ok(regionDTO);

        }

        [HttpPost]
        public IActionResult CreateRegion([FromBody] AddRegionDTO regionDTO)
        {
            var regionDM = new Region
            {
                Code = regionDTO.Code,
                Name = regionDTO.Name,
                RegionImageUrl = regionDTO.RegionImageUrl,
            };

            dbConext.Regions.Add(regionDM);
            dbConext.SaveChanges();

            var regionDTOResponse = new RegionDTO
            {
                Id = regionDM.Id,
                Code = regionDM.Code,
                Name = regionDM.Name,
                RegionImageUrl = regionDM.RegionImageUrl,
            };

            return CreatedAtAction(nameof(GetById), new { Id = regionDM.Id }, regionDTOResponse);
        }

        [HttpPut]
        [Route("{Id:Guid}")]
        public IActionResult UpdateRegion([FromRoute] Guid Id, [FromBody] UpdateRegionDTO updateRegionDTO) {

            var regionDM = dbConext.Regions.FirstOrDefault(x => x.Id == Id);
            if (regionDM == null)
            {
                return NotFound();
            }

            regionDM.Code = updateRegionDTO.Code;
            regionDM.Name = updateRegionDTO.Name;
            regionDM.RegionImageUrl = updateRegionDTO.RegionImageUrl;
            dbConext.SaveChanges();

            var regionDTOResponse = new RegionDTO
            {
                Id = regionDM.Id,
                Code = regionDM.Code,
                Name = regionDM.Name,
                RegionImageUrl = regionDM.RegionImageUrl,
            };
            return Ok(regionDTOResponse);
            //return CreatedAtAction(nameof(GetById),new {Id=regionDTOResponse.Id}, regionDTOResponse);
        }

        [HttpDelete]
        [Route("{Id:Guid}")]
        public IActionResult DeleteRegion([FromRoute] Guid Id)
        {
            var RegionToDelete=dbConext.Regions.FirstOrDefault(x => x.Id == Id);
            if(RegionToDelete == null)
            {
                return NotFound();
            }
            dbConext.Regions.Remove(RegionToDelete);
            dbConext.SaveChanges();

            //return the deleted region
            //map the DM to a DTO

            var regionDTO = new RegionDTO
            {
                Id = RegionToDelete.Id,
                Name = RegionToDelete.Name,
                Code = RegionToDelete.Code,
                RegionImageUrl = RegionToDelete.RegionImageUrl,
            };
            return Ok(regionDTO);
        }
    }
}
