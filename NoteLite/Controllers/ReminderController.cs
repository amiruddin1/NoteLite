using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace NoteLite.Controllers
{
    public class ReminderController : BackgroundService
    {
        private Timer _timer;
        private readonly IHubContext<NotificationHub> _hubContext;

        public ReminderController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _hubContext.Clients.All.SendAsync("ReceiveMessage", "It's time to drink water!");
        }

        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }
    }
}
