using Godot;

using System.Collections.Generic;

public class StateMachine : Node
{
    private Dictionary<string, State> _states = new();
    public State CurrentState { get; private set; }

    public override void _Ready()
    {
        foreach (Node node in GetChildren())
        {
            if (node is State state)
            {
                _states.Add(node.Name.ToString().ToLower(), state);
            }
        }
    }

    public void TransitionTo(string stateName)
    {
        if (CurrentState != null)
        {
            ///CurrentState.Exit();
        }

        if (_states.TryGetValue(stateName.ToLower(), out State newState))
        {
            CurrentState = newState;
            CurrentState.Enter(GetParent<Player>());
        }
        else
        {
            GD.PrintErr($"State {stateName} not found");
        }
    }
}
