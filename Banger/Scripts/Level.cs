using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using JuicyChicken;

namespace Banger
{

    public class Level : GameObject
    {
        private List<Vector2> tiles = new List<Vector2>();
        private List<Texture2D> textures = new List<Texture2D>();
        private Texture2D texture;
        private Player player;

        private int selectedIndex;
        private Vector2 selected;

        protected override void Create()
        {
            texture = Content.GetTexture("background");
            player = GameObject.Find<Player>();
            player.OnMove += CalculateTiles;

            tiles.Add(Vector2.Zero);
            tiles.Add(Vector2.UnitX * texture.Width);
            tiles.Add(Vector2.UnitY * texture.Height);
            tiles.Add(Vector2.One * texture.Width);

            textures.Add(Content.GetTexture("background"));
            textures.Add(Content.GetTexture("background1"));
            textures.Add(Content.GetTexture("background2"));
            textures.Add(Content.GetTexture("background3"));
        }

        protected override void Draw(Space space)
        {
            if (space != Space.World)
                return;

            for (int i = 0; i < tiles.Count; i++)
            {
                Color tileColor;
                if (GameWorld.DebugEnabled && selected != tiles[i])
                    tileColor = Color.DarkGray;
                else
                    tileColor = Color.White;

                UI.DrawTexture(textures[i], tileColor, tiles[i], 0f, Vector2.One, texture.GetPoint(TexturePoint.Center), 0.1f);
            }
        }

        private void CalculateTiles()
        {
            Vector2 playerPos = player.Transform.Position;
            for (int i = 0; i < tiles.Count; i++)
            {
                if (Vector2.Distance(playerPos, selected) > Vector2.Distance(playerPos, tiles[i]))
                {
                    selected = tiles[i];
                    selectedIndex = i;
                    break;
                }
            }

            Vector2 offset = playerPos - selected;
            offset.X += offset.X == 0 ? 1 : 0;
            offset.Y += offset.Y == 0 ? 1 : 0;
            offset = new Vector2(Math.Sign(offset.X), Math.Sign(offset.Y)) * texture.GetSize();

            if (selectedIndex == 0)
            {
                tiles[0] = selected;
                tiles[1] = selected + Vector2.UnitX * offset.X;
                tiles[2] = selected + Vector2.UnitY * offset.Y;
                tiles[3] = selected + offset;
            }
            else if (selectedIndex == 1)
            {
                tiles[1] = selected;
                tiles[0] = selected + Vector2.UnitX * offset.X;
                tiles[3] = selected + Vector2.UnitY * offset.Y;
                tiles[2] = selected + offset;
            }

            else if (selectedIndex == 2)
            {
                tiles[2] = selected;
                tiles[3] = selected + Vector2.UnitX * offset.X;
                tiles[0] = selected + Vector2.UnitY * offset.Y;
                tiles[1] = selected + offset;
            }
            else if (selectedIndex == 3)
            {
                tiles[3] = selected;
                tiles[2] = selected + Vector2.UnitX * offset.X;
                tiles[1] = selected + Vector2.UnitY * offset.Y;
                tiles[0] = selected + offset;
            }



            if (selectedIndex % 2 == 0) // selected index 0 or 2 (or in theory higher)
            {
                tiles[selectedIndex] = selected;
                tiles[GetIndex(1)] = selected + Vector2.UnitX * offset.X;
                tiles[GetIndex(2)] = selected + Vector2.UnitY * offset.Y;
                tiles[GetIndex(3)] = selected + offset;
                return;
            }

            if (selectedIndex % 2 == 1) // selected index 1 or 3 (or in theory higher)
            {
                tiles[selectedIndex] = selected;
                tiles[GetIndex(-1)] = selected + Vector2.UnitX * offset.X;
                tiles[GetIndex(-2)] = selected + Vector2.UnitY * offset.Y;
                tiles[GetIndex(-3)] = selected + offset;
            }
        }

        private int GetIndex(int offset)
        {
            int index = selectedIndex;
            int count = 0;


            while (count < Math.Abs(offset))
            {
                if (offset > 0)
                {
                    index++;
                    if (index >= tiles.Count)
                        index = 0;
                }
                else if (offset < 0)
                {
                    index--;
                    if (index < 0)
                        index = tiles.Count - 1;
                }
                count++;
            }
            return index;
        }
    }
}
