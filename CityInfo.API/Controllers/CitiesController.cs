using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoService _cityInfoService;

        public CitiesController(ICityInfoService cityInfoService)
        {
            _cityInfoService = cityInfoService;
        }

        [HttpGet()]
        public IActionResult GetCities()
        {
            var cities = _cityInfoService.GetCities();

            var results = Mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cities);
             
            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = _cityInfoService.GetCity(id, includePointsOfInterest);
            if(city == null)
            {
                return NotFound();
            }
            
            if (includePointsOfInterest)
            {
                var cityResult = Mapper.Map<CityDto>(city);
                
                return Ok(cityResult);
            }

            var cityWithoutPointsOfInterestResult = Mapper.Map<CityWithoutPointsOfInterestDto>(city);

            return Ok(cityWithoutPointsOfInterestResult);
        }
    }
}
