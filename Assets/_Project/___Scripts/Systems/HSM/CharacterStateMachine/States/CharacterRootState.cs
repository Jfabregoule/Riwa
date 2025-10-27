namespace HSM
{
    public class CharacterRootState : State
    {
        public readonly GroundedState GroundedState;
        public readonly AirborneState AirborneState;
        public CharacterContext Context;

        public CharacterRootState(StateMachine machine, CharacterContext context) : base(machine, null)
        {
            Context = context;
            GroundedState = new GroundedState(machine, this, Context);
            AirborneState = new AirborneState(machine, this, Context);
        }


        protected override State GetInitialState() => GroundedState;
        protected override State GetTransition() => Context.Grounded ? null : AirborneState;
    }
}