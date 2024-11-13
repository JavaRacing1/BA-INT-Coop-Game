using Godot;

public partial class Inactive : PlayerState
{
    public override void Enter(Player player)
    {
        base.Enter(player);
        GD.Print("Entering inactive State");
        // Logik für den inaktiven Zustand
    }
}
