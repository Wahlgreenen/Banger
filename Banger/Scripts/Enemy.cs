using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using JuicyChicken;
using JuicyChicken.Input;

namespace Banger
{
    class Enemy : GameObject
    {
        private Player player;
        private float speed = 1f;

        protected override void Create()
        {
            Sprite.Texture = Content.GetTexture("enemy");
            player = GameObject.Find<Player>();
            Transform.Position = Vector2.Zero;
        }

        protected override void Update()
        {
            Vector2 direction = Transform.Position - player.Transform.Position;
            direction.Normalize();
            Transform.Translate(direction * speed * Time.DeltaTime);
        }
    }
}
