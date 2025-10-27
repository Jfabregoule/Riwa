namespace HSM
{
    public class AirborneState : State
    {
        private readonly CharacterContext _context;

        public AirborneState(StateMachine machine, State parent, CharacterContext context) : base(machine, parent)
        {
            _context = context;
        }
        
        protected override State GetTransition() => _context.Grounded ? ((CharacterRootState)Parent).GroundedState : null;

        protected override void OnEnter()
        {
            // Update Animator through _ctx.anim;
        }
    }
}
