using System.Collections.Generic;
using Synetec.CityInfo.DataAccess.Entities;

namespace Synetec.CityInfo.DataAccess.Repositories
{
    public interface ICityRepository
    {
        IEnumerable<City> GetAll();
        City GetById(int cityId);
        void Delete(City cityToDelete);
    }
}
