using System.Threading;
using System.Threading.Tasks;

namespace HSM
{
    public interface ISequence
    {
        bool IsDone { get; }
        void Start();
        bool Update();
    }
}
