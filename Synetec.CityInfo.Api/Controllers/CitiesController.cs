using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Synetec.CityInfo.Api.Dtos;
using Synetec.CityInfo.Core.Exceptions;
using Synetec.CityInfo.Core.Services;
using Synetec.CityInfo.DataAccess.Entities;

namespace Synetec.CityInfo.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class CitiesController : Controller
    {
        private readonly ICityInfoService _cityInfoService;

        public CitiesController(ICityInfoService cityInfoService)
        {
            _cityInfoService = cityInfoService;
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            var cities = _cityInfoService.GetCities();

            if (!cities.Any())
                return NoContent();

            var citiesMapped = cities.Select(MapCity);

            return Ok(citiesMapped);
        }
        
        [HttpGet("{cityId:int}")]
        public IActionResult GetCity(int cityId)
        {
            try
            {
                var city = _cityInfoService.GetCity(cityId);
                var cityMapped = MapCity(city);

                return Ok(cityMapped);
            }
            catch (CityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private static CityDto MapCity(City city)
        {
            return new CityDto
            {
                Id = city.Id,
                Description = city.Description,
                Name = city.Name
            };
        }
    }
}
