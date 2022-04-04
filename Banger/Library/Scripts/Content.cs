using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace JuicyChicken
{
    public static class Content
    {
        private static readonly Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        private static readonly Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();
        private static readonly Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();
        private static ContentManager contentManager;

        public static event Action OnContentLoaded;

        private static Texture2D missingTexture;

        /// <summary>
        /// Loads, creates and stores all content from a specified directory path
        /// </summary>
        public static void Load(ContentManager contentManager, string textureDirectory, string soundDirectory, string fontDirectory)
        {
            textures.Clear();
            sounds.Clear();
            fonts.Clear();

            Content.contentManager = contentManager;


            //Create missing texture, texture
            missingTexture = new Texture2D(Graphics.Device, 32, 32);

            Color[] color = new Color[missingTexture.Width * missingTexture.Height];
            for (int i = 0; i < color.Length; i++)
            {
                if (i % 2 == 0)
                {
                    color[i] = Color.Magenta;
                }
                else
                {
                    color[i] = Color.Black;
                }
            }

            missingTexture.SetData(color);

            if (!Directory.Exists(textureDirectory))
                Directory.CreateDirectory(textureDirectory);

            if (!Directory.Exists(soundDirectory))
                Directory.CreateDirectory(soundDirectory);

            if (!Directory.Exists(fontDirectory))
                Directory.CreateDirectory(fontDirectory);

            ScanTextures(textureDirectory);
            ScanSounds(soundDirectory);
            ScanFonts(fontDirectory);

            OnContentLoaded?.Invoke();
        }

        private static void ScanTextures(string textureDirectory)
        {
            // Scan texture directory
            foreach (string file in Directory.GetFiles(textureDirectory))
            {
                string fileName = Path.GetFileNameWithoutExtension(file).ToLower();

                Texture2D texture = CreateTexture(file);
                if (texture != null)
                {
                    textures.Add(fileName, texture);
                }
            }

            // Scan all sub-directories in the texture directory
            foreach (string directory in Directory.GetDirectories(textureDirectory, "*", SearchOption.AllDirectories))
            {
                foreach (string file in Directory.GetFiles(directory))
                {
                    string fileName = Path.GetFileNameWithoutExtension(file).ToLower();

                    Texture2D texture = CreateTexture(file);
                    if (texture != null)
                    {
                        textures.Add(fileName, texture);
                    }
                }
            }
        }

        private static void ScanSounds(string soundDirectory)
        {
            // Scan sound directory
            foreach (string file in Directory.GetFiles(soundDirectory))
            {
                string fileName = Path.GetFileNameWithoutExtension(file).ToLower();

                SoundEffect sound = CreateSound(file);
                if (sound != null)
                {
                    sounds.Add(fileName, sound);
                }
            }

            // Scan all sub-directories in the sound directory
            foreach (string directory in Directory.GetDirectories(soundDirectory, "*", SearchOption.AllDirectories))
            {
                foreach (string file in Directory.GetFiles(directory))
                {
                    string fileName = Path.GetFileNameWithoutExtension(file).ToLower();

                    SoundEffect sound = CreateSound(file);
                    if (sound != null)
                    {
                        sounds.Add(fileName, sound);
                    }
                }
            }
        }

        private static void ScanFonts(string fontDirectory)
        {
            // Scan font directory
            foreach (string file in Directory.GetFiles(fontDirectory))
            {
                string fileName = Path.GetFileNameWithoutExtension(file).ToLower();

                SpriteFont font = CreateFont(file);
                if (font != null)
                {
                    fonts.Add(fileName, font);
                }
            }

            // Scan all sub-directories in the font directory
            foreach (string directory in Directory.GetDirectories(fontDirectory, "*", SearchOption.AllDirectories))
            {
                foreach (string file in Directory.GetFiles(directory))
                {
                    string fileName = Path.GetFileNameWithoutExtension(file).ToLower();

                    SpriteFont font = CreateFont(file);
                    if (font != null)
                    {
                        fonts.Add(fileName, font);
                    }
                }
            }
        }

        /// <summary>
        /// Creates a new Texture2D from a specified file path
        /// </summary>
        /// <returns>A Texture2D with the texture</returns>
        private static Texture2D CreateTexture(string path)
        {
            Texture2D texture = null;

            string directoryName = Path.GetDirectoryName(path);
            string fileName = Path.GetFileNameWithoutExtension(path);
            string combinedPath = Path.Combine(directoryName, fileName);
            string extension = Path.GetExtension(path).ToLower();

            switch (extension)
            {
                case ".xnb":
                    {
                        texture = contentManager.Load<Texture2D>(combinedPath);
                    }
                    break;

                case ".png":
                case ".jpg":
                case ".bmp":
                case ".gif":
                case ".tif":
                case ".dds":
                    {
                        // Load the texture
                        FileStream fileStream = new FileStream(path, FileMode.Open);
                        texture = Texture2D.FromStream(Graphics.Device, fileStream);
                        fileStream.Dispose();

                        // Add AlphaBlend support
                        Color[] data = new Color[texture.Width * texture.Height];
                        texture.GetData(data);

                        for (int i = 0; i < data.Length; i++)
                            data[i] = Color.FromNonPremultiplied(data[i].R, data[i].G, data[i].B, data[i].A);

                        texture.SetData(data);
                    }
                    break;
            }

            return texture;
        }

        /// <summary>
        /// Creates a new SoundEffectInstance from a specified file path
        /// </summary>
        /// <returns>A SoundEffectInstance with the audio file</returns>
        private static SoundEffect CreateSound(string path)
        {
            SoundEffect soundEffect = null;

            string directoryName = Path.GetDirectoryName(path);
            string fileName = Path.GetFileNameWithoutExtension(path);
            string combinedPath = Path.Combine(directoryName, fileName);
            string extension = Path.GetExtension(path).ToLower();

            switch (extension)
            {
                case ".xnb":
                    {
                        soundEffect = contentManager.Load<SoundEffect>(combinedPath);
                    }
                    break;

                case ".wav":
                    {
                        FileStream fileStream = new FileStream(path, FileMode.Open);
                        SoundEffect sound = SoundEffect.FromStream(fileStream);
                        soundEffect = sound;
                        fileStream.Dispose();
                    }
                    break;
            }

            return soundEffect;
        }

        /// <summary>
        /// Creates a new SpriteFont instance using MonoGame's built-in ContentManager system
        /// </summary>
        /// <returns></returns>
        private static SpriteFont CreateFont(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            string fileName = Path.GetFileNameWithoutExtension(path);
            string combinedPath = Path.Combine(directoryName, fileName);

            SpriteFont font = contentManager.Load<SpriteFont>(combinedPath);
            return font;
        }

        /// <summary>
        /// Tries to get a texture from the dictionary with specified key
        /// </summary>
        /// <returns>A Texture2D from the texture dictionary</returns>
        public static Texture2D GetTexture(string key)
        {
            if (textures.TryGetValue(key.ToLower(), out Texture2D value))
            {
                return value;
            }
            else
            {
                //throw new Exception($"Could not find a texture with the specified key: '{key}'");

                return missingTexture;
            }
        }

        /// <summary>
        /// Tries to get a sound instance from the dictionary with specified key
        /// </summary>
        /// <returns>A SoundEffectInstance from the sound instance dictionary</returns>
        public static SoundEffect GetSound(string key)
        {
            if (sounds.TryGetValue(key.ToLower(), out SoundEffect value))
            {
                return value;
            }
            else
            {
                throw new Exception($"Could not find a sound instance with the specified key: '{key}'");
            }
        }

        /// <summary>
        /// Tries to get a SpriteFont from the dictionary with specified key
        /// </summary>
        /// <returns>A SpriteFont from the font dictionary</returns>
        public static SpriteFont GetFont(string key)
        {
            if (fonts.TryGetValue(key.ToLower(), out SpriteFont value))
            {
                return value;
            }
            else
            {
                throw new Exception($"Could not find a font with the specified key: '{key}'");
            }
        }
    }
}
