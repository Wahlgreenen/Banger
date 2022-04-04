using JuicyChicken;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Collections.Generic;
using JuicyChicken.Input;

namespace Banger
{
    public class GameWorld : Game
    {
        private InputAction debugToggle = new InputAction(Keys.Tab);
        public static bool DebugEnabled { get; private set; }


        public GameWorld()
        {
            // Initialize graphics with a default resolution
            Graphics.Initialize(this, 1280, 720);
            IsFixedTimeStep = false;
            IsMouseVisible = false;
        }

        protected override void LoadContent()
        {
            // Setup directories and load content
            string mainPath = Directory.GetCurrentDirectory();
            string textureDirectory = Path.Combine(mainPath, "Content", "Textures");
            string audioDirectory = Path.Combine(mainPath, "Content", "Sounds");
            string fontDirectory = Path.Combine(mainPath, "Content", "Fonts");
            JuicyChicken.Content.Load(Content, textureDirectory, audioDirectory, fontDirectory);

            Camera.Zoom = 1.9f;

            //Instantiate player
            Player player = GameObject.Instantiate<Player>();
            Animator animator = player.AddComponent<Animator>();
            animator.AddAnimation("playeranimation", new Animation(JuicyChicken.Content.GetTexture("player-sheet"), 2, 1, 17, 18, 1, true));
            //animator.Play("playeranimation");

            Enemy enemy = GameObject.Instantiate<Enemy>();


            Level ground = GameObject.Instantiate<Level>();


            InputAction zoomOut = new InputAction(Keys.Q, 0.01f);
            zoomOut.OnHoldEvent += () => Camera.Zoom -= 0.03f;

            InputAction zoomIn = new InputAction(Keys.E, 0.01f);
            zoomIn.OnHoldEvent += () => Camera.Zoom += 0.03f;

            InputAction resetZoom = new InputAction(Keys.F);
            resetZoom.OnDownEvent += () => Camera.Zoom = 1.9f;

            debugToggle.OnDownEvent += () =>
            {
                DebugEnabled = !DebugEnabled;
                player.GetComponent<BoxCollider>().DrawDebug = DebugEnabled;
            };
        }

        protected override void Update(GameTime gameTime)
        {
            // Invoke all objects subscribed to the game loop
            GameLoop.InvokeUpdate(gameTime);
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            // Invoke all objects subscribed to the game loop
            GameLoop.InvokeDraw(BlendState.AlphaBlend, SamplerState.PointClamp);
            base.Draw(gameTime);
        }
    }
}
