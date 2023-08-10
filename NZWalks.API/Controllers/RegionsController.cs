using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Runtime.InteropServices;

namespace NZWalks.API.Controllers
{
    // https:localhost:1234/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, 
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        // GET ALL REGIONS
        // GET: https://localhost:portnumber - Domain models
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Get data from database - Domain models
            //var regions = dbContext.Regions.ToListAsync();
            var regionsDomain = await regionRepository.GetAllAsync();

            // Map domains models to DTOs using normal method
            
            //var regionsDto = new List<RegionDto>();
            //foreach (var regionDomain in regionsDomain)
            //{
            //    regionsDto.Add(new RegionDto()
            //    {
            //        Id = regionDomain.Id,
            //        Name = regionDomain.Name,
            //        Code = regionDomain.Code,
            //        RegionImageUrl = regionDomain.RegionImageUrl,
            //    });
            //}

            // Map domains model too DTOs using automapper
            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);
            
            // Return DTOs
            return Ok(regionsDto);

        }

        //GET SINGLE REGION (Get Region by ID)
        // GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task <IActionResult> GetByID([FromRoute] Guid id) 
        {
            //var region = dbContext.Regions.Find(id);
            // Get Region Domain Model From Database
            //var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            var regionDomain = await regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            // Map/Convert Region Domain Model to Region DTO
            //var regionsDto = new RegionDto
            //{
            //    Id = regionDomain.Id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    RegionImageUrl = regionDomain.RegionImageUrl,
            //};

            var regionsDto = mapper.Map<RegionDto>(regionDomain);
            //Return DTO back to client
            return Ok(regionsDto);
        }

        // POST to create new region
        //POST: https://localhost:portnumber/api/regions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map or Convert DTO to Domain Model
            //var regionDomainmodel = new Region
            //{
            //    Code = addRegionRequestDto.Code,
            //    Name = addRegionRequestDto.Name,
            //    RegionImageUrl = addRegionRequestDto.RegionImageUrl
            //};

            //using automapper
            var regionDomainmodel = mapper.Map<Region>(addRegionRequestDto);

            //use domain model to create Region

            //dbContext.Regions.Add(regionDomainmodel);
            //dbContext.SaveChanges();
            regionDomainmodel = await regionRepository.CreateAsync(regionDomainmodel);

            //Map Domain model back to DTO
            //var regionDto = new RegionDto
            //{
            //    Id = regionDomainmodel.Id,
            //    Code = regionDomainmodel.Code,
            //    Name = regionDomainmodel.Name,
            //    RegionImageUrl = regionDomainmodel.RegionImageUrl
            //};

            var regionDto = mapper.Map<RegionDto>(regionDomainmodel);
            return CreatedAtAction(nameof(GetByID), new { id = regionDomainmodel.Id }, regionDto);

        }

        //Update region
        //PUT: https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto) 
        {
            //Map DTO to Domain Model
            //var regionDomainModel = new Region
            //{
            //    Code = updateRegionRequestDto.Code,
            //    Name = updateRegionRequestDto.Name,
            //    RegionImageUrl = updateRegionRequestDto.RegionImageUrl
            //};

            //Using automapper
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);
            //Check if region exists
            //var regionDomainmodel = dbContext.Regions.FirstOrDefault(x=> x.Id == id);
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            // Map DTO to Doman model
            //regionDomainModel.Code = updateRegionRequestDto.Code;
            //regionDomainModel.Name = updateRegionRequestDto.Name;
            //regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            //dbContext.SaveChanges();

            // Convert Domain Model to DTO
            //var regionDto = new RegionDto
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};
            ////using automapper
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            return Ok(regionDto);
        }

        // Delete Region
        // DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task <IActionResult> Delete([FromRoute] Guid id)
        {
            //var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            // Delete region
            //dbContext.Regions.Remove(regionDomainModel);
            //dbContext.SaveChanges();

            // return deleted Region back
            // map Domain Model  to DTO
            //var regionDto = new RegionDto
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};

            //using automapper
            var regionDto = mapper.Map<RegionDto>(regionDomainModel); 

            return Ok(regionDto);
        }
    }
}
 