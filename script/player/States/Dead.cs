namespace INTOnlineCoop.Script.Player.States
{
    /// <summary>
    /// State used when the character dies because his healthpoints are lower than 0
    /// </summary>
    public partial class Dead : State
    {
        /// <summary>
        /// define what animation is played when entering the state
        /// </summary>
        public override void Enter()
        {
            base.Enter();
            CharacterSprite.Play("Dead");
        }
    }
}
