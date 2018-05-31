using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        private ILogger<PointsOfInterestController> _logger;
        private IMailService _mailService;
        private ICityInfoService _cityInfoService;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, ICityInfoService cityInfoService)
        {
            _logger = logger;
            _mailService = mailService;
            _cityInfoService = cityInfoService;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                if (!_cityInfoService.CityExists(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} was not found");
                    return NotFound();
                }
                
                var pointsOfInterestForCity = _cityInfoService.GetPointsOfInterestForCity(cityId);

                var results = Mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity);

                return Ok(results);
            }
            catch (Exception)
            {
                _logger.LogInformation($"Exception while getting poi for city with id {cityId}");
                return StatusCode(500, "Problem when handling request");
            }
        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name ="GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            if (!_cityInfoService.CityExists(cityId))
                return NotFound();

            var pointOfInterest = _cityInfoService.GetPointOfInterest(cityId, id);

            if (pointOfInterest == null)
                return NotFound();

            return Ok(Mapper.Map<PointOfInterestDto>(pointOfInterest));
        }

        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();
            
            if (!_cityInfoService.CityExists(cityId))
                return NotFound();

            var pointOfInterestToInsert = Mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            _cityInfoService.AddPointOfInterest(cityId, pointOfInterestToInsert);

            if (!_cityInfoService.Save())
            {
                return StatusCode(500, "Error while handling the request");
            }

            var createdPointOfInterest = Mapper.Map<Models.PointOfInterestDto>(pointOfInterestToInsert);

            return CreatedAtRoute("GetPointOfInterest", new
            { cityId = cityId, id = pointOfInterestToInsert.Id }, createdPointOfInterest);
        }

        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id, [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            if (pointOfInterest == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();
            
            if (!_cityInfoService.CityExists(cityId))
                return NotFound();

            var pointOfInterestEntity = _cityInfoService.GetPointOfInterest(cityId, id);

            if (pointOfInterestEntity == null)
                return NotFound();

            Mapper.Map(pointOfInterest, pointOfInterestEntity);

            if (!_cityInfoService.Save())
            {
                return StatusCode(500, "Error while handling the request");
            }

            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id, [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            if (patchDocument == null)
                return BadRequest();

            if (!_cityInfoService.CityExists(cityId))
                return NotFound();

            var pointOfInterestEntity = _cityInfoService.GetPointOfInterest(cityId, id);

            if (pointOfInterestEntity == null)
                return NotFound();

            var pointOfInterestToPatch = Mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);
            
            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest();

            if (pointOfInterestToPatch.Name == pointOfInterestToPatch.Description)
                ModelState.AddModelError("Description", "Name and Description cannot be the same");

            TryValidateModel(pointOfInterestToPatch);


            if (!ModelState.IsValid)
                return BadRequest();

            Mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterest/id")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {

            if (!_cityInfoService.CityExists(cityId))
                return NotFound();

            var pointOfInterestEntity = _cityInfoService.GetPointOfInterest(cityId, id);

            if (pointOfInterestEntity == null)
                return NotFound();

            _cityInfoService.DeletePointOfInterest(pointOfInterestEntity);

            if (!_cityInfoService.Save())
            {
                return StatusCode(500, "Error while handling the request");
            }

            _mailService.Send("Point of interest deleted", $"Point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} has been deleted");

            return NoContent();
        }
    }
}
