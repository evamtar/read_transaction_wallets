



using Microsoft.Extensions.Hosting;
using System.Timers;

namespace SyncronizationBot.Service.HostedServices
{
    public class TestService : BackgroundService
    {
        protected System.Timers.Timer? Timer { get; set; }

        public TestService() { }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine($"DateTime --> {DateTime.Now}");
            Timer = new System.Timers.Timer();
            Timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            Timer.Interval = 10000;
            Timer.Enabled = true;
            Timer.AutoReset = true;
            Timer?.Start();
            return Task.CompletedTask;
        }
        private async void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            TryStop();
            Console.WriteLine($"DateTime Elapsed --> {DateTime.Now}");
            Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
            await Task.Delay(12000);
            TryStart();
        }
        
        public override void Dispose()
        {
            try 
            {
                TryStop();
                Timer?.Dispose();
            }
            finally 
            {
                
            }
            base.Dispose();
        }

        private void TryStop() 
        {
            try
            {
                Timer?.Stop();
            }
            finally{ }
        }
        private void TryStart() 
        {
            try { Timer?.Start(); }
            finally{ }
        }
    }
}
