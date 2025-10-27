using System;
using System.Threading;
using System.Threading.Tasks;

namespace HSM
{
    public class DelayActivationActivity : Activity
    {
        public float Seconds = .2f;

        public override async Task ActivateAsync(CancellationToken token)
        {
            await Task.Delay(TimeSpan.FromSeconds(Seconds), token);
            await base.ActivateAsync(token);
        }
    }
}
