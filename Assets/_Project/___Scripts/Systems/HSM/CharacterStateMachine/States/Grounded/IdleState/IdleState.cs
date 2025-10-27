using UnityEngine;

namespace HSM
{
    public class IdleState : State
    {
        readonly CharacterContext _context;

        public IdleState(StateMachine machine, State parent, CharacterContext context) : base(machine, parent)
        {
            _context = context;
        }

        protected override State GetTransition()
        {
            return Mathf.Abs(_context.Move.x) > 0.01f || Mathf.Abs(_context.Move.z) > 0.01f ? ((GroundedState)Parent).MoveState : null;
        }

        protected override void OnEnter()
        {
            _context.Velocity.x = 0f;
            _context.Velocity.z = 0f;
        }
    }
}
