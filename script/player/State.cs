using Godot;

public abstract partial class State : Node
{
    protected const float Speed = 50f;         //Spielergeschwindigkeit
    protected const float Gravity = 100f;       //Gravitation
    protected PlayerCharacter Character { get; private set; }                      //Spieler initialisieren
    public virtual void Enter(PlayerCharacter player)
    {
        Character = player;
    }

    public virtual void Update(double delta) { }

    public virtual void HandleInput(InputEvent inputEvent) { }
}
