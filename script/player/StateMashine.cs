using Godot;

using System.Collections.Generic;

public partial class StateMashine : Node
{
    private readonly Dictionary<string, State> _states = new();     //State-Dictionary zum Speichern aller States anlegen
    public State CurrentState { get; private set; }                 //Aktuellen Status deklarieren

    public override void _Ready()
    {
        //Jeden Kind-Node des StateMashine-Nodes durchlaufen
        foreach (Node node in GetChildren())
        {
            //Wenn der Node vom Typ State ist, ins Dictionary aufnehmen
            if (node is State state)
            {
                _states.Add(node.Name.ToString().ToLower(), state); //States zum Dictionary hinzufügen
                GD.Print($"State registered: {node.Name}");         //Debugger-Code
            }
        }
    }

    public void TransitionTo(string stateName)
    {
        //Prüfen, ob der State, in den gewechselt werden soll, existiert
        if (_states.TryGetValue(stateName, out State newState))
        {
            CurrentState = newState;                        //In den neuen State wechseln
            CurrentState.Enter(GetParent<Player>());        //Spieler-Objekt im State hinterlegen
        }
        else
        {
            GD.PrintErr($"State {stateName} not found");    //Debugger-Code
        }
    }
}
