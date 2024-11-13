using Godot;

using System.Collections.Generic;

public partial class StateMashine : Node
{
    private readonly Dictionary<string, State> _states = new();
    public State CurrentState { get; private set; }

    public override void _Ready()
    {
        foreach (Node node in GetChildren())
        {
            if (node is State state)
            {
                _states.Add(node.Name.ToString().ToLower(), state);
                GD.Print($"State registered: {node.Name}");
            }
        }
    }

    public void TransitionTo(string stateName)
    {
        CurrentState?.Quit();

        if (_states.TryGetValue(stateName, out State newState))
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
