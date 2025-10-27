using System.Collections.Generic;

namespace HSM
{
    public abstract class State
    {
        public readonly StateMachine Machine;
        public readonly State Parent;
        public State ActiveChild;
        private readonly List<IActivity> _activities = new List<IActivity>();
        public IReadOnlyList<IActivity> Activities => _activities;

        public State(StateMachine machine, State parent = null)
        {
            Machine = machine;
            Parent = parent;
        }
        
        public void Add(IActivity activity) { if (activity != null)  _activities.Add(activity); }

        protected virtual State GetInitialState() => null;
        protected virtual State GetTransition() => null;
        
        // Lifecycle hooks
        protected virtual void OnEnter() {}
        protected virtual void OnExit() {}
        protected virtual void OnUpdate(float deltaTime) {}

        internal void Enter()
        {
            if (Parent != null) Parent.ActiveChild = this;
            OnEnter();
            State init = GetInitialState();
            init?.Enter();
        }
        internal void Exit()
        {
            ActiveChild?.Exit();
            ActiveChild = null;
            OnExit();
        }
        internal void Update(float deltaTime)
        {
            State t = GetTransition();

            if (t != null)
            {
                Machine.Sequencer.RequestTransition(this, t);
                return;
            }
            
            if (ActiveChild != null) ActiveChild.Update(deltaTime);
            OnUpdate(deltaTime);
        }

        public State Leaf()
        {
            State s = this;
            while (s.ActiveChild != null) s = s.ActiveChild;
            return s;
        }

        public IEnumerable<State> PathToRoot()
        {
            for (State s = this; s != null; s = s.Parent) 
                yield return s;
        }
    }
}
