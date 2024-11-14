using Godot;

public partial class PlayerCharacter : CharacterBody2D
{
    [Export] public StateMachine StateMachine { get; private set; }

    public override void _Ready()
    {
        StateMachine = GetNode<StateMachine>("StateMachine");   //StateMashine-Node initialisieren
        StateMachine.TransitionTo("idle");                      // Initialer Zustand
    }

    public override void _PhysicsProcess(double delta)
    {
        StateMachine.CurrentState.PhysicProcess(delta);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        StateMachine.CurrentState.HandleInput(@event);
    }
}
