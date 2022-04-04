using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JuicyChicken
{
    public class Transform
    {
        private Texture2D debugTexture;

        /// <summary>
        /// The current position of the transform
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The current rotation of the transform
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// The current scale of the transform
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// The forward direction of the transform with rotation taken into account
        /// </summary>
        public Vector2 Right
        {
            get
            {
                Vector2 direction = new Vector2(MathF.Cos(MathHelper.ToRadians(Rotation)), MathF.Sin(MathHelper.ToRadians(Rotation)));
                if (direction.Length() != 0f)
                    direction.Normalize();

                return direction;
            }
        }

        /// <summary>
        /// The up direction of the transform with rotation taken into account
        /// </summary>
        public Vector2 Up
        {
            get
            {
                Vector2 direction = new Vector2(MathF.Sin(MathHelper.ToRadians(Rotation)), -MathF.Cos(MathHelper.ToRadians(Rotation)));
                if (direction.Length() != 0f)
                    direction.Normalize();

                return direction;
            }
        }

        /// <summary>
        /// Constructs a Transform with default parameters
        /// </summary>
        public Transform()
        {
            Position = Vector2.Zero;
            Rotation = 0f;
            Scale = Vector2.One;
        }

        /// <summary>
        /// Adds a new position to the current position of the transform
        /// </summary>
        public void Translate(Vector2 direction)
        {
            Position += direction;
        }

        /// <summary>
        /// Adds a new rotation to the current rotation of the transform
        /// </summary>
        public void Rotate(float angle)
        {
            Rotation += angle;
        }

        /// <summary>
        /// Clamps the current position of the transform
        /// </summary>
        public void ClampPosition(Vector2 minValue, Vector2 maxValue)
        {
            ClampX(minValue.X, maxValue.X);
            ClampY(minValue.Y, maxValue.Y);
        }

        /// <summary>
        /// Clamps the X value of the current position of the transform
        /// </summary>
        public void ClampX(float minValue, float maxValue)
        {
            Position = new Vector2(MathHelper.Clamp(Position.X, minValue, maxValue), Position.Y);
        }

        /// <summary>
        /// Clamps the Y value of the current position of the transform
        /// </summary>
        public void ClampY(float minValue, float maxValue)
        {
            Position = new Vector2(Position.X, MathHelper.Clamp(Position.Y, minValue, maxValue));
        }

        /// <summary>
        /// Adjusts rotation to look towards a specified target
        /// </summary>
        public void LookAt(Vector2 target)
        {
            Vector2 distance = target - Position;
            Rotation = MathHelper.ToDegrees((float)Math.Atan2(distance.Y, distance.X));
        }

        /// <summary>
        /// Gets a rotation that look towards a specified target
        /// </summary>
        /// <returns>Look rotation in degrees (angle)</returns>
        public float GetLookRotation(Vector2 target)
        {
            Vector2 distance = target - Position;
            float rotation = (float)Math.Atan2(distance.Y, distance.X);

            return MathHelper.ToDegrees(rotation);
        }

        /// <summary>
        /// Gets a directional vector towards a specified target
        /// </summary>
        /// <returns>A Vector2 direction</returns>
        public Vector2 GetLookDirection(Vector2 target)
        {
            float lookAngle = MathHelper.ToRadians(GetLookRotation(target));
            return new Vector2(MathF.Cos(lookAngle), MathF.Sin(lookAngle));
        }

        /// <summary>
        /// Draws the X and Y axis onto the screen
        /// </summary>
        public void DrawDebug(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            if (debugTexture == null)
            {
                int width = 2;
                int height = 2;
                debugTexture = new Texture2D(graphics, width, height);

                Color[] data = new Color[width * height];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = Color.White;
                }

                debugTexture.SetData(data);
            }

            // Draw X axis
            spriteBatch.Draw(debugTexture, Position, null, Color.Red,
                        MathHelper.ToRadians(GetLookRotation(Position + Right)), Vector2.Zero, Vector2.One + Vector2.UnitX * 30f, SpriteEffects.None, 1f);

            // Draw Y axis
            spriteBatch.Draw(debugTexture, Position, null, Color.LightGreen,
                        MathHelper.ToRadians(GetLookRotation(Position + Up)), Vector2.Zero, Vector2.One + Vector2.UnitX * 30f, SpriteEffects.None, 1f);
        }
    }
}