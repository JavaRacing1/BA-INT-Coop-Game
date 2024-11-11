using Godot;

public abstract class State : Node
{
    public Player Spieler;
    public virtual void Enter(Player player)
    {
        this.Spieler = player;
    }

    /// public virtual void Exit(){}

    public virtual void Update(double delta) { }

    public virtual void HandleInput(InputEvent inputEvent) { }
}
