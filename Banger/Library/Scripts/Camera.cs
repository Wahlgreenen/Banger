using System;
using Microsoft.Xna.Framework;

namespace JuicyChicken
{
    public static class Camera
    {
        private static float zoom = 1f;
        private static Vector2 currentPosition;
        private static Vector2 shakePosition;
        private static float shakeAmplitude;
        private static float shakeSpeed;
        private static Random random = new Random();

        public static bool Lerp { get; set; }
        public static Matrix Matrix { get; private set; }
        public static Vector2 Target { get; set; } = Vector2.Zero;
        public static float Zoom
        {
            get => zoom;

            set
            {
                zoom = value * ((Graphics.AspectRatio.X + Graphics.AspectRatio.Y) / 2);
            }
        }

        static Camera()
        {
            GameLoop.OnUpdate += Update;
        }

        private static void Update()
        {
            shakePosition = new Vector2(random.Next(-shakeAmplitude, shakeAmplitude), random.Next(-shakeAmplitude, shakeAmplitude));
            shakePosition = Vector2.Lerp(shakePosition, Vector2.Zero, 50f * Time.DeltaTime);
            shakeAmplitude = MathHelper.Lerp(shakeAmplitude, 0f, shakeSpeed * Time.DeltaTime);

            if (Lerp)
                currentPosition = Vector2.Lerp(currentPosition, Target + shakePosition, 10f * Time.DeltaTime);
            else
                currentPosition = Target;
            
            float widthOffset = Graphics.CurrentResolution.X / 2;
            float heightOffset = Graphics.CurrentResolution.Y / 2;

            Matrix = Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) * Matrix.CreateTranslation(new Vector3((-currentPosition.X * Zoom) + widthOffset, (-currentPosition.Y * Zoom) + heightOffset, 0f));
        }

        /// <summary>
        /// Shakes the Camera
        /// </summary>
        /// <param name="amplitude">Amplitude of the shake</param>
        /// <param name="speed">Duration of the shake</param>
        public static void Shake(float amplitude, float speed)
        {
            shakeAmplitude = amplitude;
            shakeSpeed = speed;
        }

        /// <summary>
        /// Returns a inversed matrix
        /// </summary>
        public static Vector2 ScreenToWorld(Vector2 pos)
        {
            Matrix inverseMatrix = Matrix.Invert(Matrix);
            return Vector2.Transform(pos, inverseMatrix);
        }
    }
}
