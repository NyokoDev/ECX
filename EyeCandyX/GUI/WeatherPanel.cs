﻿using ColossalFramework.UI;
using UnityEngine;

namespace EyeCandyX.GUI
{
    public class WeatherPanel : UIPanel
    {
        private UICheckBox _enableWeatherCheckbox;

        private UICheckBox _rainMotionblurCheckbox;

        private UILabel _fogIntensityLabel;
        private UISlider _fogIntensitySlider;

        private UILabel _precipitationLabel;
        private UISlider _precipitationSlider;
        
        //private UILabel _wetnessLabel;
        //private UISlider _wetnessSlider;

        private UIButton _resetWeatherButton;

        public UICheckBox enableWeatherCheckbox
        {
            get { return _enableWeatherCheckbox; }
        }
        public UICheckBox rainMotionblurCheckbox
        {
            get { return _rainMotionblurCheckbox; }
        }
        public UISlider fogIntensitySlider
        {
            get { return _fogIntensitySlider; }
        }
        public UISlider precipitationSlider
        {
            get { return _precipitationSlider; }
        }
        //public UISlider wetnessSlider
        //{
        //    get { return _wetnessSlider; }
        //}

        private static WeatherPanel _instance;
        public static WeatherPanel instance => _instance;

        public override void Start()
        {
            base.Start();
            _instance = this;
            canFocus = true;
            isInteractive = true;
            //  
            SetupControls();
        }

        private void SetupControls()
        {
            //  Enable weather:
            var topContainer = UIUtils.CreateFormElement(this, "top");
            topContainer.name = "enableWeatherCheckboxContainer";
            //topContainer.relativePosition = new Vector3(0, -3);
            //topContainer.autoLayout = false;

            _enableWeatherCheckbox = UIUtils.CreateCheckBox(topContainer);
            _enableWeatherCheckbox.relativePosition = new Vector3(5, 17);
            _enableWeatherCheckbox.name = "enableWeatherCheckbox";
            //_enableWeatherCheckbox.tooltip = "Check this box to enable Dynamic Weather. This setting is the same as the Dynamic Weather option in the Gameplay Options panel.";
            _enableWeatherCheckbox.isChecked = EyeCandyXTool.optionsGameplayPanel.enableWeather;
            _enableWeatherCheckbox.eventCheckChanged += CheckboxChanged;

            _enableWeatherCheckbox.label.text = "Weather";

            //  Precipitation intensity:
            var precipitationContainer = UIUtils.CreateFormElement(this, "center");
            precipitationContainer.name = "precipitationSliderContainer";
            precipitationContainer.relativePosition = new Vector3(0, 80);

            _precipitationLabel = precipitationContainer.AddUIComponent<UILabel>();
            _precipitationLabel.textScale = 0.9f;
            _precipitationLabel.padding = new RectOffset(0, 0, 0, 5);

            _precipitationSlider = UIUtils.CreateSlider(precipitationContainer, 0, 2.5f);
            _precipitationSlider.name = "precipitationSlider";
            _precipitationSlider.stepSize = 0.05f;
            //_precipitationSlider.tooltip = "Move this slider to the right to increase Rain Intensity.\nDynamic Weather will be enabled if necessary.";
            _precipitationSlider.eventValueChanged += SliderChanged;
            //  Winter map?
            if (EyeCandyXTool.isWinterMap)
            {
                //  Snow intensity:
                _precipitationLabel.text = "Snowfall intensity (0)";
                //_precipitationSlider.tooltip = "Move this slider to the right to increase Snow Intensity.\nDynamic Weather will be enabled if necessary.";
            }
            //  Non-winter map:
            else
            {
                //  Rain intensity:
                _precipitationLabel.text = "Rain intensity (0)";
                //_precipitationSlider.tooltip = "Move this slider to the right to increase Rain Intensity.\nDynamic Weather will be enabled if necessary.";
            }

            //  Precipitation motion blur:
            var rainMotionblurContainer = UIUtils.CreateFormElement(this, "center");
            rainMotionblurContainer.name = "rainMotionblurCheckboxContainer";
            rainMotionblurContainer.relativePosition = new Vector3(0, 155);
            rainMotionblurContainer.autoLayout = false;

            _rainMotionblurCheckbox = UIUtils.CreateCheckBox(rainMotionblurContainer);
            _rainMotionblurCheckbox.name = "rainMotionblurCheckbox";
            //_rainMotionblurCheckbox.tooltip = "Check this box to enable the Rain Motion Blur Effect. This setting is mainly visible when the game is paused.";
            _rainMotionblurCheckbox.isChecked = true;
            _rainMotionblurCheckbox.eventCheckChanged += CheckboxChanged;
            _rainMotionblurCheckbox.label.text = "Rain motion blur";
            //  Hide on winter maps:
            rainMotionblurContainer.isVisible = (!EyeCandyXTool.isWinterMap);

            //  Fog intensity:
            var fogContainer = UIUtils.CreateFormElement(this, "center");
            fogContainer.name = "fogIntensitySliderContainer";
            fogContainer.relativePosition = (EyeCandyXTool.isWinterMap) ? new Vector3(0, 155) : new Vector3(0, 210);

            _fogIntensityLabel = fogContainer.AddUIComponent<UILabel>();
            _fogIntensityLabel.text = "Fog intensity (0)";
            _fogIntensityLabel.textScale = 0.9f;
            _fogIntensityLabel.padding = new RectOffset(0, 0, 0, 5);

            _fogIntensitySlider = UIUtils.CreateSlider(fogContainer, -0.485f, 1f);
            _fogIntensitySlider.name = "fogIntensitySlider";
            _fogIntensitySlider.stepSize = 0.01f;
            //_fogIntensitySlider.tooltip = "Move this slider to the right to increase Fog Density.\nDynamic Weather will be enabled if necessary.";
            _fogIntensitySlider.eventValueChanged += SliderChanged;

            //  Ground Wetness:
            //var wetnessContainer = UIUtils.CreateFormElement(this, "center");
            //wetnessContainer.name = "wetnessContainer";
            //wetnessContainer.relativePosition = (EyeCandyX.isWinterMap) ? new Vector3(0, 156) : new Vector3(0, 202);

            //_wetnessLabel = wetnessContainer.AddUIComponent<UILabel>();
            //_wetnessLabel.text = "Ground wetness (" + WeatherManager.instance.m_groundWetness + ")";
            //_wetnessLabel.textScale = 0.9f;
            //_wetnessLabel.padding = new RectOffset(0, 0, 0, 5);

            //_wetnessSlider = UIUtils.CreateSlider(wetnessContainer, 0f, 1f);
            //_wetnessSlider.name = "wetnessSlider";
            //_wetnessSlider.tooltip = "Move this slider to change ground wetness.";
            //_wetnessSlider.stepSize = 0.01f;
            //_wetnessSlider.value = WeatherManager.instance.m_groundWetness;
            //_wetnessSlider.eventValueChanged += SliderChanged;

            //  Reset button:
            var resetContainer = UIUtils.CreateFormElement(this, "bottom");

            _resetWeatherButton = UIUtils.CreateButton(resetContainer);
            _resetWeatherButton.name = "resetButton";
            _resetWeatherButton.text = "Reset";
            _resetWeatherButton.tooltip = "Reset all values set in this panel to default values.";
            _resetWeatherButton.eventClicked += (c, e) =>
            {
                if (EyeCandyXTool.config.outputDebug)
                {
                    DebugUtils.Log($"WeatherPanel: 'Reset' clicked.");
                }
                //  
                _enableWeatherCheckbox.isChecked = false;
                _precipitationSlider.value = 0;
                _rainMotionblurCheckbox.isChecked = false;
                _fogIntensitySlider.value = 0;
                //_wetnessSlider.value = 0;
            };
        }

        private void SliderChanged(UIComponent trigger, float value)
        {
            if (EyeCandyXTool.config.outputDebug)
            {
                DebugUtils.Log($"WeatherPanel: Slider {trigger.name} = {value}");
            }
            //  
            if (trigger == _precipitationSlider)
            {
                WeatherManager.instance.m_currentRain = value;
                if (EyeCandyXTool.isWinterMap)
                {
                    EyeCandyXTool.currentSettings.weather_snowintensity = value;
                    _precipitationLabel.text = "Snowfall intensity (" + value.ToString() + ")";
                }
                else
                {
                    EyeCandyXTool.currentSettings.weather_rainintensity = value;
                    _precipitationLabel.text = "Rain intensity (" + value.ToString() + ")";
                }
                //  Enable dynamic weather if disabled (required for rainfall):
                if (!EyeCandyXTool.optionsGameplayPanel.enableWeather)
                {
                    EyeCandyXTool.currentSettings.weather = true;
                    EyeCandyXTool.optionsGameplayPanel.enableWeather = true;
                    _enableWeatherCheckbox.isChecked = true;
                }
            }
            else if (trigger == _fogIntensitySlider)
            {
                WeatherManager.instance.m_currentFog = value;
                EyeCandyXTool.currentSettings.weather_fogintensity = value;
                _fogIntensityLabel.text = "Fog intensity (" + value.ToString() + ")";
                //  Enable dynamic weather if disabled (required for rainfall):
                if (!EyeCandyXTool.optionsGameplayPanel.enableWeather)
                {
                    EyeCandyXTool.currentSettings.weather = true;
                    EyeCandyXTool.optionsGameplayPanel.enableWeather = true;
                    _enableWeatherCheckbox.isChecked = true;
                }
            }
            //else if (trigger == _wetnessSlider)
            //{
            //    WeatherManager.instance.m_groundWetness = value;
            //    NetManager.instance.m_lastMaxWetness = (int)value;
            //    NetManager.instance.m_currentMaxWetness = (int)value;
            //    EyeCandyX.currentSettings.weather_wetness = value;
            //    NetManager.instance.m_wetnessChanged = (int)value;
            //    _wetnessLabel.text = "Ground wetness (" + value.ToString() + ")";
            //}
        }

        private void CheckboxChanged(UIComponent trigger, bool isChecked)
        {
            if (EyeCandyXTool.config.outputDebug)
            {
                DebugUtils.Log($"WeatherPanel: Checkbox {trigger.name} = {isChecked}");
            }
            //  
            if (trigger == _enableWeatherCheckbox)
            {
                EyeCandyXTool.currentSettings.weather = isChecked;
                EyeCandyXTool.optionsGameplayPanel.enableWeather = isChecked;
            }
            else if (trigger == _rainMotionblurCheckbox)
            {
                var rrp = FindObjectOfType<RainParticleProperties>();
                rrp.ForceRainMotionBlur = isChecked;
                EyeCandyXTool.currentSettings.weather_rainmotionblur = isChecked;
            }
        }
    }
}
