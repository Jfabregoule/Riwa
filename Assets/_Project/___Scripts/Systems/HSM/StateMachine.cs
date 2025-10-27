using System.Collections.Generic;

namespace HSM
{
    public class StateMachine
    {
        public readonly State Root;
        public readonly TransitionSequencer Sequencer;
        private bool _started;

        public StateMachine(State root)
        {
            Root = root;
            Sequencer = new TransitionSequencer(this);
        }

        public void Start()
        {
            if (_started) return;
            
            _started = true;
            Root.Enter();
        }

        public void Tick(float deltaTime)
        {
            if (!_started) Start();
            Sequencer.Tick(deltaTime);
        }
        
        internal void InternalTick(float deltaTime) => Root.Update(deltaTime);

        public void ChangeState(State from, State to)
        {
            if (from == to || from == null || to == null) return;
            
            State lca = TransitionSequencer.Lca(from, to);
            
            // Sortie de toutes les banches communes antérieures hors LCA
            for (State state = from; state != lca; state = state.Parent) state.Exit();
            
            // Entrée dans toutes les banches situées entre le LCA et la target
            Stack<State> stack = new();
            for (State state = to; state != lca; state = state.Parent) stack.Push(state);
            while (stack.Count > 0) stack.Pop().Enter();
        }
    }
}