using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JuicyChicken
{
    public class Sprite : Component
    {
        private float opacity = 1f;
        private Texture2D colorMaskTexture;

        /// <summary>
        /// The texture being drawn to the screen
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// The order the texture is being drawn to the screen between 0 and 1
        /// </summary>
        public int Layer { get; set; }

        /// <summary>
        /// Automatically sets the layer to be equal to the Transform Y position
        /// </summary>
        public bool AutoLayer { get; set; }

        /// <summary>
        /// The pivot point an object will rotate around
        /// </summary>
        public Vector2 Origin { get; set; }

        /// <summary>
        /// The sprite will draw at it's Transform position + this offset value
        /// </summary>
        public Vector2 Offset { get; set; }

        /// <summary>
        /// Whether or not the sprite should be flipped horizontally or vertically
        /// </summary>
        public SpriteEffects FlipMode { get; set; }

        /// <summary>
        /// The alpha value of the sprite color
        /// </summary>
        public float Opacity { get => opacity; set => opacity = Math.Clamp(value, 0f, 1f); }

        /// <summary>
        /// If this color is set, it will draw an overlay color on top of the sprite
        /// </summary>
        public Color ColorMask { get; set; }

        public Space Space { get; set; } = Space.World;

        public override void Update()
        {
            Layer = AutoLayer ? (int)Parent.Transform.Position.Y - (int)Offset.Y : Layer;
        }

        public override void Draw(Space drawMode)
        {
            if (Texture == null || Space != drawMode)
                return;

            float layerDepth = ConvertToLayerDepth(Layer);

            // Draw sprite
            Graphics.SpriteBatch.Draw(Texture, Parent.Transform.Position + Offset, null, Color.White * Opacity,
                        MathHelper.ToRadians(Parent.Transform.Rotation), Origin, Parent.Transform.Scale, FlipMode, layerDepth);

            // Draw color mask
            if (ColorMask.A > 0.01f)
            {
                // Create new mask texture if none exists
                if (colorMaskTexture == null)
                    colorMaskTexture = new Texture2D(Graphics.Device, Texture.Width, Texture.Height);

                // Set to current texture
                Color[] maskData = new Color[colorMaskTexture.Width * colorMaskTexture.Height];
                Texture.GetData(maskData);
                colorMaskTexture.SetData(maskData);

                // Add color to all pixels
                colorMaskTexture.GetData(maskData);

                for (int i = 0; i < maskData.Length; i++)
                {
                    if (maskData[i].A > 0f)
                        maskData[i] = ColorMask;
                }

                // Apply new changes
                colorMaskTexture.SetData(maskData);

                // Draw the mask texture
                Graphics.SpriteBatch.Draw(colorMaskTexture, Parent.Transform.Position + Offset, null, ColorMask,
                    MathHelper.ToRadians(Parent.Transform.Rotation), Origin, Parent.Transform.Scale, FlipMode, layerDepth + 0.0001f);
            }
        }

        public static float ConvertToLayerDepth(int layer)
        {
            return layer / (2f * 999999) + 0.5f;
        }
    }
}