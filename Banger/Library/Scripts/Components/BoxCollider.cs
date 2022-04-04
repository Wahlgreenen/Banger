using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace JuicyChicken
{
    public class BoxCollider : Collider
    {
        private Rectangle rectangle;
        private Texture2D debugTexture;

        public Vector2 Bounds { get; set; }
        public Vector2 Offset { get; set; }
        public bool DrawDebug { get; set; }

        public override void Update()
        {
            rectangle = new Rectangle((int)Parent.Transform.Position.X + (int)Offset.X, (int)Parent.Transform.Position.Y + (int)Offset.Y, (int)Bounds.X, (int)Bounds.Y);
        }

        public override void Draw(Space space)
        {
            if (!DrawDebug)
                return;

            switch (space)
            {
                case Space.World:
                    if (debugTexture == null)
                    {
                        debugTexture = new Texture2D(Graphics.Device, 1, 1);

                        Color[] data = new Color[1] { Color.White };
                        debugTexture.SetData(data);
                    }

                    // Get collision box from game object
                    Rectangle lineTop = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, 1);
                    Rectangle lineBottom = new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width, 1);
                    Rectangle lineLeft = new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, 1, rectangle.Height);
                    Rectangle lineRight = new Rectangle(rectangle.X, rectangle.Y, 1, rectangle.Height);

                    // Set color according to collision state
                    Color color = IsColliding ? Color.Red : Color.Lime;
                    Graphics.SpriteBatch.Draw(debugTexture, lineTop, null, color, 0, Vector2.Zero, SpriteEffects.None, 1);
                    Graphics.SpriteBatch.Draw(debugTexture, lineBottom, null, color, 0, Vector2.Zero, SpriteEffects.None, 1);
                    Graphics.SpriteBatch.Draw(debugTexture, lineLeft, null, color, 0, Vector2.Zero, SpriteEffects.None, 1);
                    Graphics.SpriteBatch.Draw(debugTexture, lineRight, null, color, 0, Vector2.Zero, SpriteEffects.None, 1);
                    break;
            }
        }

        public override bool Intersects(Collider other)
        {
            if (!(other is BoxCollider boxCollider))
                return false;

            return rectangle.Intersects(boxCollider.rectangle);
        }
    }
}
