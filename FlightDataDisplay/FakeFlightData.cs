using System.Collections.Generic;
using System.Threading.Tasks;
using GenericsBasics.Domain;
using Faker;
using System.Timers;
using Microsoft.VisualBasic;
using System;
using System.Linq;

namespace GenericsBasics.Application
{
    class FakeFlightData : IFlightDataRepository
    {
        public FakeFlightData()
        {
        }
        public async Task<BaggageInfo> GetAllAsync()
        {
            return await Task.FromResult(GetBaggageInfo());
        }


        private BaggageInfo GetBaggageInfo()
        {
            return new BaggageInfo()
            {
                flight = Faker.RandomNumber.Next(500, 5000),
                from = Faker.Address.City(),
                carousel = Faker.RandomNumber.Next(0, 5)
            };
        }
    }

}