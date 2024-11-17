using Godot;

namespace INTOnlineCoop.Script.Player.States
{
    /// <summary>
    /// State used when the character is damaged by another player with a weapon
    /// </summary>
    public partial class TakeingDamage : State
    {
        /// <summary>
        /// reference to the usage of the AnimationPlayer build into AnimatedSprite2D with his SpriteFrames and Animation options
        /// </summary>
        [Export] private AnimatedSprite2D _figureAnimation;

        private bool _isdamaged;
        /// <summary>
        /// define what animation is played when entering the state
        /// </summary>
        public override void Enter()
        {
            base.Enter();
            _figureAnimation.Play("TakeingDamage(Flying)");
            if ((_figureAnimation.Animation == "TakeingDamage(Flying)") && (_figureAnimation.Frame == 3))
            {
                _figureAnimation.Play("DamageSprite(InAir)");
                if ((_figureAnimation.Animation == "DamageSprite(InAir)") && Character.IsOnFloor())
                {
                    _figureAnimation.Play("RecoveringDamage(Grounded)");
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
                    if ((_figureAnimation.Animation == "TakeingDamage(Flying)") && (_figureAnimation.Frame == 3))
                    {
                        _figureAnimation.Play("DamageSprite(InAir)");
                        if ((_figureAnimation.Animation == "DamageSprite(InAir)") && Character.IsOnFloor())
                        {
                            _figureAnimation.Stop();
                            _figureAnimation.Play("RecoveringDamage(Grounded)");
                            if ((_figureAnimation.Animation == "RecoveringDamage(Grounded)") && (_figureAnimation.Frame == 3))
                            {
                                _isdamaged = false;
                                Character.StateMachine.TransitionTo(AvailableState.Idle);
                            }
                        }
                    }
                }

            } while (_isdamaged == false);

        }
    }
}