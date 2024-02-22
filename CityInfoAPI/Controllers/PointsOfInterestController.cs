using AutoMapper;
using CityInfoAPI.Entities;
using CityInfoAPI.Models;
using CityInfoAPI.Repositories;
using CityInfoAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointsofinterest")]
    public class PointsOfInterestController : ControllerBase
    {

        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _localMailService;
        private readonly CitiesDataStore citiesDataStore;
        private readonly IMapper _mapper;
        private readonly ICityInfoRepository _cityInfoRepository;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            IMailService localMailService, CitiesDataStore citiesDataStore, IMapper mapper, ICityInfoRepository cityInfoRepository
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _localMailService = localMailService ?? throw new ArgumentNullException(nameof(logger));
            this.citiesDataStore = citiesDataStore;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"{cityId} Not found ...");
                return NotFound();
            }

            var points = await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);
            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(points));
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
        }

        #region Post

        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(
            int cityId, PointOfInterestForCreationDto pointOfInterest
            )
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) return NotFound();

            var finalPoint = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalPoint);

            await _cityInfoRepository.SaveChangesAsync();

            var createdPoint = _mapper.Map<Models.PointOfInterestDto>(finalPoint);

            return CreatedAtRoute("GetPointOfInterest", new
            {
                cityId = cityId,
                pointOfInterestId = createdPoint.Id
            }, createdPoint); ;
        }

        #endregion

        #region Edit

        [HttpPut("{pointOfInterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterest)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) return NotFound();

            var point = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (point == null) return NotFound();

            _mapper.Map(pointOfInterest, point);

            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region Edit with patch

        [HttpPatch("{pointOfInterestid}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestid, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) return NotFound();

            var pointEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestid);

            if (pointEntity == null) return NotFound();

            var pointToPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointEntity);

            patchDocument.ApplyTo(pointToPatch, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!TryValidateModel(pointToPatch))
            {
                BadRequest(ModelState);
            }

            _mapper.Map(pointToPatch, pointEntity);
            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region Delete
        [HttpDelete("{pointOfInterestId}")]
        public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) return NotFound();

            var pointEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if (pointEntity == null) return NotFound();

            _cityInfoRepository.DeletePointOfInterestForCityAsync(pointEntity);
            await _cityInfoRepository.SaveChangesAsync();

            _localMailService
                .Send("Point of interest deleted", $"point of interest {pointEntity.Name} was deleted");
            return NoContent();

        }
        #endregion
    }
}
