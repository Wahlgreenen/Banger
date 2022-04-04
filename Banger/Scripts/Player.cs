using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using JuicyChicken;
using JuicyChicken.Input;

namespace Banger
{
    class Player : GameObject
    {
        private int maxHealth;
        private int currentHealth;
        private float speed;
        private Vector2 moveDirection;
        private DirectionAction moveInput = new DirectionAction();
        public bool IsMoving { get => moveDirection.Length() != 0; }

        public event Action OnMove;

        protected override void Create()
        {
            Sprite.Texture = Content.GetTexture("player1");
            Sprite.Origin = Sprite.Texture.GetPoint(TexturePoint.Bottom);
            
            speed = 500f;
            moveInput.OnDirectionInput += HandleMovement;
            moveInput.OnDirectionInput += HandleAnimation;

            BoxCollider collider = AddComponent<BoxCollider>();
            collider.Bounds = Sprite.Texture.GetSize() * Transform.Scale;
            collider.Offset -= Sprite.Texture.GetPoint(TexturePoint.Bottom);

            Camera.Target = Transform.Position - Vector2.UnitY * Sprite.Texture.Height / 2;
            Camera.Lerp = true;
        }

        protected override void Update()
        {
            Transform.Scale = Vector2.Lerp(Transform.Scale, Vector2.One, 10f * Time.DeltaTime);
            Transform.Rotation = MathHelper.Lerp(Transform.Rotation, 0f, 10f * Time.DeltaTime);
        }

        private void HandleMovement(Vector2 direction)
        {
            Camera.Target = Transform.Position - Vector2.UnitY * Sprite.Texture.Height / 2;

            direction.Normalize();
            moveDirection = direction;
            Transform.Translate((moveDirection * speed * Time.DeltaTime));
            OnMove?.Invoke();
        }

        private void HandleAnimation(Vector2 direction)
        {
            Transform.Scale = new Vector2(Transform.Scale.X, ((float)Math.Sin(Time.TotalTime * 20) / 4) + 1f);
            Transform.Rotation = 6f * (float)Math.Sin(Time.TotalTime * 22f);
            Sprite.FlipMode = direction.X < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        }
    }
}
