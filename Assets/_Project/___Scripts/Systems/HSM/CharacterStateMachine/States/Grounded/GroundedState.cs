namespace HSM
{
    public class GroundedState : State
    {
        private readonly CharacterContext _context;
        public readonly IdleState IdleState;
        public readonly MoveState MoveState;

        public GroundedState(StateMachine machine, State parent, CharacterContext context) : base(machine, parent)
        {
            _context = context;
            IdleState = new IdleState(machine, this, context);
            MoveState = new MoveState(machine, this, context);
            Add(new DelayActivationActivity() { Seconds = .5f});
        }
        
        protected override State GetInitialState() => IdleState;

        protected override State GetTransition() => _context.Grounded ? null : ((CharacterRootState)Parent).AirborneState;
    }
}