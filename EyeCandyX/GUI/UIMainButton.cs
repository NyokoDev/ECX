﻿using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace EyeCandyX.GUI
{
    public class UIMainButton : UIButton
    {
        public static UIMainButton instance;
        private bool dragging = false;

        public override void Start()
        {
            base.Start();
            instance = this;

            const int buttonSize = 36;
            UITextureAtlas toggleButtonAtlas = null;
            string UE = "EyeCandyX";

            // Positioned relative to Freecamera Button:
            var freeCameraButton = UIView.GetAView().FindUIComponent<UIButton>("Freecamera");
            verticalAlignment = UIVerticalAlignment.Middle;

            if (EyeCandyXTool.config.buttonPos.x == -9999)
            {
                absolutePosition = new Vector2(freeCameraButton.absolutePosition.x - (3 * buttonSize) - 5, freeCameraButton.absolutePosition.y);
            }
            else
            {
                absolutePosition = EyeCandyXTool.config.buttonPos;
            }

            size = new Vector2(36f, 36f);
            playAudioEvents = true;
            tooltip = "Eyecandy X";

            // Create custom atlas:
            if (toggleButtonAtlas == null)
            {
                toggleButtonAtlas = UIUtils.CreateAtlas(UE, buttonSize, buttonSize, "ToolbarIcon.png", new[]
                {
                                                "EyecandyNormalBg",
                                                "EyecandyHoveredBg",
                                                "EyecandyPressedBg",
                                                "EyecandyNormalFg",
                                                "EyecandyHoveredFg",
                                                "EyecandyPressedFg",
                                                "EyecandyButtonNormal",
                                                "EyecandyButtonHover",
                                                "EyecandyInfoTextBg",
                                            });
            }

            // Apply custom sprite:
            atlas = toggleButtonAtlas;
            normalFgSprite = "EyecandyNormalFg";
            normalBgSprite = "EyecandyNormalBg";
            hoveredFgSprite = "EyecandyHoveredBg";
            hoveredBgSprite = "EyecandyHoveredFg";
            pressedFgSprite = "EyecandyPressedBg";
            pressedBgSprite = "EyecandyPressedFg";
            focusedFgSprite = "EyecandyPressedBg";
            focusedBgSprite = "EyecandyPressedFg";
        }

        protected override void OnClick(UIMouseEventParameter p)
        {
            if (p.buttons.IsFlagSet(UIMouseButton.Left))
            {
                UIMainPanel.instance.Toggle();
            }

            base.OnClick(p);
        }

        protected override void OnMouseDown(UIMouseEventParameter p)
        {
            if (p.buttons.IsFlagSet(UIMouseButton.Right))
            {
                dragging = true;
            }
            base.OnMouseDown(p);
        }

        protected override void OnMouseUp(UIMouseEventParameter p)
        {
            if (p.buttons.IsFlagSet(UIMouseButton.Right))
            {
                dragging = false;
            }
            base.OnMouseUp(p);
        }

        protected override void OnMouseMove(UIMouseEventParameter p)
        {
            if (p.buttons.IsFlagSet(UIMouseButton.Right))
            {
                var ratio = UIView.GetAView().ratio;
                position = new Vector3(position.x + (p.moveDelta.x * ratio), position.y + (p.moveDelta.y * ratio), position.z);
                //  
                EyeCandyXTool.config.buttonPos = absolutePosition;
                EyeCandyXTool.SaveConfig();
                //  
                if (EyeCandyXTool.config.outputDebug)
                {
                    DebugUtils.Log($"Button position changed to {absolutePosition}.");
                }
            }
            base.OnMouseMove(p);
        }
    }
}