using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JuicyChicken
{
    public static class UI
    {
        public enum AlignmentType { LeftTop, LeftMid, LeftBottom, CenterTop, CenterMid, CenterBottom, RightTop, RightMid, RightBottom }

        private static Dictionary<string, UIGroup[]> panels = new Dictionary<string, UIGroup[]>();
        private static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();
        private static Dictionary<string, Color[]> palettes = new Dictionary<string, Color[]>();
        private static UIGroup[] currentPanel;

        public static void AddPanel(string key, params UIGroup[] groups)
        {
            if (!panels.ContainsKey(key))
            {
                panels.Add(key, groups);
            }
        }

        public static void SetCurrentPanel(string key)
        {
            if (panels.ContainsKey(key))
            {
                foreach (UIGroup group in currentPanel)
                    group.Enabled = false;
             
                currentPanel = panels[key];

                foreach (UIGroup group in currentPanel)
                    group.Enabled = true;
            }
        }

        public static void AddFont(string key, SpriteFont font)
        {
            if (!fonts.ContainsKey(key))
            {
                fonts.Add(key, font);
            }
        }

        public  static SpriteFont GetFont(string key)
        {
            if (fonts.ContainsKey(key))
            {
                return fonts[key];
            }
            else
            {
                return null;
            }
        }
        public static void AddPalette(string key, Texture2D texture)
        {
            if (!palettes.ContainsKey(key))
            {
                Color[] palette = new Color[texture.Width * texture.Height];
                texture.GetData(palette);
                palettes.Add(key, palette);
            }
        }

        public  static Color GetColor(Color color, string key)
        {
            if (palettes.TryGetValue(key, out Color[] palette))
            {
                return FindNearestColor(palette, color);
            }
            return Color.White;
        }

        public static Color GetColor(int index, string key)
        {
            if (palettes.TryGetValue(key, out Color[] palette))
            {
                return palette[index];
            }
            return Color.White;
        }
        private static int GetColorDistance(Color current, Color match)
        {
            int redDifference;
            int greenDifference;
            int blueDifference;

            redDifference = current.R - match.R;
            greenDifference = current.G - match.G;
            blueDifference = current.B - match.B;

            return redDifference * redDifference + greenDifference * greenDifference + blueDifference * blueDifference;
        }

        private static Color[] ConvertColorArray(Color[,] data)
        {
            int width = data.GetLength(0);
            int height = data.GetLength(1);
            Color[] colorData = new Color[width * height];
            int i = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    colorData[i++] = data[x, y];
                }
            }
            return colorData;
        }

        private static Color FindNearestColor(Color[] map, Color current)
        {
            int shortestDistance;
            int index;

            index = -1;
            shortestDistance = int.MaxValue;

            for (int i = 0; i < map.Length; i++)
            {
                Color match;
                int distance;

                match = map[i];
                distance = GetColorDistance(current, match);

                if (distance < shortestDistance)
                {
                    index = i;
                    shortestDistance = distance;
                }
            }

            return map[index];
        }

        public static Texture2D CreateLine(Vector2 pos1, Vector2 pos2, int lineWidth, Color color)
        {
            int width = Math.Abs((int)pos2.X + (int)pos1.X);
            int height = Math.Abs((int)pos2.Y + (int)pos1.Y);

            Texture2D box = new Texture2D(Graphics.Device, width, height);
            Color[,] pixels = new Color[width, height];


            List<Vector2> line = GetLine(pos1, pos2);
            for (int i = 0; i < line.Count; i++)
            {
                pixels[(int)line[i].X, (int)line[i].Y] = color;
            }
            box.SetData(ConvertColorArray(pixels));
            return box;
        }


        public static Texture2D CreateBox(int width, int height, int outlineWidth, Color color, Color outlineColor)
        {
            Texture2D box = new Texture2D(Graphics.Device, width, height);
            Color[,] pixels = new Color[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x >= outlineWidth && x < width - outlineWidth && y >= outlineWidth && y < height - outlineWidth)
                    {
                        pixels[x, y] = color;
                    }
                    else
                    {
                        pixels[x, y] = outlineColor;
                    }
                }
            }
            box.SetData(ConvertColorArray(pixels));
            return box;
        }

        public static Texture2D CreateBox(int width, int height, int outlineWidth, int cornerRadius, Color color, Color outlineColor)
        {
            Texture2D box = new Texture2D(Graphics.Device, width, height);
            Color[,] pixels = new Color[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x >= outlineWidth && x < width - outlineWidth && y >= outlineWidth && y < height - outlineWidth)
                    {
                        pixels[x, y] = color;
                    }
                    else
                    {
                        pixels[x, y] = outlineColor;
                    }
                }
            }
            int diagonal = cornerRadius * 2 - 1;
            RoundCorner(ref pixels, outlineWidth, color, outlineColor, cornerRadius, 0,  diagonal, 0, diagonal, 1 );
            RoundCorner(ref pixels, outlineWidth, color, outlineColor, cornerRadius, width - diagonal, width, 0, diagonal, 2);
            RoundCorner(ref pixels, outlineWidth, color, outlineColor, cornerRadius, 0, diagonal, height - diagonal, height, 3);
            RoundCorner(ref pixels, outlineWidth, color, outlineColor, cornerRadius, width - diagonal, width, height - diagonal, height, 4);

            box.SetData(ConvertColorArray(pixels));
            return box;
        }

        private static void RoundCorner(ref Color[,] pixels, int outlineWidth, Color color, Color outlineColor, int roundAmount, int startX, int endX, int startY, int endY, int cornerIndex)
        {
            cornerIndex = Math.Clamp(cornerIndex, 1, 4);
            Vector2 center = new Vector2((startX + endX) / 2, (startY + endY) / 2);
            for (int y = startY; y < endY; y++)
            {
                for (int x = startX; x < endX; x++)
                {
                    switch (cornerIndex)
                    {
                        case 1:
                            if (x > center.X || y > center.Y)
                            {
                                continue;
                            }
                            break;
                        case 2:
                            if (x < center.X || y > center.Y)
                            {
                                continue;
                            }
                            break;
                        case 3:
                            if (x > center.X || y < center.Y)
                            {
                                continue;
                            }
                            break;
                        case 4:
                            if (x < center.X || y < center.Y)
                            {
                                continue;
                            }
                            break;
                    }
                    Vector2 current = new Vector2(x, y);

                    if (Vector2.Distance(current, center) > roundAmount)
                    {
                        pixels[x, y] = Color.Black * 0f;
                    }

                    if (Vector2.Distance(current, center) < roundAmount - outlineWidth)
                    {
                       pixels[x, y] = color;
                    }
                    else if (Vector2.Distance(current, center) < roundAmount)
                    {
                       pixels[x, y] = outlineColor;
                    }
                }
            }
        }

        public static Texture2D CreateCircle(int radius, int outlineWidth, Color color, Color outlineColor)
        {
            int diagonal = radius * 2;
            Texture2D box = new Texture2D(Graphics.Device, diagonal, diagonal);
            Color[,] pixels = new Color[diagonal, diagonal];
            Vector2 center = new Vector2(diagonal / 2, diagonal / 2);
            for (int y = 0; y < diagonal; y++)
            {
                for (int x = 0; x < diagonal; x++)
                {
                    Vector2 current = new Vector2(x, y);
                    if (Vector2.Distance(current, center) <= radius - outlineWidth)
                    {
                        pixels[x, y] = color;
                    }
                    else if (Vector2.Distance(current, center) <= radius)
                    {
                        pixels[x, y] = outlineColor;
                    }
                }
            }
            box.SetData(ConvertColorArray(pixels));
            return box;
        }

        /// <summary>
        /// A custom overload of spritebatch.draw to remove unused parameters. Uses Vector2 for scale
        /// </summary>
        public static void DrawTexture(Texture2D texture, Color color, Vector2 position, float rotation, Vector2 scale, Vector2 origin, float layerDepth)
        {
            Graphics.SpriteBatch.Draw(texture, position * Graphics.AspectRatio, null, color, MathHelper.ToRadians(rotation), origin, scale * Graphics.AspectRatio, SpriteEffects.None, layerDepth);
        }

        /// <summary>
        /// A custom overload of spritebatch.drawstring to remove unused parameters.
        /// 
        /// </summary>
        /// <param name="alignment"></param> sets the origin of the string to the specified area
        public static void DrawText(string text, string font, AlignmentType alignment, Vector2 position, Color color, Color outlineColor, float rotation, Vector2 scale, float layerDepth)
        {
            Vector2 origin = Vector2.Zero;
            Vector2 textBounds = GetFont(font).MeasureString(text);
            switch (alignment)
            {
                case AlignmentType.LeftTop:
                    origin = Vector2.Zero;
                    break;
                case AlignmentType.LeftMid:
                    origin = new Vector2(0, textBounds.Y / 2);
                    break;
                case AlignmentType.LeftBottom:
                    origin = new Vector2(0, textBounds.Y);
                    break;
                case AlignmentType.CenterTop:
                    origin = new Vector2(textBounds.X / 2, 0);
                    break;
                case AlignmentType.CenterMid:
                    origin = textBounds / 2;
                    break;
                case AlignmentType.CenterBottom:
                    origin = new Vector2(textBounds.X / 2, textBounds.Y);
                    break;
                case AlignmentType.RightTop:
                    origin = new Vector2(textBounds.X, 0);
                    break;
                case AlignmentType.RightMid:
                    origin = new Vector2(textBounds.X, textBounds.Y / 2);
                    break;
                case AlignmentType.RightBottom:
                    origin = textBounds;
                    break;
            }

            Graphics.SpriteBatch.DrawString(GetFont(font), text, position * Graphics.AspectRatio, color, MathHelper.ToRadians(rotation), origin, scale * Graphics.AspectRatio, SpriteEffects.None, layerDepth);
            Graphics.SpriteBatch.DrawString(GetFont(font), text, (position + Vector2.One * scale) * Graphics.AspectRatio, outlineColor, MathHelper.ToRadians(rotation), origin, scale * Graphics.AspectRatio, SpriteEffects.None, layerDepth - 0.05f);
        }
        private static List<Vector2> GetLine(Vector2 from, Vector2 to)
        {
            List<Vector2> line = new List<Vector2>();

            int x = (int)from.X;
            int y = (int)from.Y;

            int distanceX = (int)to.X - (int)from.X;
            int distanceY = (int)to.Y - (int)from.Y;

            bool inverted = false;
            int step = Math.Sign(distanceX);
            int gradientStep = Math.Sign(distanceY);

            int longest = Math.Abs(distanceX);
            int shortest = Math.Abs(distanceY);

            if (longest < shortest)
            {
                inverted = true;
                longest = Math.Abs(distanceY);
                shortest = Math.Abs(distanceX);

                step = Math.Sign(distanceY);
                gradientStep = Math.Sign(distanceX);
            }

            int gradientAccumulation = longest / 2;
            for (int i = 0; i < longest; i++)
            {
                line.Add(new Vector2(x, y));

                if (inverted)
                {
                    y += step;
                }
                else
                {
                    x += step;
                }

                gradientAccumulation += shortest;
                if (gradientAccumulation >= longest)
                {
                    if (inverted)
                    {
                        x += gradientStep;
                    }
                    else
                    {
                        y += gradientStep;
                    }

                    gradientAccumulation -= longest;
                }
            }

            return line;
        }
    }
}
