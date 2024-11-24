using System;

using Godot;

using System.Collections.Generic;

namespace INTOnlineCoop.Script.Player
{
    /// <summary>
    /// States available for usage by the StateMachine
    /// </summary>
    public enum AvailableState
    {
        /// <summary>State when character is not moving but controllable</summary>
        Idle,

        /// <summary>State when character is falling</summary>
        Falling,

        /// <summary>State when character jumped</summary>
        Jumping,

        /// <summary>State when character is walking</summary>
        Walking,

        /// <summary>State when character takes damage</summary>
        TakingDamage,

        /// <summary>State when character dies of haveing no healthpoints left (healthpoints lower than 0)</summary>
        Dead
    }

    /// <summary>
    /// Manages the states of a player
    /// </summary>
    public partial class StateMachine : Node
    {
        private readonly Dictionary<AvailableState, State>
            _states = new(); //State-Dictionary zum Speichern aller States anlegen

        /// <summary>
        /// Current state of the state machine
        /// </summary>
        public State CurrentState { get; private set; }

        /// <summary>
        /// True if the player jumped
        /// </summary>
        [Export] public bool Jumped;

        /// <summary>
        /// Movement of the player
        /// </summary>
        [Export] public float Direction;

        /// <summary>
        /// Rotation of the item
        /// </summary>
        [Export] public float ItemRotation;

        /// <summary>
        /// Load all available states
        /// </summary>
        public override void _Ready()
        {
            //Jeden Kind-Node des StateMashine-Nodes durchlaufen
            foreach (Node node in GetChildren())
            {
                //Wenn der Node vom Typ State ist, ins Dictionary aufnehmen
                if (node is State state)
                {
                    bool parseOk = Enum.TryParse(state.Name, true, out AvailableState availableState);
                    if (!parseOk)
                    {
                        GD.PrintErr($"Could not parse state '{state.Name}'");
                        continue;
                    }

                    _states.Add(availableState, state); //States zum Dictionary hinzufügen
                }
            }
        }

        /// <summary>
        /// Changes the currently active state
        /// </summary>
        /// <param name="state">New state</param>
        public void TransitionTo(AvailableState state)
        {
            //Prüfen, ob der State, in den gewechselt werden soll, existiert
            if (_states.TryGetValue(state, out State newState))
            {
                CurrentState = newState; //In den neuen State wechseln
                CurrentState.Enter();
            }
            else
            {
                GD.PrintErr($"State {state} not found");
            }
        }

        /// <summary>
        /// Transfers the jump input to all peers
        /// </summary>
        [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
        public void Jump()
        {
            Jumped = true;
        }
    }
}