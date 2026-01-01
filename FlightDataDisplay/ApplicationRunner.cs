using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using GenericsBasics.Application;
using GenericsBasics.Domain;
using Microsoft.Extensions.Hosting;

namespace GenericsBasics.Presentation
{
    class ApplicationRunner : IHostedService
    {
        private readonly BaggageHandler _provider;
        private readonly ArrivalsMonitor _observer1;
        private readonly IHostApplicationLifetime _appLifetime;
        //private readonly ArrivalsMonitor _observer2;

        public ApplicationRunner(BaggageHandler provider, ArrivalsMonitor observer1, IHostApplicationLifetime lifetime)
        {
            _provider = provider;
            _observer1 = observer1;
            _appLifetime = lifetime;

            //_observer2 = observer2;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);
            //_observer2.Subscribe(_provider);

            /*_provider.BaggageStatus(712, "Detroit", 3);
            Thread.Sleep(1000);*/
            _observer1.Subscribe(_provider);
            /*_provider.BaggageStatus(713, "Kalamazoo", 3);
            Thread.Sleep(1000);
            _provider.BaggageStatus(400, "New York-Kennedy", 1);
            Thread.Sleep(1000);
            _provider.BaggageStatus(712, "Detroit", 3);
            Thread.Sleep(1000);*/


            /*_provider.BaggageStatus(511, "San Francisco", 2);
            Thread.Sleep(1000);
            _provider.BaggageStatus(712);*/
            //_observer2.Unsubscribe();


            /*_provider.BaggageStatus(400);
            _provider.LastBaggageClaimed();*/


            await Task.CompletedTask;
        }

        private void OnStopping()
        {
            throw new NotImplementedException();
        }

        private void OnStopped()
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _observer1.Unsubscribe();

            return Task.FromCanceled(cancellationToken);
        }
    }
}