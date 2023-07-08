using ColossalFramework;
using ICities;
using System;
using EyeCandyX.GUI;

namespace EyeCandyX
{
    public class ThreadingExtension : ThreadingExtensionBase
    {
        public override void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            base.OnUpdate(realTimeDelta, simulationTimeDelta);

            // Execute code only when in-game:
            if (EyeCandyXTool.isGameLoaded)
            {
                // Register Hotkey:
                bool flag = InputUtils.HotkeyPressed();
                if (flag)
                {
                    if (EyeCandyXTool.config.outputDebug)
                    {
                        DebugUtils.Log($"Hotkey pressed.");
                    }
                    UIMainPanel.instance.Toggle();
                }
            }
        }
    }
}
