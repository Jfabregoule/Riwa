using System;
using System.Collections.Generic;
using System.Threading;

namespace HSM
{
    public class TransitionSequencer
    {
        public readonly StateMachine Machine;

        private ISequence _sequencer;
        private Action _nextPhase;
        private (State from, State to)? _pending;
        private State _lastFrom, _lastTo;

        public TransitionSequencer(StateMachine machine)
        {
            Machine = machine;
        }
        
        public void RequestTransition(State from, State to)
        {
            if (to == null || from == to) return;
            if (_sequencer != null)
            {
                _pending = (from, to); 
                return;
            }
            BeginTransition(from, to);
        }

        private static List<PhaseStep> GatherPhaseSteps(List<State> chain, bool deactivate)
        {
            List<PhaseStep> steps = new();

            foreach (State state in chain)
            {
                IReadOnlyList<IActivity> activities = state.Activities;

                foreach (IActivity activity in activities)
                {
                    if (deactivate)
                    {
                        if (activity.Mode != ActivityMode.Active) continue;
                        steps.Add(ct => activity.DeactivateAsync(ct));
                    }
                    else
                    {
                        if (activity.Mode != ActivityMode.Inactive) continue;
                        steps.Add(ct => activity.ActivateAsync(ct));
                    }
                }
            }
            return steps;
        }


        private static List<State> StatesToExit(State from, State lca)
        {
            List<State> states = new();
            for (State state = from; state != null && state != lca; state = state.Parent) states.Add(state);
            return states;
        }

        private static List<State> StatesToEnter(State to, State lca)
        {
            Stack<State> states = new();
            for (State state = to; state != lca; state = state.Parent) states.Push(state);
            return new List<State>(states);
        }

        private readonly CancellationTokenSource _tokenSource = new();
        public readonly bool UseSequential = true;
        
        private void BeginTransition(State from, State to)
        {
            State lca = Lca(from, to);
            List<State> exitChain = StatesToExit(from, lca);
            List<State> enterChain = StatesToEnter(to, lca);
            
            List<PhaseStep> exitSteps = GatherPhaseSteps(exitChain, deactivate: true);
            _sequencer = UseSequential ? new SequentialPhase(exitSteps, _tokenSource.Token) : new ParallelPhase(exitSteps, _tokenSource.Token);
            _sequencer.Start();

            _nextPhase = () =>
            {
                Machine.ChangeState(from, to);

                List<PhaseStep> enterSteps = GatherPhaseSteps(enterChain, deactivate: false);
                _sequencer = UseSequential ? new SequentialPhase(enterSteps, _tokenSource.Token) : new ParallelPhase(enterSteps, _tokenSource.Token);
                _sequencer.Start();
            };
        }

        private void EndTransition()
        {
            _sequencer = null;

            if (_pending.HasValue)
            {
                (State from, State to) phase = _pending.Value;
                _pending = null;
                BeginTransition(phase.from, phase.to);
            }
        }

        public void Tick(float deltaTime)
        {
            if (_sequencer != null)
            {
                if (!_sequencer.Update()) return;
                
                if (_nextPhase != null)
                {
                    Action next = _nextPhase;
                    _nextPhase = null;
                    next();
                }
                else
                {
                    EndTransition();
                }
                return;
            }
            Machine.InternalTick(deltaTime);
        }

        // Récupère l'ancêtre commun le plus proche entre deux state
        public static State Lca(State a, State b)
        {
            // Créé un Hashset de tous les parents du state 'a'
            HashSet<State> aParents = new();
            for (State state = a; state != null; state = state.Parent) aParents.Add(state);
            
            // Find the first parent of 'b' that is also a parent of 'a'
            for (State state = b; state != null; state = state.Parent)
                if (aParents.Contains(state)) return state;
            
            // If no common ancestor found, return null
            return null;
        }
    }
}