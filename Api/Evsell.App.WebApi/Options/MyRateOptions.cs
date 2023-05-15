using System.Threading.RateLimiting;

namespace Evsell.App.WebApi.Options
{
    public class MyRateOptions
    {
        public int PermitLimit { get; set; } = 10000;
        public TimeSpan Window { get; set; } = TimeSpan.FromSeconds(20);
        public int QueueLimit { get; set; } = 10000;
        public QueueProcessingOrder QueueProcessingOrder { get; set; } = QueueProcessingOrder.OldestFirst;
    }
}
