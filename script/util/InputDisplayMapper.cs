using System.Collections.Generic;
using System.Collections.Immutable;

using Godot;

namespace INTOnlineCoop.Script.Util
{
    /// <summary>
    /// Converts InputActions and Keys to user-friendly names
    /// </summary>
    public static class InputDisplayMapper
    {
        private static readonly ImmutableDictionary<Key, string> KeyMap = ImmutableDictionary.CreateRange(
            new[]
            {
                KeyValuePair.Create(Key.Space, "Leertaste"), KeyValuePair.Create(Key.Exclam, "!"),
                KeyValuePair.Create(Key.Quotedbl, "\""), KeyValuePair.Create(Key.Numbersign, "#"),
                KeyValuePair.Create(Key.Dollar, "$"), KeyValuePair.Create(Key.Percent, "%"),
                KeyValuePair.Create(Key.Ampersand, "&"), KeyValuePair.Create(Key.Apostrophe, "'"),
                KeyValuePair.Create(Key.Parenleft, "("), KeyValuePair.Create(Key.Parenright, ")"),
                KeyValuePair.Create(Key.Asterisk, "*"), KeyValuePair.Create(Key.Plus, "+"),
                KeyValuePair.Create(Key.Comma, ","), KeyValuePair.Create(Key.Minus, "-"),
                KeyValuePair.Create(Key.Period, "."), KeyValuePair.Create(Key.Slash, "/"),
                KeyValuePair.Create(Key.Key0, "0"), KeyValuePair.Create(Key.Key1, "1"),
                KeyValuePair.Create(Key.Key2, "2"), KeyValuePair.Create(Key.Key3, "3"),
                KeyValuePair.Create(Key.Key4, "4"), KeyValuePair.Create(Key.Key5, "5"),
                KeyValuePair.Create(Key.Key6, "6"), KeyValuePair.Create(Key.Key7, "7"),
                KeyValuePair.Create(Key.Key8, "8"), KeyValuePair.Create(Key.Key9, "9"),
                KeyValuePair.Create(Key.Colon, ":"), KeyValuePair.Create(Key.Semicolon, ";"),
                KeyValuePair.Create(Key.Less, "<"), KeyValuePair.Create(Key.Equal, "="),
                KeyValuePair.Create(Key.Greater, ">"), KeyValuePair.Create(Key.Question, "?"),
                KeyValuePair.Create(Key.At, "@"), KeyValuePair.Create(Key.Bracketleft, "["),
                KeyValuePair.Create(Key.Backslash, "\\"), KeyValuePair.Create(Key.Bracketright, "]"),
                KeyValuePair.Create(Key.Asciicircum, "^"), KeyValuePair.Create(Key.Underscore, "_"),
                KeyValuePair.Create(Key.Quoteleft, "`"), KeyValuePair.Create(Key.Braceleft, "{"),
                KeyValuePair.Create(Key.Bar, "|"), KeyValuePair.Create(Key.Braceright, "}"),
                KeyValuePair.Create(Key.Asciitilde, "~"), KeyValuePair.Create(Key.Section, "§"),
                KeyValuePair.Create(Key.Escape, "Esc"), KeyValuePair.Create(Key.KpEnter, "Num_Enter"),
                KeyValuePair.Create(Key.Insert, "Einfg"), KeyValuePair.Create(Key.Delete, "Entf"),
                KeyValuePair.Create(Key.Pause, "Pause"), KeyValuePair.Create(Key.Print, "Druck"),
                KeyValuePair.Create(Key.Home, "Pos1"), KeyValuePair.Create(Key.End, "Ende"),
                KeyValuePair.Create(Key.Left, "Links"), KeyValuePair.Create(Key.Up, "Oben"),
                KeyValuePair.Create(Key.Right, "Rechts"), KeyValuePair.Create(Key.Down, "Unten"),
                KeyValuePair.Create(Key.Pageup, "Bild Hoch"), KeyValuePair.Create(Key.Pagedown, "Bild Runter"),
                KeyValuePair.Create(Key.Shift, "Umschalt"), KeyValuePair.Create(Key.Ctrl, "Strg"),
                KeyValuePair.Create(Key.Scrolllock, "Rollen"), KeyValuePair.Create(Key.KpMultiply, "Num_*"),
                KeyValuePair.Create(Key.KpDivide, "Num_/"), KeyValuePair.Create(Key.KpSubtract, "Num_-"),
                KeyValuePair.Create(Key.KpPeriod, "Num_."), KeyValuePair.Create(Key.KpAdd, "Num_+"),
                KeyValuePair.Create(Key.Kp0, "Num_0"), KeyValuePair.Create(Key.Kp1, "Num_1"),
                KeyValuePair.Create(Key.Kp2, "Num_2"), KeyValuePair.Create(Key.Kp3, "Num_3"),
                KeyValuePair.Create(Key.Kp4, "Num_4"), KeyValuePair.Create(Key.Kp5, "Num_5"),
                KeyValuePair.Create(Key.Kp6, "Num_6"), KeyValuePair.Create(Key.Kp7, "Num_7"),
                KeyValuePair.Create(Key.Kp8, "Num_8"), KeyValuePair.Create(Key.Kp9, "Num_9")
            });

        private static readonly ImmutableDictionary<string, string> InputActionMap = ImmutableDictionary.CreateRange(
            new[]
            {
                KeyValuePair.Create("walk_left", "Bewegen (Links)"), KeyValuePair.Create("walk_right", "Bewegen (Rechts)"),
                KeyValuePair.Create("jump", "Springen"), KeyValuePair.Create("aim_up", "Zielen (Hoch)"),
                KeyValuePair.Create("aim_down", "Zielen (Runter)"), KeyValuePair.Create("pause", "Pausieren"),
                KeyValuePair.Create("open_item_inv", "Inventar öffnen"), KeyValuePair.Create("use_item", "Gegenstand benutzen"),
                KeyValuePair.Create("modify_item_primary", "Waffe einstellen (Prim.)"), KeyValuePair.Create("modify_item_secondary", "Waffe einstellen (Sek.)"),
                KeyValuePair.Create("camera_left", "Kamera bewegen (Links)"), KeyValuePair.Create("camera_up", "Kamera bewegen (Hoch)"),
                KeyValuePair.Create("camera_right", "Kamera bewegen (Rechts)"), KeyValuePair.Create("camera_down", "Kamera bewegen (Runter)")
            });

        private static readonly ImmutableDictionary<JoyButton, string> JoyButtonMap = ImmutableDictionary.CreateRange(
            new[]
            {
                KeyValuePair.Create(JoyButton.Back, "CON-Zurück"), KeyValuePair.Create(JoyButton.Guide, "CON-Home"),
                KeyValuePair.Create(JoyButton.LeftStick, "CON-LS"),
                KeyValuePair.Create(JoyButton.RightStick, "CON-RS"),
                KeyValuePair.Create(JoyButton.LeftShoulder, "CON-LB"),
                KeyValuePair.Create(JoyButton.RightShoulder, "CON-RB"),
                KeyValuePair.Create(JoyButton.DpadUp, "CON-DPAD: Hoch"),
                KeyValuePair.Create(JoyButton.DpadLeft, "CON-DPAD: Links"),
                KeyValuePair.Create(JoyButton.DpadDown, "CON-DPAD: Runter"),
                KeyValuePair.Create(JoyButton.DpadRight, "CON-DPAD: Rechts"),
            });

        private static readonly ImmutableDictionary<(JoyAxis, float), string> JoyAxisMap =
            ImmutableDictionary.CreateRange(
                new[]
                {
                    KeyValuePair.Create((JoyAxis.LeftX, -1.0f), "CON-LS: Links"),
                    KeyValuePair.Create((JoyAxis.LeftX, 1.0f), "CON-LS: Rechts"),
                    KeyValuePair.Create((JoyAxis.LeftY, -1.0f), "CON-LS: Hoch"),
                    KeyValuePair.Create((JoyAxis.LeftY, 1.0f), "CON-LS: Runter"),
                    KeyValuePair.Create((JoyAxis.RightX, -1.0f), "CON-RS: Links"),
                    KeyValuePair.Create((JoyAxis.RightX, 1.0f), "CON-RS: Rechts"),
                    KeyValuePair.Create((JoyAxis.RightY, -1.0f), "CON-RS: Hoch"),
                    KeyValuePair.Create((JoyAxis.RightY, 1.0f), "CON-RS: Runter"),
                    KeyValuePair.Create((JoyAxis.TriggerLeft, 1.0f), "CON-LT"),
                    KeyValuePair.Create((JoyAxis.TriggerRight, 1.0f), "CON-RT")
                });

        /// <summary>
        /// Returns the name of a Key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The name of the key</returns>
        public static string GetKeyName(Key key)
        {
            string keyName = key.ToString();
            // Convert special keycodes without an enum entry (e.g. Ä,ß) to readable strings 
            if (int.TryParse(keyName, out _))
            {
                keyName = OS.GetKeycodeString(key);
            }

            if (KeyMap.ContainsKey(key))
            {
                keyName = KeyMap.GetValueOrDefault(key);
            }

            return keyName;
        }

        /// <summary>
        /// Returns the name of an action
        /// </summary>
        /// <param name="action">The input action</param>
        /// <returns>The name of the action</returns>
        public static string GetActionName(string action)
        {
            return InputActionMap.GetValueOrDefault(action, "");
        }

        /// <summary>
        /// Returns the name of a JoyButton
        /// </summary>
        /// <param name="button">The controller button</param>
        /// <returns>The name of the button</returns>
        public static string GetJoyButtonName(JoyButton button)
        {
            string buttonName = button.ToString();
            buttonName = JoyButtonMap.GetValueOrDefault(button, "CON-" + buttonName);
            return buttonName;
        }

        /// <summary>
        /// Returns the name of a JoyAxis
        /// </summary>
        /// <param name="axis">The controller axis</param>
        /// <param name="value">The value of the axis</param>
        /// <returns></returns>
        public static string GetJoyAxisName(JoyAxis axis, float value)
        {
            value = value > 0 ? 1.0f : -1.0f;
            return JoyAxisMap.GetValueOrDefault((axis, value));
        }
    }
}