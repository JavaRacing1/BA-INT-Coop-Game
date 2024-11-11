using System;
using System.Collections.Generic;

using Godot;
using Godot.Collections;

namespace INTOnlineCoop.Script.Singleton
{
    /// <summary>
    /// Available display modes
    /// </summary>
    public enum DisplayMode
    {
        ///<summary>Exclusive fullscreen mode</summary>
        Fullscreen,

        ///<summary>Windowed mode</summary>
        Window
    }

    /// <summary>
    /// Available input modes
    /// </summary>
    public enum InputType
    {
        /// <summary>Keyboard input</summary>
        Key,

        /// <summary>Controller button input</summary>
        JoyButton,

        /// <summary>Controller axis input</summary>
        JoyAxis
    }

    /// <summary>
    /// The type of the input
    /// </summary>
    public enum InputKind
    {
        /// <summary>Primary input</summary>
        Primary,

        /// <summary>Secondary input</summary>
        Secondary
    }

    /// <summary>
    /// Singleton node for saving and accessing the settings of the player
    /// </summary>
    public partial class PlayerSettingsData : Node
    {
        /// <summary>
        /// Array with all available input actions
        /// </summary>
        public static readonly string[] InputActions =
        {
            "walk_left", "walk_right", "jump", "aim_up", "aim_down", "pause", "open_item_inv", "use_item",
            "modify_item_primary", "modify_item_secondary", "camera_left", "camera_up", "camera_right",
            "camera_down"
        };

        private const string SettingsPath = "user://settings.dat";
        private string _displayMode = "Fullscreen";
        private bool _changedDisplaySettings;

        private Godot.Collections.Dictionary<string, Godot.Collections.Dictionary<string, Variant>> _controlSettings =
            CreateDefaultControls();

        /// <summary>
        /// The currently selected display mode
        /// </summary>
        public DisplayMode SelectedDisplayMode => Enum.Parse<DisplayMode>(_displayMode);

        /// <summary>
        /// The particle status
        /// </summary>
        /// <returns>true if particles are enabled</returns>
        public bool AreParticlesEnabled { get; private set; } = true;

        /// <summary>
        /// The master volume of the game
        /// </summary>
        public int MasterVolume { get; private set; } = 100;

        /// <summary>
        /// The music volume of the game
        /// </summary>
        public int MusicVolume { get; private set; } = 100;

        /// <summary>
        /// The effect volume of the game
        /// </summary>
        public int EffectVolume { get; private set; } = 100;

        /// <summary>
        /// The visibility of control hints
        /// </summary>
        public bool ShowControlHints { get; private set; } = true;

        /// <summary>
        /// Returns the status of unsaved changes
        /// </summary>
        /// <value>true if a save to file is pending</value>
        public bool HasUnsavedChanges { get; private set; }

        /// <summary>
        /// Returns the status of changes to the control
        /// </summary>
        public bool HasControlChanges { get; private set; }

        /// <summary>
        /// Initializes the settings file
        /// </summary>
        public override void _Ready()
        {
            if (!FileAccess.FileExists(SettingsPath))
            {
                Save();
            }
            else
            {
                Load();
            }

            UpdateWindow(true);
            ApplyInputSettings();
        }

        /// <summary>
        /// Sets the display mode of the player
        /// </summary>
        /// <param name="displayMode">The new display mode of the player</param>
        public void SetDisplayMode(DisplayMode displayMode)
        {
            _displayMode = displayMode.ToString();
            HasUnsavedChanges = true;
            _changedDisplaySettings = true;
        }

        /// <summary>
        /// Sets the particle activation status
        /// </summary>
        /// <param name="enabled">True if particles should be enabled</param>
        public void SetParticlesEnabled(bool enabled)
        {
            AreParticlesEnabled = enabled;
            HasUnsavedChanges = true;
        }

        /// <summary>
        /// Changes the master volume
        /// </summary>
        /// <param name="volume">The new volume</param>
        public void SetMasterVolume(int volume)
        {
            MasterVolume = volume;
            HasUnsavedChanges = true;
        }

        /// <summary>
        /// Changes the music volume
        /// </summary>
        /// <param name="volume">The new volume</param>
        public void SetMusicVolume(int volume)
        {
            MusicVolume = volume;
            HasUnsavedChanges = true;
        }

        /// <summary>
        /// Changes the effect volume
        /// </summary>
        /// <param name="volume">The new volume</param>
        public void SetEffectVolume(int volume)
        {
            EffectVolume = volume;
            HasUnsavedChanges = true;
        }

        /// <summary>
        /// Changes the visibility of control hints
        /// </summary>
        /// <param name="visible">True if control hints should be visible</param>
        public void SetControlHintVisibility(bool visible)
        {
            ShowControlHints = visible;
            HasUnsavedChanges = true;
        }

        /// <summary>
        /// Saves and applies the pending setting changes
        /// </summary>
        public void ApplyChanges()
        {
            Save();
            UpdateWindow(false);
            HasUnsavedChanges = false;
            _changedDisplaySettings = false;
            if (HasControlChanges)
            {
                ApplyInputSettings();
            }
            HasControlChanges = false;
        }

        /// <summary>
        /// Resets the pending setting changes
        /// </summary>
        public void DiscardChanges()
        {
            Load();
            HasUnsavedChanges = false;
            _changedDisplaySettings = false;
            HasControlChanges = false;
        }

        /// <summary>
        /// Changes the input for an action
        /// </summary>
        /// <param name="action">The name of the input action</param>
        /// <param name="kind">The kind of input (primary or secondary)</param>
        /// <param name="input">The new input</param>
        /// <param name="inputType">The type of the new input</param>
        public void SetInput(string action, InputKind kind, string input, InputType inputType)
        {
            if (!_controlSettings.ContainsKey(action))
            {
                GD.PrintErr($"Invalid action {action}!");
                return;
            }

            Godot.Collections.Dictionary<string, Variant> inputSetting = _controlSettings.GetValueOrDefault(action);
            inputSetting[kind.ToString()] = input;
            inputSetting[kind + "Type"] = inputType.ToString();
            _controlSettings[action] = inputSetting;

            HasUnsavedChanges = true;
            HasControlChanges = true;
        }

        /// <summary>
        /// Returns the configured input for an action
        /// </summary>
        /// <param name="action">The name of the action</param>
        /// <param name="kind">The input kind (primary or secondary)</param>
        /// <returns>The value and type of the input</returns>
        public (string, InputType) GetInput(string action, InputKind kind)
        {
            if (!_controlSettings.ContainsKey(action))
            {
                GD.PrintErr($"Invalid action {action}!");
                return (null, InputType.Key);
            }

            Godot.Collections.Dictionary<string, Variant> inputSetting = _controlSettings.GetValueOrDefault(action);
            return ((string)inputSetting[kind.ToString()], Enum.Parse<InputType>((string)inputSetting[kind + "Type"]));
        }

        private static Godot.Collections.Dictionary<string, Godot.Collections.Dictionary<string, Variant>> CreateDefaultControls()
        {
            return new()
            {
                { "walk_left", CreateControlSetting("A", InputType.Key, "-LeftX", InputType.JoyAxis) },
                { "walk_right", CreateControlSetting("D", InputType.Key, "+LeftX", InputType.JoyAxis) },
                { "jump", CreateControlSetting("Space", InputType.Key, "A", InputType.JoyButton) },
                { "aim_up", CreateControlSetting("W", InputType.Key, "-LeftY", InputType.JoyAxis) },
                { "aim_down", CreateControlSetting("S", InputType.Key, "+LeftY", InputType.JoyAxis) },
                { "pause", CreateControlSetting("Escape", InputType.Key, "Start", InputType.JoyButton) },
                { "open_item_inv", CreateControlSetting("E", InputType.Key, "B", InputType.JoyButton) },
                { "use_item", CreateControlSetting("Q", InputType.Key, "+TriggerRight", InputType.JoyAxis) },
                { "modify_item_primary", CreateControlSetting("R", InputType.Key, "X", InputType.JoyButton) },
                { "modify_item_secondary", CreateControlSetting("F", InputType.Key, "Y", InputType.JoyButton) },
                { "camera_left", CreateControlSetting("Left", InputType.Key, "-RightX", InputType.JoyAxis) },
                { "camera_up", CreateControlSetting("Up", InputType.Key, "-RightY", InputType.JoyAxis) },
                { "camera_right", CreateControlSetting("Right", InputType.Key, "+RightX", InputType.JoyAxis) },
                { "camera_down", CreateControlSetting("Down", InputType.Key, "+RightY", InputType.JoyAxis) }
            };
        }

        private static Godot.Collections.Dictionary<string, Variant> CreateControlSetting(String primaryInput,
            InputType primaryType, String secondaryInput, InputType secondaryType)
        {
            return new Godot.Collections.Dictionary<string, Variant>()
            {
                { "Primary", primaryInput },
                { "PrimaryType", primaryType.ToString() },
                { "Secondary", secondaryInput },
                { "SecondaryType", secondaryType.ToString() }
            };
        }


        private static InputEvent CreateInput(string input, InputType type)
        {
            return type switch
            {
                InputType.Key => CreateKeyInput(input),
                InputType.JoyButton => CreateJoyButtonInput(input),
                InputType.JoyAxis => CreateJoyMotionInput(input),
                _ => null
            };
        }

        private static InputEventKey CreateKeyInput(string input)
        {
            Key key = Enum.Parse<Key>(input);
            InputEventKey keyEvent = new() { KeyLabel = key };
            return keyEvent;
        }

        private static InputEventJoypadButton CreateJoyButtonInput(string input)
        {
            JoyButton button = Enum.Parse<JoyButton>(input);
            InputEventJoypadButton buttonEvent = new() { ButtonIndex = button };
            return buttonEvent;
        }

        private static InputEventJoypadMotion CreateJoyMotionInput(string input)
        {
            float value = input[0].Equals('+') ? 1.0f : -1.0f;
            JoyAxis axis = Enum.Parse<JoyAxis>(input[1..]);
            InputEventJoypadMotion motionEvent = new() { Axis = axis, AxisValue = value };
            return motionEvent;
        }

        private void ApplyInputSettings()
        {
            foreach (string action in InputActions)
            {
                InputMap.EraseAction(action);
            }

            foreach (string action in _controlSettings.Keys)
            {
                InputMap.AddAction(action, 0.1f);
                Godot.Collections.Dictionary<string, Variant> actionSettings =
                    _controlSettings.GetValueOrDefault(action);

                InputEvent primaryInput = CreateInput((string)actionSettings["Primary"],
                    Enum.Parse<InputType>((string)actionSettings["PrimaryType"]));
                InputEvent secondaryInput = CreateInput((string)actionSettings["Secondary"],
                    Enum.Parse<InputType>((string)actionSettings["SecondaryType"]));

                InputMap.ActionAddEvent(action, primaryInput);
                InputMap.ActionAddEvent(action, secondaryInput);
            }
        }

        private void UpdateWindow(bool forceUpdate)
        {
            if (_changedDisplaySettings || forceUpdate)
            {
                DisplayServer.WindowMode newMode = SelectedDisplayMode switch
                {
                    DisplayMode.Window => DisplayServer.WindowMode.Windowed,
                    DisplayMode.Fullscreen => DisplayServer.WindowMode.ExclusiveFullscreen,
                    _ => DisplayServer.WindowMode.ExclusiveFullscreen
                };
                DisplayServer.WindowSetMode(newMode);
            }
        }

        private void Load()
        {
            if (!FileAccess.FileExists(SettingsPath))
            {
                GD.PrintErr("Tried to load a non-existing savefile!");
                return;
            }

            using FileAccess saveFile = FileAccess.Open(SettingsPath, FileAccess.ModeFlags.Read);
            if (saveFile == null)
            {
                Error fileError = FileAccess.GetOpenError();
                GD.PrintErr($"File Error: {fileError}");
                return;
            }

            string fileData = saveFile.GetAsText();
            Json json = new();
            Error parseResult = json.Parse(fileData);
            if (parseResult != Error.Ok)
            {
                GD.PrintErr($"JSON Parse Error: {json.GetErrorMessage()} in {fileData} at line {json.GetErrorLine()}");
            }

            Godot.Collections.Dictionary<string, Variant> dataDict = new((Dictionary)json.Data);

            //Write variables
            _displayMode = (string)CollectionExtensions.GetValueOrDefault(dataDict, "DisplayMode", "Fullscreen");
            AreParticlesEnabled = (bool)CollectionExtensions.GetValueOrDefault(dataDict, "ParticlesEnabled", true);
            MasterVolume = Math.Clamp((int)CollectionExtensions.GetValueOrDefault(dataDict, "MasterVolume", 100), 0,
                100);
            MusicVolume = Math.Clamp((int)CollectionExtensions.GetValueOrDefault(dataDict, "MusicVolume", 100), 0, 100);
            EffectVolume = Math.Clamp((int)CollectionExtensions.GetValueOrDefault(dataDict, "EffectVolume", 100), 0,
                100);
            ShowControlHints = (bool)CollectionExtensions.GetValueOrDefault(dataDict, "ShowControlHints", true);
            _controlSettings =
                (Godot.Collections.Dictionary<string, Godot.Collections.Dictionary<string, Variant>>)
                CollectionExtensions
                    .GetValueOrDefault(dataDict, "Controls", CreateDefaultControls());
        }

        private void Save()
        {
            Godot.Collections.Dictionary<string, Variant> dataDict = new()
            {
                { "DisplayMode", _displayMode },
                { "ParticlesEnabled", AreParticlesEnabled },
                { "MasterVolume", MasterVolume },
                { "MusicVolume", MusicVolume },
                { "EffectVolume", EffectVolume },
                { "ShowControlHints", ShowControlHints },
                { "Controls", _controlSettings }
            };
            string jsonData = Json.Stringify(dataDict);

            using FileAccess saveFile = FileAccess.Open(SettingsPath, FileAccess.ModeFlags.Write);
            if (saveFile == null)
            {
                Error fileError = FileAccess.GetOpenError();
                GD.PrintErr($"File Error: {fileError}");
                return;
            }

            saveFile.StoreLine(jsonData);
        }
    }
}