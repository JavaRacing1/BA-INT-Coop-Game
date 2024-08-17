using Godot;

public abstract class State : Node
{
    protected PlayerCharacter player;
    public virtual void Enter(PlayerCharacter player)
    {
        this.player = player;
    }
    public virtual void Exit() {}

    public virtual void Update(float delta) {}

    public virtual void HandleInput(InputEvent inputEvent) {}
}
