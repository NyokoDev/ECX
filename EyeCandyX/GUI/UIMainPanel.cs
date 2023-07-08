using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;
using EyecandyX.TranslationFramework;
using EyeCandyX.TranslationFramework;
using EyecandyX;

namespace EyeCandyX.GUI
{
    public class UIMainPanel : UIPanel
    {
        public UIMainTitleBar m_title;

        public UITabstrip panelTabs;
        public UIButton ambientButton;
        public UIButton weatherButton;
        public UIButton colorManagementButton;
        public UIButton presetsButton;

        public AmbientPanel ambientPanel;
        public WeatherPanel weatherPanel;
        public ColorManagementPanel colorManagementPanel;
        public PresetsPanel presetsPanel;

        public static UIMainPanel instance;

        public static void Initialize()
        {
        }

        public override void Start()
        {
            base.Start();
            instance = this;
            backgroundSprite = (EyeCandyXTool.isEditor) ? "MenuPanel2" : "UnlockingPanel2";
            isVisible = false;
            canFocus = true;
            isInteractive = true;
            name = "modMainPanel";
            padding = new RectOffset(10, 10, 4, 4);
            width = UIUtils.c_modPanelWidth;
            height = UIUtils.c_modPanelHeight;
            relativePosition = new Vector3(10, 60);

            SetupControls();
        }

        public void SetupControls()
        {
            m_title = AddUIComponent<UIMainTitleBar>();
            m_title.title = "Eyecandy X";

            panelTabs = AddUIComponent<UITabstrip>();
            panelTabs.size = new Vector2(UIUtils.c_modPanelInnerWidth, UIUtils.c_tabButtonHeight);
            panelTabs.relativePosition = new Vector2(UIUtils.c_spacing, UIUtils.c_titleBarHeight + UIUtils.c_spacing);

            ambientButton = UIUtils.CreateTab(panelTabs, Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.AMBIENT_TAB), true);
            ambientButton.width = UIUtils.c_tabButtonWidth;
            ambientButton.height = UIUtils.c_tabButtonHeight;
            ambientButton.tooltip = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.AMBIENT_TAB_TOOLTIP);
            ambientButton.textScale = 0.9f;

            weatherButton = UIUtils.CreateTab(panelTabs, Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.WEATHER_TAB));
            weatherButton.width = UIUtils.c_tabButtonWidth;
            weatherButton.height = UIUtils.c_tabButtonHeight;
            weatherButton.tooltip = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.WEATHER_TAB_TOOLTIP);
            weatherButton.textScale = 0.9f;

            colorManagementButton = UIUtils.CreateTab(panelTabs, Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.LUT_TAB));
            colorManagementButton.width = UIUtils.c_tabButtonWidth;
            colorManagementButton.height = UIUtils.c_tabButtonHeight;
            colorManagementButton.tooltip = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.LUT_TAB_TOOLTIP);
            colorManagementButton.textScale = 0.9f;

            presetsButton = UIUtils.CreateTab(panelTabs, Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.PRESETS_TAB));
            presetsButton.width = UIUtils.c_tabButtonWidth;
            presetsButton.height = UIUtils.c_tabButtonHeight;
            presetsButton.tooltip = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.PRESETS_TAB_TOOLTIP);
            presetsButton.textScale = 0.9f;

            UIPanel body = AddUIComponent<UIPanel>();
            body.width = UIUtils.c_modPanelInnerWidth;
            body.height = UIUtils.c_modPanelInnerHeight;
            body.relativePosition = new Vector3(5, 36 + 28 + 5);

            ambientPanel = body.AddUIComponent<AmbientPanel>();
            ambientPanel.name = "ambientPanel";
            ambientPanel.width = UIUtils.c_modPanelInnerWidth;
            ambientPanel.height = UIUtils.c_modPanelInnerHeight;
            ambientPanel.relativePosition = Vector3.zero;
            ambientPanel.isVisible = true;
            ambientPanel.todManager = Singleton<DayNightCycleManager>.instance;

            weatherPanel = body.AddUIComponent<WeatherPanel>();
            weatherPanel.name = "weatherPanel";
            weatherPanel.width = UIUtils.c_modPanelInnerWidth;
            weatherPanel.height = UIUtils.c_modPanelInnerHeight;
            weatherPanel.relativePosition = Vector3.zero;
            weatherPanel.isVisible = false;

            colorManagementPanel = body.AddUIComponent<ColorManagementPanel>();
            colorManagementPanel.name = "colormanagementPanel";
            colorManagementPanel.width = UIUtils.c_modPanelInnerWidth;
            colorManagementPanel.height = UIUtils.c_modPanelInnerHeight;
            colorManagementPanel.relativePosition = Vector3.zero;
            colorManagementPanel.isVisible = false;

            presetsPanel = body.AddUIComponent<PresetsPanel>();
            presetsPanel.name = "presetsPanel";
            presetsPanel.width = UIUtils.c_modPanelInnerWidth;
            presetsPanel.height = UIUtils.c_modPanelInnerHeight;
            presetsPanel.relativePosition = Vector3.zero;
            presetsPanel.isVisible = false;

            ambientButton.eventClick += (c, e) => TabClicked(c, e);
            weatherButton.eventClick += (c, e) => TabClicked(c, e);
            colorManagementButton.eventClick += (c, e) => TabClicked(c, e);
            presetsButton.eventClick += (c, e) => TabClicked(c, e);
        
    
}


private void TabClicked(UIComponent trigger, UIMouseEventParameter e)
        {
            if (EyeCandyXTool.config.outputDebug)
            {
                DebugUtils.Log($"MainPanel: Tab '{trigger.name}' clicked");
            }
            //  
            weatherPanel.isVisible = false;
            ambientPanel.isVisible = false;
            colorManagementPanel.isVisible = false;
            presetsPanel.isVisible = false;

            if (trigger == ambientButton)
            {
                ambientPanel.isVisible = true;
            }
            if (trigger == weatherButton)
            {
                weatherPanel.isVisible = true;
            }
            if (trigger == colorManagementButton)
            {
                colorManagementPanel.isVisible = true;
                colorManagementPanel.lutFastlist.selectedIndex = ColorCorrectionManager.instance.lastSelection;
                var isActive = (colorManagementPanel._selectedLut.internal_name == EyeCandyXTool.currentSettings.color_selectedlut);
                colorManagementPanel.loadLutButton.isEnabled = (isActive) ? false : true;
                colorManagementPanel.loadLutButton.opacity = (isActive) ? 0.5f : 1.0f;
                colorManagementPanel.loadLutButton.tooltip = (isActive) ? "LUT selected in list is already active." : "Load LUT selected in list.";
            }
            if (trigger == presetsButton)
            {
                if (presetsPanel.presetFastlist.selectedIndex < 0)
                {
                    presetsPanel.updateButtons(true);
                }
                else
                {
                    presetsPanel.updateButtons(false);
                }
                presetsPanel.isVisible = true;
            }
        }

        //  Toggle main panel and update button state:
        public void Toggle()
        {
            if (instance.isVisible)
            {
                instance.isVisible = false;
                UIMainButton.instance.state = UIButton.ButtonState.Normal;
            }
            else
            {
                instance.isVisible = true;
                UIMainButton.instance.state = UIButton.ButtonState.Focused;
            }
        }
    }
}