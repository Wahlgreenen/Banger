using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JuicyChicken.Input;

namespace JuicyChicken
{
    public class Button : UIElement
    {
        private bool hovered;

        public event Action OnClick;
        public event Action OnHoverEnter;
        public event Action OnHoverExit;
        private MouseAction mouse = new MouseAction(); 

        protected override void Create()
        {
            base.Create();
            mouse.OnLeftClickDown += () =>
            {
                if (hovered)
                {
                    OnClick?.Invoke();
                    System.Diagnostics.Debug.WriteLine("clicked button");

                    hovered = false;
                }
            };
        }

        private void EnterButton()
        {
            Vector2 mousePos = InputChecker.CurrentMousePosition;

            if (mousePos.X < Transform.Position.X + Sprite.Texture.Width * Graphics.AspectRatio.X &&
                mousePos.X > Transform.Position.X &&
                mousePos.Y < Transform.Position.Y + Sprite.Texture.Height * Graphics.AspectRatio.Y &&
                mousePos.Y > Transform.Position.Y)
            {
                if (!hovered)
                {
                    OnHoverEnter?.Invoke();
                    System.Diagnostics.Debug.WriteLine("entered button");
                    hovered = true;
                }
            }
            else
            {
                if (hovered)
                {
                    OnHoverExit?.Invoke();
                    System.Diagnostics.Debug.WriteLine("exited button");

                    hovered = false;
                }
            }
        }

        protected override void Update()
        {
            EnterButton();
        }

    }
}
