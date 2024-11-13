using Godot;

public abstract partial class State : Node
{
    public Player Spieler;                      //Spieler initialisieren
    protected const float Speed = 50f;         //Spielergeschwindigkeit
    protected const float Gravity = 100f;       //Gravitation
    public virtual void Enter(Player player)
    {
        Spieler = player;
    }

    public virtual void Update(double delta) { }

    public virtual void HandleInput(InputEvent inputEvent) { }
}
