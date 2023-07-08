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
using UnifiedUI.Helpers;
using ColossalFramework;
using System.Collections.Generic;

namespace EyeCandyX
{
    public class Mod : IUserMod
    {
        public string Name => Translation.Instance.GetTranslation(TranslationID.MOD_NAME);

        public string Description => Translation.Instance.GetTranslation(TranslationID.MOD_DESCRIPTION);

        public const string version = "1.2";




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

                // Open File Location Button:
                UIButton openLocationButton = (UIButton)group.AddButton("Open File Location", OnOpenLocationButtonClick);
                openLocationButton.tooltip = "Open the file location of ECX.ecx";
            }
            catch (Exception e)
            {
                DebugUtils.LogException(e);
            }
        }

        private void OnOpenLocationButtonClick()
        {
            string logFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ECX.ecx");
            if (File.Exists(logFilePath))
            {
                string directoryPath = Path.GetDirectoryName(logFilePath);
                Process.Start("explorer.exe", directoryPath);
            }
            else
            {
                Debug.Log("The file does not exist.");
            }
        }
    }
    public class ECXTool : ToolBase
    {
        UILabel label_;
        UIComponent button_;

        
        protected override void Awake()
        {
            try
            {
                base.Awake();
                string spritePath = UUIHelpers.GetFullPath<Mod>("Assets", "Icon2.png");
                Debug.Log("EYECANDYX - UUI -Sprite path: " + spritePath);
                Texture2D icon = UUIHelpers.LoadTexture(spritePath);
                var hotkeys = new UUIHotKeys();
                button_ = UUIHelpers.RegisterToolButton(
                    name: "EyecandyX",
                    groupName: null, // default group
                    tooltip: "Eyecandy X",
                    spritefile: spritePath,
                    tool: this,
                    activationKey: hotkeys.ActivationKey,
                    activeKeys: hotkeys.InToolKeys);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                UIView.ForwardException(ex);
            }
        }
    }
}


