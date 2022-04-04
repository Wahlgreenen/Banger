using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JuicyChicken
{
    public enum TexturePoint { Center, Top, Bottom, Left, Right }

    public static class Extensions
    {
        /// <summary>
        /// Extension method for getting a random float by adding minValue and maxValue to Random.NextDouble
        /// </summary>
        /// <returns>A random float between minValue and maxValue</returns>
        public static float Next(this Random random, float minValue, float maxValue)
        {
            double value = (random.NextDouble() * (maxValue - minValue) + minValue);
            return (float)value;
        }

        /// <summary>
        /// Extension method for getting a specified point of a texture
        /// </summary>
        /// <returns>The specified point of the texture</returns>
        public static Vector2 GetPoint(this Texture2D texture, TexturePoint point)
        {
            if (texture == null)
                return Vector2.Zero;

            switch (point)
            {
                default:
                case TexturePoint.Center:
                    return new Vector2(texture.Width / 2, texture.Height / 2);

                case TexturePoint.Top:
                    return new Vector2(texture.Width / 2, 0);

                case TexturePoint.Bottom:
                    return new Vector2(texture.Width / 2, texture.Height);

                case TexturePoint.Left:
                    return new Vector2(0, texture.Height / 2);

                case TexturePoint.Right:
                    return new Vector2(texture.Width, texture.Height / 2);
            }
        }

        /// <summary>
        /// Extension method for getting the size of a texture
        /// </summary>
        /// <returns>A Vector2 containing the Width and the Height of the texture</returns>
        public static Vector2 GetSize(this Texture2D texture)
        {
            return new Vector2(texture.Width, texture.Height);
        }

        /// <summary>
        /// Extension method for getting the data of a texture as a two-dimensional array
        /// </summary>
        /// <returns>A two-dimensional array containing the color data</returns>
        public static Color[,] GetData(this Texture2D texture)
        {
            Color[] colorsOne = new Color[texture.Width * texture.Height];
            texture.GetData(colorsOne);

            Color[,] colorsTwo = new Color[texture.Width, texture.Height];
            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                    colorsTwo[x, y] = colorsOne[x + y * texture.Width];

            return colorsTwo;
        }

        /// <summary>
        /// Extension method for setting the data of a texture as a two-dimensional array
        /// </summary>
        public static void SetData(this Texture2D texture, Color[,] data)
        {
            Color[] colorData = new Color[texture.Width * texture.Height];
            int i = 0;

            for (int y = 0; y < data.GetLength(1); y++)
            {
                for (int x = 0; x < data.GetLength(0); x++)
                {
                    colorData[i++] = data[x, y];
                }
            }

            texture.SetData(colorData);
        }

        /// <summary>
        /// Cuts out a certain part of a texture
        /// </summary>
        /// <returns>A sliced Texture</returns>
        public static Texture2D Slice(this Texture2D texture, int xIndex, int yIndex, int width, int height)
        {
            Color[,] data = texture.GetData();
            Texture2D newTexture = new Texture2D(Graphics.Device, width, height);
            Color[,] newData = new Color[width, height];

            int startX = xIndex * width;
            int startY = yIndex * height;

            for (int y = startY; y < startY + height; y++)
            {
                for (int x = startX; x < startX + width; x++)
                {
                    newData[x - startX, y - startY] = data[x, y];
                }
            }

            newTexture.SetData(newData);
            return newTexture;
        }
    }
}
