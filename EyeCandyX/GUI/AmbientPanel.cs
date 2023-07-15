using ColossalFramework;
using ColossalFramework.UI;
using System;
using EyeCandyX;
using UnityEngine;
using ICities;
using CompCheck;
using System.IO;
using EyeCandyX.TranslationFramework;

namespace EyeCandyX.GUI
{
    public class AmbientPanel : UIPanel
    {
        private UILabel _todLabel;
        private UISlider _todSlider;
        private UILabel _speedLabel;
        private UISlider _speedSlider;
        private UILabel _heightLabel;
        private UISlider _heightSlider;
        private UILabel _rotationLabel;
        private UISlider _rotationSlider;
        private UILabel _intensityLabel;
        private UISlider _intensitySlider;
        private UILabel _ambientLabel;
        private UISlider _ambientSlider;
        private UILabel _gameSpeedLabel;
        private UISlider _gameSpeedSlider;

        private UIButton _resetAmbientButton;

        public UISlider todSlider
        {
            get { return _todSlider; }
            set { _todSlider = value; }
        }

        public UISlider speedSlider
        {
            get { return _speedSlider; }
            set { _speedSlider = value; }
        }

        public UISlider heightSlider
        {
            get { return _heightSlider; }
            set { _heightSlider = value; }
        }

        public UISlider rotationSlider
        {
            get { return _rotationSlider; }
        }

        public UISlider intensitySlider
        {
            get { return _intensitySlider; }
        }

        public UISlider ambientSlider
        {
            get { return _ambientSlider; }
        }

        public UISlider gameSpeedSlider
        {
            get { return _gameSpeedSlider; }
        }

        private DayNightCycleManager _todManager;
        public DayNightCycleManager todManager
        {
            get { return _todManager; }
            set { _todManager = value; }
        }

        private static Fraction[] speeds = {
            new Fraction(){num=0, den=1},
            new Fraction(){num=1, den=128},
            new Fraction(){num=1, den=64},
            new Fraction(){num=1, den=16},
            new Fraction(){num=1, den=8},
            new Fraction(){num=1, den=4},
            new Fraction(){num=1, den=2},
            new Fraction(){num=1, den=1},
            new Fraction(){num=2, den=1},
            new Fraction(){num=4, den=1},
            new Fraction(){num=8, den=1},
            new Fraction(){num=16, den=1},
            new Fraction(){num=32, den=1},
            new Fraction(){num=64, den=1},
            new Fraction(){num=128,den=1}
        };

        bool pauseUpdates;

        Color32 daytimeColor = new Color32(235, 255, 92, 255);
        Color32 nighttimeColor = new Color32(24, 84, 255, 255);

        private static AmbientPanel _instance;
        public static AmbientPanel instance
        {
            get { return _instance; }
        }

        public override void Start()
        {
            base.Start();
            _instance = this;
            canFocus = true;
            isInteractive = true;
            SetupControls();
        }

        private void SetupControls()
        {
            // Time of Day:
            {
                var topContainer = UIUtils.CreateFormElement(this, "top");
                topContainer.name = "heightSliderContainer";
                _todLabel = topContainer.AddUIComponent<UILabel>();
                _todLabel.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.TIME_DAY_TEXT);
                _todLabel.textScale = 0.9f;
                _todLabel.padding = new RectOffset(0, 0, 0, 0);
                _todSlider = UIUtils.CreateSlider(topContainer, 0.0f, 24.0f);
                _todSlider.name = "todSlider";
                _todSlider.stepSize = 1f / 60.0f;
                _todSlider.eventValueChanged += ValueChanged;
                _todSlider.eventDragStart += timeSlider_eventDragStart;
                _todSlider.eventMouseUp += timeSlider_eventDragEnd;
            }

            // day night cycle speed:
            var speedContainer = UIUtils.CreateFormElement(this, "center");
            speedContainer.name = "sizeContainer";
            speedContainer.relativePosition = new Vector3(0, 95);
            _speedLabel = speedContainer.AddUIComponent<UILabel>();
            _speedLabel.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.DAYNIGHTCYCLE_TEXT);
            _speedLabel.textScale = 0.9f;
            _speedLabel.padding = new RectOffset(0, 0, 0, 0);
            _speedSlider = UIUtils.CreateSlider(speedContainer, 0f, speeds.Length);
            _speedSlider.name = "speedSlider";
            _speedSlider.stepSize = 0.005f;
            _speedSlider.eventValueChanged += ValueChanged;

            // Sun height (Latitude):
            var heightContainer = UIUtils.CreateFormElement(this, "center");
            heightContainer.name = "heightContainer";
            heightContainer.relativePosition = new Vector3(0, 160);
            _heightLabel = heightContainer.AddUIComponent<UILabel>();
            _heightLabel.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.LAT_TEXT) + "(0)";
            _heightLabel.textScale = 0.9f;
            _heightLabel.padding = new RectOffset(0, 0, 0, 0);
            _heightSlider = UIUtils.CreateSlider(heightContainer, -120f, 120f);
            _heightSlider.name = "heightSlider";
            _heightSlider.stepSize = 0.005f;
            _heightSlider.value = EyeCandyXTool.currentSettings.ambient_height;
            _heightSlider.eventValueChanged += ValueChanged;

            // Sun rotation (Longitude):
            var rotationContainer = UIUtils.CreateFormElement(this, "center");
            rotationContainer.name = "rotationContainer";
            rotationContainer.relativePosition = new Vector3(0, 225);
            _rotationLabel = rotationContainer.AddUIComponent<UILabel>();
            _rotationLabel.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.LOT_TEXT) + "(0)";
            _rotationLabel.textScale = 0.9f;
            _rotationLabel.padding = new RectOffset(0, 0, 0, 0);
            _rotationSlider = UIUtils.CreateSlider(rotationContainer, -180f, 180f);
            _rotationSlider.name = "rotationSlider";
            _rotationSlider.stepSize = 0.005f;
            _rotationSlider.value = EyeCandyXTool.currentSettings.ambient_rotation;
            _rotationSlider.eventValueChanged += ValueChanged;

            // Global light intensity:
            var intensityContainer = UIUtils.CreateFormElement(this, "center");
            intensityContainer.name = "intensityContainer";
            intensityContainer.relativePosition = new Vector3(0, 290);
            _intensityLabel = intensityContainer.AddUIComponent<UILabel>();
            _intensityLabel.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.SUNINTENSITY) + "(0)";
            _intensityLabel.textScale = 0.9f;
            _intensityLabel.padding = new RectOffset(0, 0, 0, 0);
            _intensitySlider = UIUtils.CreateSlider(intensityContainer, 0f, 10f);
            _intensitySlider.name = "intensitySlider";
            _intensitySlider.stepSize = 0.0005f;
            _intensitySlider.value = EyeCandyXTool.currentSettings.ambient_intensity;
            _intensitySlider.eventValueChanged += ValueChanged;

            // Ambient light intensity:
            var ambientContainer = UIUtils.CreateFormElement(this, "center");
            ambientContainer.name = "ambientContainer";
            ambientContainer.relativePosition = new Vector3(0, 355);
            _ambientLabel = ambientContainer.AddUIComponent<UILabel>();
            _ambientLabel.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.EXPOSURE) + "(0)";
            _ambientLabel.textScale = 0.9f;
            _ambientLabel.padding = new RectOffset(0, 0, 0, 0);
            _ambientSlider = UIUtils.CreateSlider(ambientContainer, 0f, 2f);
            _ambientSlider.name = "ambientSlider";
            _ambientSlider.stepSize = 0.0005f;
            _ambientSlider.value = EyeCandyXTool.currentSettings.ambient_ambient;
            _ambientSlider.eventValueChanged += ValueChanged;

            // Game Speed:
            var gameSpeedContainer = UIUtils.CreateFormElement(this, "center");
            gameSpeedContainer.name = "gameSpeedContainer";
            gameSpeedContainer.relativePosition = new Vector3(0, 420);
            _gameSpeedLabel = gameSpeedContainer.AddUIComponent<UILabel>();
            _gameSpeedLabel.text = "Game Speed (0)";
            _gameSpeedLabel.textScale = 0.9f;
            _gameSpeedLabel.padding = new RectOffset(0, 0, 0, 0);
            _gameSpeedSlider = UIUtils.CreateSlider(gameSpeedContainer, 0f, 2f);
            _gameSpeedSlider.name = "gameSpeedSlider";
            _gameSpeedSlider.stepSize = 0.01f;
            _gameSpeedSlider.value = Time.timeScale;
            _gameSpeedSlider.eventValueChanged += ValueChanged;

            // Reset button:
            var resetContainer = UIUtils.CreateFormElement(this, "bottom");
            _resetAmbientButton = UIUtils.CreateButton(resetContainer);
            _resetAmbientButton.name = "resetButton";
            _resetAmbientButton.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.RESET_BUTTON_TEXT);
            _resetAmbientButton.tooltip = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.RESET_VALUES_TOOLTIP);
            _resetAmbientButton.eventClicked += (c, e) =>
            {
                if (EyeCandyXTool.config.outputDebug)
                {
                    DebugUtils.Log($"AmbientPanel: 'Reset' clicked.");
                }
                _heightSlider.value = EyeCandyXTool.isWinterMap ? 66f : 35f;
                _rotationSlider.value = 98f;
                _intensitySlider.value = 6f;
                _ambientSlider.value = EyeCandyXTool.isWinterMap ? 0.4f : 0.71f;
                _gameSpeedSlider.value = 1f;
                Time.timeScale = 1f;
                _gameSpeedLabel.text = "Game Speed (1)";
            };
        }

        void ValueChanged(UIComponent trigger, float value)
        {
            if (EyeCandyXTool.config.outputDebug)
            {
                DebugUtils.Log($"AmbientPanel: Slider {trigger.name} = {value}");
            }

            if (trigger == _todSlider)
            {
                _todManager.TimeOfDay = value;
                
            }
            else if (trigger == _speedSlider)
            {
                _todManager.speed = speeds[(int)value];
                _speedLabel.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.DAYNIGHTCYCLE_TEXT) + "(" + value.ToString() + ")";
            }
            else if (trigger == _heightSlider)
            {
                DayNightProperties.instance.m_Latitude = value;
                EyeCandyXTool.currentSettings.ambient_height = value;
                _heightLabel.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.LAT_TEXT) + "(" + value.ToString() + ")";
            }
            else if (trigger == _rotationSlider)
            {
                DayNightProperties.instance.m_Longitude = value;
                EyeCandyXTool.currentSettings.ambient_rotation = value;
                _rotationLabel.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.LOT_TEXT) + "(" + value.ToString() + ")";
            }
            else if (trigger == _intensitySlider)
            {
                DayNightProperties.instance.m_SunIntensity = value;
                EyeCandyXTool.currentSettings.ambient_intensity = value;
                _intensityLabel.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.SUNINTENSITY) + "(" + value.ToString() + ")";
            }
            else if (trigger == _ambientSlider)
            {
                DayNightProperties.instance.m_Exposure = value;
                EyeCandyXTool.currentSettings.ambient_ambient = value;
                _ambientLabel.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.EXPOSURE) + "(" + value.ToString() + ")";
            }
            else if (trigger == _gameSpeedSlider)
            {
                Time.timeScale = value;
                _gameSpeedLabel.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.GAMESPEED) + "(" + value.ToString() + ")";
            }
        }

        void timeSlider_eventDragEnd(UIComponent trigger, UIMouseEventParameter eventParam)
        {
            if (EyeCandyXTool.config.outputDebug)
            {
                DebugUtils.Log($"AmbientPanel: Slider {trigger.name} drag end.");
            }
            pauseUpdates = false;
        }

        void timeSlider_eventDragStart(UIComponent trigger, UIDragEventParameter eventParam)
        {
            if (EyeCandyXTool.config.outputDebug)
            {
                DebugUtils.Log($"AmbientPanel: Slider {trigger.name} drag start.");
            }
            pauseUpdates = true;
        }

        public override void Update()
        {
            if (EyeCandyXTool.isGameLoaded)
            {
                if (!EyeCandyXTool.config.enableSimulationControl)
                {
                    _todSlider.isEnabled = false;
                    _todLabel.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.TIME_DAY_TEXT_DISABLED);
                    //  
                    _speedLabel.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.DNC_SPEED_DISABLED);
                    _speedSlider.value = 0;
                    _speedSlider.isEnabled = false;
                    return;
                }
                if (_todManager != null)
                {
                    if (_todManager.DayNightEnabled)
                    {
                        float tod = _todManager.TimeOfDay;
                        int hour = (int)Math.Floor(tod);
                        int minute = (int)Math.Floor((tod - hour) * 60.0f);
                        _todLabel.text = string.Format(Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.TIME_DAY_TEXT) + "(currently: {0,2:00}:{1,2:00})", hour, minute);
                        _todSlider.isEnabled = true;
                        //  
                        if (!pauseUpdates)
                        {
                            _todSlider.value = todManager.TimeOfDay;
                        }
                        //  
                        float fade = Math.Abs(_todManager.TimeOfDay - 12.0f) / 12.0f;
                        ((UISprite)_todSlider.thumbObject).color = Color32.Lerp(daytimeColor, nighttimeColor, fade);
                        //  
                        _speedLabel.text = $"{Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.DAYNIGHTCYCLE_TEXT)}{_todManager.speed}";
                        _speedSlider.value = Array.IndexOf(speeds, _todManager.speed);
                        _speedSlider.isEnabled = true;
                    }
                    else
                    {
                        _todLabel.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.TIME_DAY_TEXT_DISABLED);
                        _todSlider.isEnabled = false;
                        //  
                        _speedLabel.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.DNC_SPEED_DISABLED);
                        _speedSlider.value = 0;
                        _speedSlider.isEnabled = false;
                    }
                }
            }
        }
    }
}
