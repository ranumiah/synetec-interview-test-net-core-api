using System.Collections.Generic;
using Synetec.CityInfo.Core.Exceptions;
using Synetec.CityInfo.DataAccess.Entities;
using Synetec.CityInfo.DataAccess.Repositories;

namespace Synetec.CityInfo.Core.Services
{
    public class CityInfoService : ICityInfoService
    {
        private readonly ICityRepository _cityRepository;

        public CityInfoService(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public IEnumerable<City> GetCities()
        {
            return _cityRepository.GetAll();
        }

        public City GetCity(int cityId)
        {
            var city = _cityRepository.GetById(cityId);

            if(city == null)
                throw new CityNotFoundException($"City with {cityId} does not exist");

            return city;
        }
    }
}
