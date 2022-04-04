using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JuicyChicken
{
    public enum Space { World, Screen }

    public static class GameLoop
    {
        public static event Action OnUpdate;
        public static event Action<Space> OnDraw;

        public static void InvokeUpdate(GameTime gameTime)
        {
            Time.Update(gameTime);
            OnUpdate?.Invoke();
        }

        public static void InvokeDraw(BlendState blendState, SamplerState samplerState)
        {
            Graphics.Device.Clear(Color.Black);

            Graphics.SpriteBatch.Begin(SpriteSortMode.FrontToBack, blendState, samplerState, null, null, null, Camera.Matrix);
            OnDraw?.Invoke(Space.World);
            Graphics.SpriteBatch.End();

            Graphics.SpriteBatch.Begin(SpriteSortMode.FrontToBack, blendState, samplerState);
            OnDraw?.Invoke(Space.Screen);
            Graphics.SpriteBatch.End();
        }
    }
}
