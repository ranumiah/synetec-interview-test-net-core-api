using System.Collections.Generic;
using Synetec.CityInfo.DataAccess.Entities;

namespace Synetec.CityInfo.DataAccess.Contexts
{
    public static class CityInfoContext
    {
        public static IList<City> Cities { get; set; } = new List<City>()
            {
                new City()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "The one with that big park."
                },
                new City()
                {
                    Id = 2,
                    Name = "Antwerp",
                    Description = "The one with the cathedral that was never really finished."
                },
                new City()
                {
                    Id = 3,
                    Name = "Paris",
                    Description = "The one with that big tower.",
                }
            };
    }
}

