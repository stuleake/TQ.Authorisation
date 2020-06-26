using Microsoft.EntityFrameworkCore;
using System;
using TQ.Geocoding.Data.Context.Addresses;

namespace TQ.Geocoding.API.Test.Context
{
    public class AddressTestInMemoryDbContextFactory
    {
        public AddressDbContext GetGeocodingDbContext()
        {
            var options = new DbContextOptionsBuilder<AddressDbContext>()
                            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                            .Options;
            return new AddressDbContext(options);
        }
    }
}