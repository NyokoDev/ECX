using System.Collections.Generic;
using ColossalFramework.UI;
using EyeCandyX.TranslationFramework;
using UnityEngine;

namespace EyeCandyX.GUI
{
    public class PresetsPanel : UIPanel
    {
        private UILabel _presetLabel;
        private UIFastList _presetFastlist;

        private UIButton _loadPresetButton;
        private UIButton _deletePresetButton;
        private UIButton _savePresetButton;
        private UIButton _overwritePresetButton;

        private UIButton _resetAllButton;

        public UIFastList presetFastlist
        {
            get { return _presetFastlist; }
        }
        public UIButton loadPresetButton
        {
            get { return _loadPresetButton; }
        }
        public UIButton deletePresetButton
        {
            get { return _deletePresetButton; }
        }
        public UIButton overwritePresetButton
        {
            get { return _overwritePresetButton; }
        }

        public Configuration.Preset _selectedPreset;
        public string[] _presets;

        private static PresetsPanel _instance;
        public static PresetsPanel instance => _instance;

        public override void Start()
        {
            base.Start();
            _instance = this;
            canFocus = true;
            isInteractive = true;
            //  
            SetupControls();
            PopulatePresetsFastList();
            //  Create temporary preset for current settings:
            if (EyeCandyXTool.config.loadLastPresetOnStart && !string.IsNullOrEmpty(EyeCandyXTool.config.lastPreset))
            {
                //  Create temporary preset based on last active preset:
                try
                {
                    EyeCandyXTool.LoadPreset(EyeCandyXTool.config.lastPreset, false);
                    if (EyeCandyXTool.config.outputDebug)
                    {
                        DebugUtils.Log($"Temporary preset created based on last active preset '{EyeCandyXTool.config.lastPreset}'.");
                    }
                }
                catch
                {
                    //  Latest preset not found: create temporary preset from scratch:
                    EyeCandyXTool.ResetAll();
                }
            }
            else
            {
                //  Fallback: create temporary preset from scratch:
                EyeCandyXTool.ResetAll();
            }
        }

        private void SetupControls()
        {
            //  Presets:
            var topContainer = UIUtils.CreateFormElement(this, "top");

            _presetLabel = topContainer.AddUIComponent<UILabel>();
            _presetLabel.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.LOADPRESET_TEXT);
            _presetLabel.textScale = 0.9f;
            _presetLabel.padding = new RectOffset(0, 0, 0, 5);

            // FastList
            _presetFastlist = UIFastList.Create<UIPresetItem>(topContainer);
            _presetFastlist.backgroundSprite = "UnlockingPanel";
            _presetFastlist.relativePosition = new Vector3(0, 15);
            _presetFastlist.width = UIUtils.c_fastListWidth;
            _presetFastlist.height = UIUtils.c_fastListHeight - 20;
            _presetFastlist.canSelect = true;
            _presetFastlist.eventSelectedIndexChanged += OnSelectedItemChanged;

            //  Load/delete preset:
            var loadDeleteContainer = UIUtils.CreateFormElement(this, "center");
            loadDeleteContainer.height = 40;
            loadDeleteContainer.name = "loadDeleteContainer";
            loadDeleteContainer.relativePosition = new Vector3(0, 260);
            loadDeleteContainer.autoLayout = false;
            loadDeleteContainer.isVisible = true;

            _loadPresetButton = UIUtils.CreateButton(loadDeleteContainer);
            _loadPresetButton.opacity = 0.25f;
            _loadPresetButton.isEnabled = false;
            _loadPresetButton.relativePosition = new Vector3(5, 10);
            _loadPresetButton.name = "loadPresetButton";
            _loadPresetButton.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.LOADPRESET_TEXT);
            _loadPresetButton.tooltip = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.LOADPRESET_TOOLTIP);
            _loadPresetButton.eventClicked += (c, e) =>
            {
                //  
                if (EyeCandyXTool.config.outputDebug)
                {
                    DebugUtils.Log($"PresetsPanel: 'Load preset' clicked: preset '{_selectedPreset.name}'.");
                }
                EyeCandyXTool.LoadPreset(_selectedPreset.name, true);
                //  Button appearance:
                updateButtons(true);
            };

            _deletePresetButton = UIUtils.CreateButton(loadDeleteContainer);
            _deletePresetButton.opacity = 0.25f;
            _deletePresetButton.isEnabled = false;
            _deletePresetButton.relativePosition = new Vector3(272, 10);
            _deletePresetButton.name = "deletePresetButton";
            _deletePresetButton.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.DELETEPRESET_TEXT);
            _deletePresetButton.tooltip = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.DELETEPRESET_TOOLTIP);
            _deletePresetButton.eventClicked += (c, e) =>
            {
                //  
                if (EyeCandyXTool.config.outputDebug)
                {
                    DebugUtils.Log($"PresetsPanel: 'Delete preset' clicked: preset '{_selectedPreset.name}'.");
                }
                ConfirmPanel.ShowModal(Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.EXCEPTIONDELETEPRESET), Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.EXCEPTIONDELETEPRESET2) + _selectedPreset.name + "'?", (d, i) => {
                    if (i == 1)
                    {
                        EyeCandyXTool.DeletePreset(_selectedPreset);
                        //  Update FastList:
                        PopulatePresetsFastList();
                        //  Button appearance:
                        updateButtons(true);
                    }
                });
            };

            //  Save/overwrite preset:
            var saveOverwriteContainer = UIUtils.CreateFormElement(this, "center");
            saveOverwriteContainer.height = 40;
            saveOverwriteContainer.relativePosition = new Vector3(0, 300);
            saveOverwriteContainer.autoLayout = false;
            saveOverwriteContainer.isVisible = true;

            _savePresetButton = UIUtils.CreateButton(saveOverwriteContainer);
            _savePresetButton.relativePosition = new Vector3(5, 10);
            _savePresetButton.name = "savePresetButton";
            _savePresetButton.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.SAVEPRESET_TEXT);
            //  Todo: add all settings to tooltip(?)
            _savePresetButton.tooltip = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.SAVESETTINGS_TOOLTIP);
            _savePresetButton.eventClicked += (c, e) =>
            {
                //  
                if (EyeCandyXTool.config.outputDebug)
                {
                    DebugUtils.Log($"PresetsPanel: 'Save preset' clicked.");
                }
                //  Open 'Preset name' modal:
                UIView.PushModal(UINewPresetModal.instance);
                UINewPresetModal.instance.Show(true);
                //  Button appearance:
                updateButtons(true);
            };

            _overwritePresetButton = UIUtils.CreateButton(saveOverwriteContainer);
            _overwritePresetButton.opacity = 0.25f;
            _overwritePresetButton.isEnabled = false;
            _overwritePresetButton.relativePosition = new Vector3(272, 10);
            _overwritePresetButton.name = "overwritePresetButton";
            _overwritePresetButton.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.OVERWRITE);
            _overwritePresetButton.tooltip = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.OVERWRITE_TOOLTIP);
            _overwritePresetButton.eventClicked += (c, e) =>
            {
                //  
                if (EyeCandyXTool.config.outputDebug)
                {
                    DebugUtils.Log($"PresetsPanel: 'Overwrite preset' clicked: preset '{_selectedPreset.name}'.");
                }
                ConfirmPanel.ShowModal(Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.EXCEPTIONOVERWRITE_TEXT), Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.EXCEPTIONOVERWRITE_TEXT2) + _selectedPreset.name + "'?", (d, i) => {
                    if (i == 1)
                    {
                        EyeCandyXTool.CreatePreset(_selectedPreset.name, true);
                        //  Button appearance:
                        updateButtons(true);
                    }
                });
            };

            //  Reset all:zz
            var resetContainer = UIUtils.CreateFormElement(this, "bottom");

            _resetAllButton = UIUtils.CreateButton(resetContainer);
            _resetAllButton.name = "resetAllButton";
            _resetAllButton.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.RESET_BUTTON_TEXT);
            _resetAllButton.tooltip = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.RESET_VALUES_TOOLTIP);
            _resetAllButton.eventClicked += (c, e) =>
            {
                //  
                if (EyeCandyXTool.config.outputDebug)
                {
                    DebugUtils.Log($"PresetsPanel: 'Reset all' clicked.");
                }
                EyeCandyXTool.ResetAll(true);
                //  Button appearance:
                updateButtons(true);
            };
        }

        public void PopulatePresetsFastList()
        {
            _presetFastlist.rowsData.Clear();
            _presetFastlist.selectedIndex = -1;
            //  
            List<Configuration.Preset> allPresets = EyeCandyXTool.config.presets;
            if (allPresets.Count > 0)
            {
                for (int i = 0; i < allPresets.Count; i++)
                {
                    if (allPresets[i] != null)
                    {
                        _presetFastlist.rowsData.Add(allPresets[i]);
                    }
                }
                //  
                _presetFastlist.rowHeight = UIUtils.c_fastListRowHeight;
                _presetFastlist.DisplayAt(0);
            }
        }

        protected void OnSelectedItemChanged(UIComponent component, int i)
        {
            _selectedPreset = _presetFastlist.rowsData[i] as Configuration.Preset;
            if (EyeCandyXTool.config.outputDebug)
            {
                DebugUtils.Log($"PresetsPanel: FastListItem selected: preset '{_selectedPreset.name}'.");
            }
            //  Button appearance:
            updateButtons(false);
        }

        protected void OnEnableStateChanged(UIComponent component, bool state)
        {
            _presetFastlist.DisplayAt(_presetFastlist.listPosition);
        }

        public void updateButtons(bool disableAll)
        {
            _loadPresetButton.opacity = (disableAll) ? 0.25f : 1f;
            _loadPresetButton.isEnabled = (disableAll) ? false : true;
            _deletePresetButton.opacity = (disableAll) ? 0.25f : 1f;
            _deletePresetButton.isEnabled = (disableAll) ? false : true;
            _overwritePresetButton.opacity = (disableAll) ? 0.25f : 1f;
            _overwritePresetButton.isEnabled = (disableAll) ? false : true;
        }
    }
}