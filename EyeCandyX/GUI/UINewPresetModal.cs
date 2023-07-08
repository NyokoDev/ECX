using ColossalFramework;
using ColossalFramework.UI;
using EyeCandyX.TranslationFramework;
using UnityEngine;

namespace EyeCandyX.GUI
{
    public class UINewPresetModal : UIPanel
    {
        private UIModalTitleBar m_title;
        private UITextField m_name;
        private UIButton m_ok;
        private UIButton m_cancel;

        private static UINewPresetModal _instance;

        public static UINewPresetModal instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = UIView.GetAView().AddUIComponent(typeof(UINewPresetModal)) as UINewPresetModal;
                }
                return _instance;
            }
        }

        public override void Start()
        {
            base.Start();

            backgroundSprite = "LevelBarBackground";
            isVisible = false;
            canFocus = true;
            isInteractive = true;
            size = new Vector2(250, 135);
            relativePosition = new Vector3(Mathf.Floor((GetUIView().fixedWidth - width) / 2), Mathf.Floor((GetUIView().fixedHeight - height) / 2));

            // Title Bar:
            m_title = AddUIComponent<UIModalTitleBar>();
            m_title.title = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.CreateNewPreset);
            m_title.isModal = true;
            m_title.relativePosition = new Vector3(10, 5);

            // Name:
            UILabel name = AddUIComponent<UILabel>();
            name.height = 30;
            name.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.PresetName);
            name.textScale = 0.8f;
            name.relativePosition = new Vector3(10, m_title.height);

            m_name = UIUtils.CreateTextField(this);
            m_name.normalBgSprite = "TextFieldUnderline";
            m_name.width = width - 20;
            m_name.height = 25;
            m_name.padding = new RectOffset(6, 6, 6, 6);
            m_name.textColor = new Color32(222, 222, 222, 255);
            m_name.textScale = 0.8f;
            //m_name.tooltip = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.PresetNameTooltip);
            m_name.relativePosition = new Vector3(10, name.relativePosition.y + name.height + 5);

            m_name.Focus();
            m_name.eventTextChanged += (c, s) =>
            {
                m_ok.text = (!s.IsNullOrWhiteSpace() && EyeCandyXTool.GetPresetByName(s) == null) ? Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.Create) : Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.OverwritePreset);
                m_name.tooltip = (EyeCandyXTool.GetPresetByName(s) != null) ? Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.PresetExists) : Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.PresetNameTooltip);
                m_ok.isEnabled = !s.IsNullOrWhiteSpace();
            };

            m_name.eventTextSubmitted += (c, s) =>
            {
                if (m_ok.isEnabled && Input.GetKey(KeyCode.Return)) m_ok.SimulateClick();
            };

            // Ok button:
            m_ok = UIUtils.CreateButton(this);
            m_ok.relativePosition = new Vector3(11, m_name.relativePosition.y + m_name.height + 10);
            m_ok.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.Create);
            m_ok.isEnabled = false;
            m_ok.eventClick += (c, p) =>
            {
                if (EyeCandyXTool.GetPresetByName(m_name.text) != null)
                {
                    //  Overwrite, confirm first:
                    ConfirmPanel.ShowModal(Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.OverwritePreset), string.Format(Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.ConfirmOverwrite), m_name.text), (d, i) => {
                        if (i == 1)
                        {
                            EyeCandyXTool.CreatePreset(m_name.text, true);
                            UIView.PopModal();
                            Hide();
                        }
                    });
                }
                else
                {
                    //  Create new:
                    EyeCandyXTool.CreatePreset(m_name.text, false);
                    UIView.PopModal();
                    Hide();
                }
            };

            // Cancel button:
            m_cancel = UIUtils.CreateButton(this);
            m_cancel.relativePosition = new Vector3(width - m_cancel.width - 11, m_ok.relativePosition.y + 1);
            m_cancel.text = Translation.Instance.GetTranslation(EyecandyX.Locale.TranslationID.Cancel);
            m_cancel.eventClick += (c, p) =>
            {
                UIView.PopModal();
                Hide();
            };
            //  
            isVisible = true;
        }

        protected override void OnVisibilityChanged()
        {
            base.OnVisibilityChanged();

            UIComponent modalEffect = GetUIView().panelsLibraryModalEffect;

            if (isVisible)
            {
                m_name.text = "";
                m_name.Focus();

                if (modalEffect != null)
                {
                    modalEffect.Show(false);
                    ValueAnimator.Animate("NewThemeModalEffect", delegate (float val)
                    {
                        modalEffect.opacity = val;
                    }, new AnimatedFloat(0f, 1f, 0.7f, EasingType.CubicEaseOut));
                }
            }
            else if (modalEffect != null)
            {
                ValueAnimator.Animate("NewThemeModalEffect", delegate (float val)
                {
                    modalEffect.opacity = val;
                }, new AnimatedFloat(1f, 0f, 0.7f, EasingType.CubicEaseOut), delegate
                {
                    modalEffect.Hide();
                });
            }
        }

        protected override void OnKeyDown(UIKeyEventParameter p)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                p.Use();
                UIView.PopModal();
                Hide();
            }

            base.OnKeyDown(p);
        }
    }
}
