using Godot;

public abstract partial class State : Node
{
    public Player Spieler;
    protected const float Speed = 300f;
    protected const float Gravity = 800f;
    public virtual void Enter(Player player)
    {
        Spieler = player;
    }

    public virtual void Quit() { }

    public virtual void Update(double delta) { }

    public virtual void HandleInput(InputEvent inputEvent) { }
}
