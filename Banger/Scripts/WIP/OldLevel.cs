using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using JuicyChicken;
using System.Diagnostics;
using System.Linq;

namespace Banger
{

    public class OldLevel : GameObject
    {
        private Texture2D background;
        private Player player;

        private Vector2 selected;
        private Vector2 horizontal;
        private Vector2 vertical;
        private Vector2 diagonal;

        public event Action OnMapChange;

        protected override void Create()
        {
            background = Content.GetTexture("background");
            
            player = GameObject.Find<Player>();
            player.OnMove += TileController;

            selected = Vector2.Zero;
            horizontal = Vector2.UnitX * background.Width;
            vertical = Vector2.UnitY * background.Height;
            diagonal = Vector2.One * background.Width;
        }

        protected override void Draw(Space space)
        {
            if (space == Space.World)
            {
                UI.DrawTexture(background, Color.White, selected, 0f, Vector2.One, background.GetPoint(TexturePoint.Center), 0.1f);
                UI.DrawTexture(background, Color.Salmon, horizontal, 0f, Vector2.One, background.GetPoint(TexturePoint.Center), 0.1f);
                UI.DrawTexture(background, Color.Salmon, vertical, 0f, Vector2.One, background.GetPoint(TexturePoint.Center), 0.1f);
                UI.DrawTexture(background, Color.Salmon, diagonal, 0f, Vector2.One, background.GetPoint(TexturePoint.Center), 0.1f);
            }
        }

        public void TileController()
        {
            Vector2 playerPos = player.Transform.Position;

            // Set horizontal tile position
            if (playerPos.X > selected.X)
                horizontal = selected + Vector2.UnitX * background.Width;
            else if (playerPos.X < selected.X)
                horizontal = selected - Vector2.UnitX * background.Width;


            // Set vertical tile position
            if (playerPos.Y > selected.Y)
                vertical = selected + Vector2.UnitY * background.Height;
            else if (playerPos.Y < selected.Y)
                vertical = selected - Vector2.UnitY * background.Height;

            // Set diagonal tile position
            if (playerPos.X > selected.X && player.Transform.Position.Y > selected.Y)
                diagonal = selected + background.GetSize();
            else if (playerPos.X > selected.X && playerPos.Y < selected.Y)
                diagonal = selected + new Vector2(background.Width, -background.Height);
            else if (playerPos.X < selected.X && playerPos.Y > selected.Y)
                diagonal = selected - new Vector2(background.Width, -background.Height);
            else if (playerPos.X < selected.X && playerPos.Y < selected.Y)
                diagonal = selected - new Vector2(background.Width, background.Height);
          

            //// Check to see if home tile should be changed
            //if (Vector2.Distance(playerPos, selected) < background.Width / 2)
            //    return;

            if (Vector2.Distance(playerPos, selected) > Vector2.Distance(playerPos, horizontal))
            {
                selected = selected + horizontal;
                horizontal = selected - horizontal;
                selected = selected - horizontal;
            }
            else if (Vector2.Distance(playerPos, selected) > Vector2.Distance(playerPos, vertical))
            {
                selected = selected + vertical;
                vertical = selected - vertical;
                selected = selected - vertical;
            }
        }
    }
}
