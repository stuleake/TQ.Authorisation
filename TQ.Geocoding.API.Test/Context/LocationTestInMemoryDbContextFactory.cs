using Microsoft.EntityFrameworkCore;
using System;
using TQ.Geocoding.Data.Context.Locations;

namespace TQ.Geocoding.API.Test.Context
{
    public class LocationTestInMemoryDbContextFactory
    {
        public LocationDbContext GetGeocodingLocationDbContext()
        {
            var options = new DbContextOptionsBuilder<LocationDbContext>()
                            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                            .Options;

            return new LocationDbContext(options);
        }
    }
}