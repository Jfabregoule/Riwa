using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HSM
{
    public delegate Task PhaseStep(CancellationToken token);

    public class SequentialPhase : ISequence
    {
        private readonly List<PhaseStep> _steps;
        private readonly CancellationToken _token;
        private int _index = -1;
        private Task _current;

        public bool IsDone { get; private set; }

        public SequentialPhase(List<PhaseStep> steps, CancellationToken token)
        {
            _steps = steps;
            _token = token;
        }

        public void Start() => Next();

        public bool Update()
        {
            if (IsDone) return true;
            if (_current == null || _current.IsCompleted) Next();
            return IsDone;
        }

        private void Next()
        {
            _index++;
            if (_index >= _steps.Count) {IsDone = true; return;}
            _current = _steps[_index](_token);
        }
    }
}
