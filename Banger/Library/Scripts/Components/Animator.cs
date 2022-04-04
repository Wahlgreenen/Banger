using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JuicyChicken
{
    public class Animator : Component
    {
        private Dictionary<string, Animation> animations = new Dictionary<string, Animation>();
        private Animation currentAnimation;
        private bool isPlaying;
        private float timeSinceLastFrame;

        public override void Update()
        {
            if (!isPlaying)
                return;

            // Exit method if array does not contain anything
            if (currentAnimation.Frames.Length == 0)
                return;

            // Set sprite index based on time since last frame
            timeSinceLastFrame += Time.DeltaTime;
            int currentIndex = (int)(timeSinceLastFrame * currentAnimation.FramesPerSecond);

            // Reset index when sprite index has reached length of the array
            if (currentIndex > currentAnimation.Frames.Length - 1)
            {
                if (currentAnimation.Loop)
                {
                    timeSinceLastFrame = 0f;
                    currentIndex = 0;
                }
                else
                {
                    timeSinceLastFrame = 0f;
                    currentIndex = currentAnimation.Frames.Length - 1;
                    isPlaying = false;
                }
            }

            // Assign current texture
            Parent.Sprite.Texture = currentAnimation.Frames[currentIndex];
        }

        public void AddAnimation(string name, Animation animation)
        {
            if (!animations.ContainsKey(name))
                animations.Add(name, animation);
        }

        public void Play(string name)
        {
            if (animations.TryGetValue(name, out currentAnimation))
            {
                Parent.Sprite.Texture = currentAnimation.Frames[0];
                isPlaying = true;
            }
        }

        public void Play(Animation animation)
        {
            currentAnimation = animation;
            Parent.Sprite.Texture = currentAnimation.Frames[0];
            isPlaying = true;
        }

        public void Stop()
        {
            Parent.Sprite.Texture = currentAnimation.Frames[0];
            isPlaying = false;
        }
    }

    public struct Animation
    {
        private Texture2D[] frames;
        private int framesPerSecond;
        private bool loop;

        public Texture2D[] Frames => frames;
        public int FramesPerSecond => framesPerSecond;
        public bool Loop => loop;

        public Animation(Texture2D spriteSheet, int frameCountX, int frameCountY, int cellWidth, int cellHeight, int framesPerSecond, bool loop)
        {
            frames = new Texture2D[0];
            this.framesPerSecond = framesPerSecond;
            this.loop = loop;

            List<Texture2D> temp = new List<Texture2D>();

            for (int y = 0; y < spriteSheet.Height / cellHeight; y++)
            {
                for (int x = 0; x < spriteSheet.Width / cellWidth; x++)
                {
                    Texture2D frame = spriteSheet.Slice(x, y, cellWidth, cellHeight);
                    if (!EmptyFrame(frame))
                        temp.Add(frame);
                }
            }

            frames = new Texture2D[temp.Count];
            for (int i = 0; i < temp.Count; i++)
            {
                frames[i] = temp[i];
            }
        }

        public Animation(int framesPerSecond, bool loop, params Texture2D[] textures)
        {
            frames = textures;
            this.framesPerSecond = framesPerSecond;
            this.loop = loop;
        }

        private bool EmptyFrame(Texture2D frame)
        {
            Color[] pixels = new Color[frame.Width * frame.Height];
            frame.GetData(pixels);

            for (int i = 0; i < pixels.Length; i++)
            {
                if (pixels[i].A != 0)
                    return false;
            }

            return true;
        }
    }
}
