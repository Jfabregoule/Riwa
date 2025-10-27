using UnityEngine;

namespace HSM
{
    public class MoveState : State
    {
        private readonly CharacterContext _context;

        public MoveState(StateMachine machine, State parent, CharacterContext context) : base(machine, parent)
        {
            _context = context;
        }

        protected override State GetTransition()
        {
            if (!_context.Grounded) return ((CharacterRootState)Parent).AirborneState;

            return Mathf.Abs(_context.Move.x) <= 0.01f && Mathf.Abs(_context.Move.z) <= 0.01f ? ((GroundedState)Parent).IdleState : null;
        }

        protected override void OnUpdate(float deltaTime)
        {
            Vector3 targetVelocity = new(_context.Move.x * _context.MoveSpeed, _context.Velocity.y, _context.Move.z * _context.MoveSpeed);
            
            _context.Velocity.x = Mathf.MoveTowards(_context.Velocity.x, targetVelocity.x, _context.Acceleration * deltaTime);
            _context.Velocity.z = Mathf.MoveTowards(_context.Velocity.z, targetVelocity.z, _context.Acceleration * deltaTime);
        }
    }
}
