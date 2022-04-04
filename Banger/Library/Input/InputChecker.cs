using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using JuicyChicken;

namespace JuicyChicken.Input
{
    public static class InputChecker
    {
        //Used by input checker
        internal static KeyboardState frameState;
        internal static KeyboardState formerState;
        internal static MouseState frameMouseState;
        internal static MouseState formerMouseState;

        //Contains every key to check
        private static Dictionary<Keys, KeyState> keyStates = new Dictionary<Keys, KeyState>();
        private static MouseKeyState mouseKeyState = new MouseKeyState();

        //Global variables
        public static Vector2 CurrentMousePosition { get { return MouseKeyState.MousePosition; } }
        public static event Action<Keys> OnTyping;

        internal static Dictionary<Keys, KeyState> KeyStates { get => keyStates; set => keyStates = value; }
        internal static MouseKeyState MouseKeyState { get => mouseKeyState; set => mouseKeyState = value; }

        static InputChecker()
        {
            GameLoop.OnUpdate += InputUpdater;
        }

        public static void InputUpdater()
        {
            //Update framestate
            frameState = Keyboard.GetState();
            frameMouseState = Mouse.GetState();

            //Update all inputaction inputs
            foreach (InputAction input in InputEvents.InputActions)
            {
                input.CheckInput();
            }

            //update all mouseaction inputs
            foreach (MouseAction action in InputEvents.MouseActions)
            {
                action.CheckMouseInput();
            }

            foreach (DirectionAction action in InputEvents.DirectionActions)
            {
                action.CheckDirection();
            }

            // triggers on typing event
            if (frameState.GetPressedKeys().Length > 0)
            {
                foreach (Keys key in frameState.GetPressedKeys())
                {
                    if (!formerState.IsKeyDown(key))
                    {
                        OnTyping?.Invoke(key);

                    }
                }
            }

            //Loop through every key on keyboard
            foreach (Keys key in InputEvents.Keys)
            {
                KeyState temp = new KeyState(KeyStates[key].HeldTime);

                //check if key down
                if (KeyDown(key))
                {
                    temp.Down = true;
                }
                //check if key up
                if (KeyUp(key))
                {
                    temp.Up = true;
                    temp.Up = true;
                }
                //check if the key is current down
                if (KeyCurrentlyDown(key))
                {
                    temp.HeldDown = true;
                    temp.HeldTime += Time.DeltaTime;
                }
                //Resets time held if key isn't held
                else
                {
                    temp.HeldTime = 0f;
                }

                KeyStates[key] = temp;
            }


            //Mouse Logic
            MouseKeyState mouseTemp = new MouseKeyState();
            mouseTemp.LeftHeldTime = MouseKeyState.LeftHeldTime;
            mouseTemp.RightHeldTime = MouseKeyState.RightHeldTime;

            //Set mouse click
            mouseTemp.LeftClicked = frameMouseState.LeftButton == ButtonState.Pressed && formerMouseState.LeftButton != ButtonState.Pressed;
            mouseTemp.RightClicked = frameMouseState.RightButton == ButtonState.Pressed && formerMouseState.RightButton != ButtonState.Pressed;

            //Mouse key down
            mouseTemp.LeftHeld = frameMouseState.LeftButton == ButtonState.Pressed;
            mouseTemp.RightHeld = frameMouseState.RightButton == ButtonState.Pressed;

            //Mouse Hold
            if (mouseTemp.LeftHeld) mouseTemp.LeftHeldTime += Time.DeltaTime;
            else mouseTemp.LeftHeldTime = 0f;

            if (mouseTemp.RightHeld) mouseTemp.RightHeldTime += Time.DeltaTime;
            else mouseTemp.RightHeldTime = 0f;


            //Mouse position
            mouseTemp.MousePosition = frameMouseState.Position.ToVector2();
            MouseKeyState = mouseTemp;

            //Set formerstates to current states
            formerState = frameState;
            formerMouseState = frameMouseState;

        }

        #region Input Checkers
        /// <summary>
        /// Checks if key was down in previous frame
        /// </summary>
        /// <returns>Whether or not the key was down in the previous frame</returns>
        public static bool KeyDown(Keys key)
        {
            return frameState.IsKeyDown(key) && !formerState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks if key is up in current frame and was down in previous
        /// </summary>
        /// <returns>Whether or not the key is up in the current frame and was down in the previous frame</returns>
        public static bool KeyUp(Keys key)
        {
            return formerState.IsKeyDown(key) && !frameState.IsKeyDown(key);
        }


        /// <summary>
        /// Checks if key has been held for X amount of seconds
        /// </summary>
        /// <returns></returns>
        public static bool KeyCurrentlyDown(Keys key)
        {
            return frameState.IsKeyDown(key);
        }

        /// <summary>
        /// Give the direction of gameobject to cusor
        /// </summary>
        /// <param name="gameObject">Gameobject to find dir to/from</param>
        /// <returns></returns>
        public static Vector2 MouseDirection(Vector2 inputVector)
        {
            return Vector2.Normalize(Vector2.Subtract(inputVector, mouseKeyState.MousePosition));
        }
        #endregion
    }
}
