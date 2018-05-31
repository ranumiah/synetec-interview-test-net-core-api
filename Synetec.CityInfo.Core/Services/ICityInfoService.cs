using System.Collections.Generic;
using Synetec.CityInfo.DataAccess.Entities;

namespace Synetec.CityInfo.Core.Services
{
    public interface ICityInfoService
    {
        IEnumerable<City> GetCities();
        City GetCity(int cityId);
    }
}
