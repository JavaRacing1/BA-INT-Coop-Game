using Godot;

using Godot;

public class PlayerCharacter : CharacterBody2D
{
    public StateMachine StateMachine { get; private set; }

    public override void _Ready()
    {
        StateMachine = GetNode<StateMachine>("StateMachine");
        StateMachine.TransitionTo("idle"); // Initialer Zustand
    }

    public override void _PhysicsProcess(float delta)
    {
        StateMachine.CurrentState.Update(delta);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        StateMachine.CurrentState.HandleInput(@event);
    }
}

