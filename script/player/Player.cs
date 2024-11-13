using Godot;

public partial class Player : CharacterBody2D
{
    public StateMashine StateMachine { get; private set; }

    public override void _Ready()
    {
        StateMachine = GetNode<StateMashine>("StateMachine");   //StateMashine-Node initialisieren
        StateMachine.TransitionTo("idle");                      // Initialer Zustand
    }

    public override void _PhysicsProcess(double delta)
    {
        StateMachine.CurrentState.Update(delta);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        StateMachine.CurrentState.HandleInput(@event);
    }
}
