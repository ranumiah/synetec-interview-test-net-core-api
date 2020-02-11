using System.Collections.Generic;
using System.Linq;
using Synetec.CityInfo.DataAccess.Contexts;
using Synetec.CityInfo.DataAccess.Entities;

namespace Synetec.CityInfo.DataAccess.Repositories
{
    public class CityRepository : ICityRepository
    {
        public IEnumerable<City> GetAll()
        {
            return CityInfoContext.Cities;
        }

        public City GetById(int cityId)
        {
            return CityInfoContext.Cities.FirstOrDefault(x => x.Id == cityId);
        }

        public void Delete(City cityToDelete)
        {
            CityInfoContext.Cities.Remove(cityToDelete);
        }
    }
}
