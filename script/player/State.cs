using Godot;

public abstract partial class State : Node
{
    public StateMachine StateMachine { set; }
    public virtual void HandleInput(InputEvent @event) 
    {
        // Implement this method in derived classes
    }
    public virtual void Update(float delta) 
    {
        // Implement this method in derived classes
    }
    public virtual void PhysicsUpdate(float delta) 
    {
        // Implement this method in derived classes
    }
    public virtual void Exit() 
    {
        // Implement this method in derived classes
    }
}
