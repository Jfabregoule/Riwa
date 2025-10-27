using System.Collections.Generic;
using System.Reflection;

namespace HSM
{
    public class StateMachineBuilder
    {
        private readonly State _root;

        public StateMachineBuilder(State root)
        {
            _root = root;
        }

        public StateMachine Build()
        {
            StateMachine machine = new StateMachine(_root);
            Wire(_root, machine, new HashSet<State>());
            return machine;
        }

        private void Wire(State state, StateMachine machine, HashSet<State> visited)
        {
            if (state == null) return;
            if (!visited.Add(state)) return; // Le State est deja wired
            
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
            FieldInfo machineField = typeof(State).GetField("Machine", flags);
            if (machineField != null) machineField.SetValue(state, machine);

            foreach (FieldInfo field in state.GetType().GetFields(flags))
            {
                if (!typeof(State).IsAssignableFrom(field.FieldType)) continue; // On ne considere que les fields qui sont des States
                if (field.Name == "Parent") continue; // On exclue le parent pour eviter une recursion infinie

                State child = (State) field.GetValue(state);
                if (child == null) continue;
                if (!ReferenceEquals(child.Parent, state)) continue; // On s'assure que c'est bien l'enfant direct
                
                Wire(child, machine, visited); // Recursion dans l'enfant
            }
        }
    }
}
