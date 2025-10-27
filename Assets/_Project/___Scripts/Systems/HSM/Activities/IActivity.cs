using System.Threading;
using System.Threading.Tasks;
using HSM;

namespace HSM
{
    public enum ActivityMode {Inactive, Activating, Active, Deactivating}

    public interface IActivity
    {
        ActivityMode Mode { get; }
        Task ActivateAsync(CancellationToken token);
        Task DeactivateAsync(CancellationToken token);
    }
}

public abstract class Activity : IActivity
{
    public ActivityMode Mode { get; protected set; } = ActivityMode.Inactive;

    public virtual async Task ActivateAsync(CancellationToken token)
    {
        if (Mode != ActivityMode.Inactive) return;
        
        Mode = ActivityMode.Activating;
        await Task.CompletedTask;
        Mode = ActivityMode.Active;
    }

    public virtual async Task DeactivateAsync(CancellationToken token)
    {
        if (Mode != ActivityMode.Active) return;
        
        Mode = ActivityMode.Deactivating;
        await Task.CompletedTask;
        Mode = ActivityMode.Inactive;
    }
}
