namespace INTOnlineCoop.Script.Player.States
{
    /// <summary>
    /// State used when the character is damaged by another player with a weapon
    /// </summary>
    public partial class TakingDamage : State
    {
        private bool _isdamaged;

        /// <summary>
        /// define what animation is played when entering the state
        /// </summary>
        public override void Enter()
        {
            base.Enter();
            CharacterSprite.Play("TakeingDamage(Flying)");
            if ((CharacterSprite.Animation == "TakeingDamage(Flying)") && (CharacterSprite.Frame == 3))
            {
                CharacterSprite.Play("DamageSprite(InAir)");
                if ((CharacterSprite.Animation == "DamageSprite(InAir)") && Character.IsOnFloor())
                {
                    CharacterSprite.Play("RecoveringDamage(Grounded)");
                }
            }
        }

        /// <summary>
        /// Updates player movement while getting hit and changeing played animation
        /// </summary>
        /// <param name="delta">Current frame delta</param>
        public override void PhysicProcess(double delta)
        {
            _isdamaged = true;
            do
            {
                if (!Character.IsOnFloor())
                {
                    if ((CharacterSprite.Animation == "TakeingDamage(Flying)") && (CharacterSprite.Frame == 3))
                    {
                        CharacterSprite.Play("DamageSprite(InAir)");
                        if ((CharacterSprite.Animation == "DamageSprite(InAir)") && Character.IsOnFloor())
                        {
                            CharacterSprite.Stop();
                            CharacterSprite.Play("RecoveringDamage(Grounded)");
                            if ((CharacterSprite.Animation == "RecoveringDamage(Grounded)") &&
                                (CharacterSprite.Frame == 3))
                            {
                                _isdamaged = false;
                                Character.StateMachine.TransitionTo(AvailableState.Idle);
                            }
                        }
                    }
                }
            } while (!_isdamaged);
        }
    }
}