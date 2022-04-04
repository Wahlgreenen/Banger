using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JuicyChicken
{
    public static class Graphics
    {
        public static GraphicsDeviceManager Manager { get; private set; }
        public static GraphicsDevice Device => Manager.GraphicsDevice;
        public static SpriteBatch SpriteBatch { get; private set; }

        public static Vector2 DeviceResolution { get; private set; }
        public static Vector2 ReferenceResolution { get; set; }
        public static Vector2 CurrentResolution { get => new Vector2(Manager.PreferredBackBufferWidth, Manager.PreferredBackBufferHeight); }
        public static Vector2 AspectRatio { get => CurrentResolution / ReferenceResolution; }
        
        public static event Action OnResolutionChanged;


        public static void Initialize(Game game, int resolutionWidth, int resolutionHeight)
        {
            // Create Manager & SpriteBatch
            Manager = new GraphicsDeviceManager(game);
            Manager.ApplyChanges();
            SpriteBatch = new SpriteBatch(Device);

            // Set resolutions
            DeviceResolution = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            ReferenceResolution = new Vector2(resolutionWidth, resolutionHeight);
            SetResolution(resolutionWidth, resolutionHeight);
        }

        public static void SetResolution(int width, int height)
        {
            Manager.PreferredBackBufferWidth = width;
            Manager.PreferredBackBufferHeight = height;
            Manager.ApplyChanges();
        }

        public static void ToggleFullscreen()
        {
            if (!Manager.IsFullScreen)
            {
                Manager.PreferredBackBufferWidth = (int)DeviceResolution.X;
                Manager.PreferredBackBufferHeight = (int)DeviceResolution.Y;

                Manager.IsFullScreen = true;
            }
            else
            {
                Manager.PreferredBackBufferWidth = (int)ReferenceResolution.X;
                Manager.PreferredBackBufferHeight = (int)ReferenceResolution.Y;

                Manager.IsFullScreen = false;
            }

            Manager.ApplyChanges();
        }
    }
}
