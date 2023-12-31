﻿using System;
using System.Collections.Generic;
using System.Linq;
using ColossalFramework.UI;
using UnityEngine;
using System.Text.RegularExpressions;
using EyeCandyX.TranslationFramework;

namespace EyeCandyX.GUI
{
    public class ColorManagementPanel : UIPanel
    {
        private UILabel _lutLabel;
        private UIFastList _lutFastlist;
        private UIButton _loadLutButton;
        private UIButton _resetColorManagementButton;

        public UIFastList lutFastlist
        {
            get { return _lutFastlist; }
        }

        public UIButton loadLutButton
        {
            get { return _loadLutButton; }
        }

        public LutList.Lut _selectedLut;

        private UICheckBox _enableLutCheckbox;
        public UICheckBox enableLutCheckbox
        {
            get { return _enableLutCheckbox; }
        }

        private UICheckBox _enableTonemappingCheckbox;
        public UICheckBox enableTonemappingCheckbox
        {
            get { return _enableTonemappingCheckbox; }
        }

        private UICheckBox _enableBloomCheckbox;
        public UICheckBox enableBloomCheckbox
        {
            get { return _enableBloomCheckbox; }
        }

        private MonoBehaviour[] cameraBehaviours;
        public CameraController cameraController;

        public bool tonemappingApplied = true;

        private static ColorManagementPanel _instance;
        public static ColorManagementPanel instance
        {
            get { return _instance; }
        }

        public override void Start()
        {
            base.Start();
            _instance = this;
            canFocus = true;
            isInteractive = true;
            //  
            cameraBehaviours = Camera.main.GetComponents<MonoBehaviour>() as MonoBehaviour[];
            cameraController = FindObjectsOfType<CameraController>().First();
            SetupControls();
            PopulateLutFastList();
        }

        private void SetupControls()
        {
            //  LUT Selection:
            var topContainer = UIUtils.CreateFormElement(this, "top");

            _lutLabel = topContainer.AddUIComponent<UILabel>();
            _lutLabel.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.SELECTLUT_TEXT);
            _lutLabel.textScale = 0.9f;
            _lutLabel.padding = new RectOffset(0, 0, 0, 5);

            // FastList
            _lutFastlist = UIFastList.Create<UILutItem>(topContainer);
            _lutFastlist.backgroundSprite = "UnlockingPanel2";
            _lutFastlist.relativePosition = new Vector3(0, 15);
            _lutFastlist.width = UIUtils.c_fastListWidth;
            _lutFastlist.height = UIUtils.c_fastListHeight - 20;
            _lutFastlist.canSelect = true;
            _lutFastlist.eventSelectedIndexChanged += OnSelectedItemChanged;

            //  Load lut:
            var loadContainer = UIUtils.CreateFormElement(this, "center");
            loadContainer.name = "loadDeleteContainer";
            loadContainer.relativePosition = new Vector3(0, 260);
            loadContainer.autoLayout = false;
            loadContainer.isVisible = true;

            _loadLutButton = UIUtils.CreateButton(loadContainer);
            _loadLutButton.width = 100f;
            _loadLutButton.isEnabled = false;
            _loadLutButton.opacity = 0.5f;
            _loadLutButton.relativePosition = new Vector3(5, 10);
            _loadLutButton.name = "loadLutButton";
            _loadLutButton.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.LOADLUT_TEXT);
            _loadLutButton.tooltip = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.ALREADYACTLUT_TEXT);
            _loadLutButton.eventClicked += (c, e) =>
            {
                try
                {
                    DebugUtils.Log($"ColorManagementPanel: 'Load lut' clicked: {_selectedLut.name} ({_selectedLut.internal_name} / {_selectedLut.index}).");
                    EyeCandyXTool.currentSettings.color_selectedlut = _selectedLut.internal_name;
                    ColorCorrectionManager.instance.currentSelection = _selectedLut.index;
                }
                catch (Exception ex)
                {
                    if (EyeCandyXTool.config.outputDebug)
                    {
                        DebugUtils.Log($"ColorManagementPanel: 'Load lut' clicked: lut {_selectedLut.name} not found, applying default Lut for current biome ({LoadingManager.instance.m_loadedEnvironment}).");
                    }
                    DebugUtils.LogException(ex);
                    _lutFastlist.DisplayAt(0);
                    EyeCandyXTool.currentSettings.color_selectedlut = LutList.GetLutNameByIndex(ColorCorrectionManager.instance.lastSelection); // "None";
                    ColorCorrectionManager.instance.currentSelection = 0;
                }
            };

            //  LUT:
            var lutContainer = UIUtils.CreateFormElement(this, "center");
            lutContainer.name = "lutContainer";
            lutContainer.relativePosition = new Vector3(0, 325);

            _enableLutCheckbox = UIUtils.CreateCheckBox(lutContainer);
            _enableLutCheckbox.relativePosition = new Vector3(5, 17);
            _enableLutCheckbox.name = "_enableLutCheckbox";
            //_enableLutCheckbox.tooltip = "Check this box to toggle LUT color correction.";
            //_enableLutCheckbox.isChecked = GetCameraBehaviour("ColorCorrectionLut");
            _enableLutCheckbox.isChecked = true;
            _enableLutCheckbox.eventCheckChanged += CheckboxChanged;
            _enableLutCheckbox.label.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.ENABLELUT_TEXT); 

            //  Tonemapping:
            var tonemappingContainer = UIUtils.CreateFormElement(this, "center");
            tonemappingContainer.name = "tonemappingContainer";
            tonemappingContainer.relativePosition = new Vector3(0, 370);

            _enableTonemappingCheckbox = UIUtils.CreateCheckBox(tonemappingContainer);
            _enableTonemappingCheckbox.relativePosition = new Vector3(5, 17);
            _enableTonemappingCheckbox.name = "_enableTonemappingCheckbox";
            _enableTonemappingCheckbox.tooltip = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.TM_TOOLTIP_TEXT);
            _enableTonemappingCheckbox.isChecked = GetCameraBehaviour("ToneMapping");
            _enableTonemappingCheckbox.isChecked = true;
            _enableTonemappingCheckbox.eventCheckChanged += CheckboxChanged;
            _enableTonemappingCheckbox.label.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.ENABLE_TM_TEXT);

            //  Bloom:
            var bloomContainer = UIUtils.CreateFormElement(this, "center");
            bloomContainer.name = "bloomContainer";
            bloomContainer.relativePosition = new Vector3(0, 415);
            bloomContainer.height = 20;

            _enableBloomCheckbox = UIUtils.CreateCheckBox(bloomContainer);
            _enableBloomCheckbox.relativePosition = new Vector3(5, 17);
            _enableBloomCheckbox.name = "_enableBloomCheckbox";
            //_enableBloomCheckbox.tooltip = "Check this box to toggle bloom effect.";
            //_enableBloomCheckbox.isChecked = GetCameraBehaviour("Bloom");
            _enableBloomCheckbox.isChecked = true;
            _enableBloomCheckbox.eventCheckChanged += CheckboxChanged;
            _enableBloomCheckbox.label.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.ENABLE_BLOOM_TEXT);

            //  Reset button:
            var bottomContainer = UIUtils.CreateFormElement(this, "bottom");

            _resetColorManagementButton = UIUtils.CreateButton(bottomContainer);
            _resetColorManagementButton.name = "resetButton";
            _resetColorManagementButton.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.RESET_BUTTON_TEXT);
            _resetColorManagementButton.tooltip = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.RESET_VALUES_TOOLTIP);
            _resetColorManagementButton.eventClicked += (c, e) =>
            {
                if (EyeCandyXTool.config.outputDebug)
                {
                    DebugUtils.Log($"ColorPanel: 'Reset' clicked.");
                }
                //  
                _lutFastlist.DisplayAt(0);
                _lutFastlist.selectedIndex = 0;
                EyeCandyXTool.currentSettings.color_selectedlut = LutList.GetLutNameByIndex(ColorCorrectionManager.instance.lastSelection);
                ColorCorrectionManager.instance.currentSelection = 0;
                _enableLutCheckbox.isChecked = true;
                _enableTonemappingCheckbox.isChecked = true;
                _enableBloomCheckbox.isChecked = true;
            };
        }

        public void PopulateLutFastList()
        {
            _lutFastlist.rowsData.Clear();
            //  
            var allLuts = LutList.GetLutList();
            for (int i = 0; i < allLuts.Count; i++)
            {
                if (allLuts[i] != null)
                {
                    _lutFastlist.rowsData.Add(allLuts[i]);
                }
            }
            //  
            _lutFastlist.rowHeight = UIUtils.c_fastListRowHeight;
            _lutFastlist.DisplayAt(ColorCorrectionManager.instance.lastSelection);
            _lutFastlist.selectedIndex = ColorCorrectionManager.instance.lastSelection;
        }

        protected void OnSelectedItemChanged(UIComponent component, int i)
        {
            _selectedLut = _lutFastlist.rowsData[i] as LutList.Lut;
            //  
            if (EyeCandyXTool.config.outputDebug)
            {
                DebugUtils.Log($"ColorManagementPanel: LutFastList SelectedItemChanged: {_selectedLut.name} selected.");
            }
            //  Button appearance:
            var isActive = (_selectedLut.internal_name == EyeCandyXTool.currentSettings.color_selectedlut);
            _loadLutButton.isEnabled = (isActive) ? false : true;
            _loadLutButton.opacity = (isActive) ? 0.5f : 1.0f;
            
            _loadLutButton.tooltip = (isActive) ? Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.LUT_ALREADY_ACTIVE_NOTICE) : Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.LOAD_LUT_SELECTED);
        }

        protected void OnEnableStateChanged(UIComponent component, bool state)
        {
            _lutFastlist.DisplayAt(_lutFastlist.listPosition);
        }
        
        private void CheckboxChanged(UIComponent trigger, bool isChecked)
        {
            if (EyeCandyXTool.config.outputDebug)
            {
                DebugUtils.Log($"AmbientPanel: Checkbox {trigger.name} = {isChecked}");
            }
            //  
            if (trigger == _enableLutCheckbox)
            {
                GetCameraBehaviour("ColorCorrectionLut").enabled = isChecked;
                EyeCandyXTool.currentSettings.color_lut = isChecked;
            }
            else if (trigger == _enableTonemappingCheckbox)
            {
                GetCameraBehaviour("ToneMapping").enabled = isChecked;
                EyeCandyXTool.currentSettings.color_tonemapping = isChecked;
                tonemappingApplied = isChecked;
            }
            if (trigger == _enableBloomCheckbox)
            {
                GetCameraBehaviour("Bloom").enabled = isChecked;
                EyeCandyXTool.currentSettings.color_bloom = isChecked;
            }
        }

        public MonoBehaviour GetCameraBehaviour(string name)
        {
            for (int i = 0; i < cameraBehaviours.Length; i++)
            {
                if (cameraBehaviours[i].GetType().Name == name)
                {
                    return cameraBehaviours[i];
                }

            }
            return null;
        }
    }

    public class LutList
    {
        private static readonly string[] vanillaLuts = new string[] {
            "None",
            "LUTSunny",
            "LUTNorth",
            "LUTTropical",
            "LUTeurope"
        };

        public static Lut GetLut(string name)
        {
            foreach (var lut in GetLutList())
            {
                if (lut.internal_name == name) return lut;
            }
            return null;
        }

        public static string GetLutNameByIndex(int index)
        {
            foreach (var lut in GetLutList())
            {
                if (lut.index == index) return lut.name;
            }
            return "None";
        }

        public static List<Lut> GetLutList()
        {
            var list = new List<Lut>();
            var i = 0;
            //  
            foreach (var lut in ColorCorrectionManager.instance.items)
            {
                Lut l = new Lut()
                {
                    index = i,
                    name = GetLutDisplayName(lut),
                    internal_name = lut
                };
                list.Add(l);
                i++;
            }
            return list;
        }

        public static string GetLutDisplayName(string name)
        {
            //  Get friendly Workshop Lut name:
            if (name.Contains('.'))
            {
                name = name.Remove(0, 10);
            }
            //  Get friendly Built-In Lut name:
            else {
                if (name.ToLower() == "lutsunny")
                {
                    name = "Temperate";
                }
                if (name.ToLower() == "lutnorth")
                {
                    name = "Boreal";
                }
                if (name.ToLower() == "luttropical")
                {
                    name = "Tropical";
                }
                if (name.ToLower() == "luteurope")
                {
                    name = "European";
                }
            }
            return name;
        }

        public class Lut
        {
            public int index;
            public string name;
            public string internal_name;
        }
    }
}
