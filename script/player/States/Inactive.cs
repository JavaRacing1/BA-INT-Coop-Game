using Godot;

public class InactiveState : PlayerState
{
    public override void Enter(PlayerCharacter player)
    {
        base.Enter(player);
        GD.Print("Entering Inactive State");
        // Logik für den inaktiven Zustand
    }
}
