using CityColors;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using EyecandyX.Locale;
using EyeCandyX.TranslationFramework;
using ICities;
using System;
using System.Management;
using System.Diagnostics;
using UnityEngine;
using static ColossalFramework.Plugins.PluginManager;
using System.IO;
using Debug = UnityEngine.Debug;
using ColossalFramework.PlatformServices;
using UnifiedUI;
using UnifiedUI.Helpers;
using ColossalFramework;
using System.Collections.Generic;
using EyeCandyX.GUI;
using ToggleKey;
using System.Reflection;

namespace EyeCandyX
{
    public class EyecandyXMod : IUserMod
    {
        public string Name => Translation.Instance.GetTranslation(TranslationID.MOD_NAME);
        public string Description => Translation.Instance.GetTranslation(TranslationID.MOD_DESCRIPTION);
        public const string version = "1.2";
        internal UUICustomButton _uuiButton;


        // Mod options:
        public void OnSettingsUI(UIHelperBase helper)
        {
            try
            {
                EyeCandyXTool.LoadConfig();

                UIHelperBase group = helper.AddGroup(Name);
                group.AddSpace(10);

                // Auto-Load Last Preset:
                UICheckBox loadLastPresetOnStartCheckBox = (UICheckBox)group.AddCheckbox(Translation.Instance.GetTranslation(TranslationID.SETTINGS_LOADMOSTRECENT), EyeCandyXTool.config.loadLastPresetOnStart,
                    sel =>
                    {
                        if (EyeCandyXTool.config.loadLastPresetOnStart != sel)
                        {
                            EyeCandyXTool.config.loadLastPresetOnStart = sel;
                            EyeCandyXTool.SaveConfig(false);
                        }
                    });
                loadLastPresetOnStartCheckBox.tooltip = Translation.Instance.GetTranslation(TranslationID.SETTINGS_LOADMOSTRECENT); // Load most recent preset on start. 

                group.AddSpace(15);

                // Enable Time of Day:
                UICheckBox enableSimulationControlCheckBox = (UICheckBox)group.AddCheckbox(Translation.Instance.GetTranslation(TranslationID.SETTINGS_ENABLESIMCONTROL), EyeCandyXTool.config.enableSimulationControl, //enable time control
                    sel =>
                    {
                        if (EyeCandyXTool.config.enableSimulationControl != sel)
                        {
                            EyeCandyXTool.config.enableSimulationControl = sel;
                            EyeCandyXTool.SaveConfig(false);
                        }
                    });
                enableSimulationControlCheckBox.tooltip = (Translation.Instance.GetTranslation(TranslationID.SETTINGS_ENABLESIMCONTROL_TOOLTIP));// Enable time of day slider 

                group.AddSpace(15);

                // Keyboard Shortcut:
                UIDropDown keyboardShortcutDropdown = (UIDropDown)group.AddDropdown(Translation.Instance.GetTranslation(TranslationID.SETTINGS_DESIREDKEY), new[] { "Shift + U", "Ctrl + U", "Alt + U" }, EyeCandyXTool.config.keyboardShortcut,
                    sel =>
                    {
                        EyeCandyXTool.config.keyboardShortcut = sel;
                        EyeCandyXTool.SaveConfig(false);
                    });
                keyboardShortcutDropdown.tooltip = (Translation.Instance.GetTranslation(TranslationID.SETTINGS_DESIREDKEY)); //Choose your desired key combination for activating the mod interface.

                group.AddSpace(15);

                // Output Debug Data:
                UICheckBox debugCheckBox = (UICheckBox)group.AddCheckbox(Translation.Instance.GetTranslation(TranslationID.SETTINGS_DEBUGDATA), EyeCandyXTool.config.outputDebug,
                    b =>
                    {
                        if (EyeCandyXTool.config.outputDebug != b)
                        {
                            EyeCandyXTool.config.outputDebug = b;
                            EyeCandyXTool.SaveConfig(false);
                        }
                    });

                // Set the tooltip for the debugCheckBox
                debugCheckBox.tooltip = Translation.Instance.GetTranslation(TranslationID.SETTINGS_DEBUGDATA_TOOLTIP);

                group.AddSpace(15);

               
              
            }
            catch (Exception e)
            {
                DebugUtils.LogException(e);
            }
        }
    }
}


    
        
    



    