using Godot;
using System.Collections.Generic;

public class StateMachine : Node
{
    private Dictionary<string, State> states = new Dictionary<string, State>();
    public State CurrentState { get; private set; }

    public override void _Ready()
    {
        foreach (Node node in GetChildren())
        {
            if (node is State state)
            {
                states.Add(node.Name.ToLower(), state);
            }
        }
    }

    public void TransitionTo(string stateName)
    {
        if (CurrentState != null)
        {
            CurrentState.Exit();
        }

        if (states.TryGetValue(stateName.ToLower(), out State newState))
        {
            CurrentState = newState;
            CurrentState.Enter(GetParent<PlayerCharacter>());
        }
        else
        {
            GD.PrintErr($"State {stateName} not found");
        }
    }
}
