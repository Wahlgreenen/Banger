using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace JuicyChicken
{
    public static class Audio
    {
        private static Random random = new Random();

        private static Dictionary<string, SoundEffectInstance> singleSoundKeys = new Dictionary<string, SoundEffectInstance>();


        /// <summary>
        /// Plays a sound effect with specified values
        /// </summary>
        /// <param name="key">Key for accessing the sound effect instance</param>
        /// <param name="loop">Should the sound effect loop after playback?</param>
        /// /// <param name="volume">Loudness of the sound effect playback</param>
        /// <param name="pitchRange">Sets sound pitch to a random value between X and Y</param>
        public static SoundEffectInstance Play(string key, float volume = 1f, Vector2 pitchRange = default, bool loop = false, bool allowMultiple = true)
        {

            SoundEffectInstance value;


            // ensure the sound only has 1 instance going
            if (!allowMultiple)
            {
                SoundEffectInstance oldVal;

                // Checks dictionary to see if current sound exists
                if (singleSoundKeys.TryGetValue(key, out oldVal))
                {
                    // checks if sound is playing or not
                    if (oldVal.State != SoundState.Playing)
                    {
                        singleSoundKeys.Remove(key);

                        value = Content.GetSound(key).CreateInstance();
                        singleSoundKeys.Add(key, value);
                    }
                    else
                    {
                        return oldVal;
                    }

                }
                else
                {
                    // Add new sound to dictionary
                    value = Content.GetSound(key).CreateInstance();
                    singleSoundKeys.Add(key, value);
                }
            }
            else
            {
                value = Content.GetSound(key).CreateInstance();
            }


            if (value != null)
            {
                // Set parameters
                value.Volume = volume;
                value.Pitch = pitchRange == default ? 0f : random.Next(pitchRange.X, pitchRange.Y);
                value.IsLooped = loop;

                // Play sound
                value.Play();
            }
            


            return value;
        }

        /// <summary>
        /// Stops a sound effect
        /// </summary>
        public static void Stop(SoundEffectInstance value)
        {
            if (value.State == SoundState.Playing)
                value.Stop();
        }
    }
}
