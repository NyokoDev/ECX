using UnityEngine;
using System.Collections.Generic;

namespace EyeCandyX
{
    class InputUtils
    {
        public static bool HotkeyPressed()
        {
            bool validInput = false;
            //  Preferred hotkey: [Shift] + [U]:
            if (((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyUp(KeyCode.U)) && EyeCandyXTool.config.keyboardShortcut == 0)
            {
                validInput = true;
            }
            //  Preferred hotkey: [Ctrl] + [U]:
            if (((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyUp(KeyCode.U)) && EyeCandyXTool.config.keyboardShortcut == 1)
            {
                validInput = true;
            }
            //  Preferred hotkey: [Alt] + [U]:
            if (((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyUp(KeyCode.U)) && EyeCandyXTool.config.keyboardShortcut == 2)
            {
                validInput = true;
            }
            return validInput;
        }

        private static readonly List<KeyCode> hotkeyList = new List<KeyCode>()
        {
            KeyCode.LeftShift,
            KeyCode.RightShift,
            KeyCode.LeftControl,
            KeyCode.RightControl,
            KeyCode.LeftAlt,
            KeyCode.RightAlt
        };
    }
}
