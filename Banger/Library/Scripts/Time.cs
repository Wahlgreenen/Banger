using Microsoft.Xna.Framework;


namespace JuicyChicken
{
    public static class Time
    {
        /// <summary>
        /// Time since the last frame
        /// </summary>
        public static float DeltaTime { get; private set; }

        /// <summary>
        /// Total time since the game started
        /// </summary>
        public static float TotalTime { get; private set; }

        public static void Update(GameTime gameTime)
        {
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TotalTime = (float)gameTime.TotalGameTime.TotalSeconds;
        }
    }
}
