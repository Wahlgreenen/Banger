using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Threading;
using System.Diagnostics;

namespace JuicyChicken.Input
{
    public static class InputEvents
    {
        //Contains every inputaction and their keys
        private static List<InputAction> inputActions = new List<InputAction>();
        private static List<MouseAction> mouseActions = new List<MouseAction>();
        private static List<DirectionAction> directionActions = new List<DirectionAction>();
        private static List<Keys> keys = new List<Keys>();
        internal static bool runUpdate = true;

        public static List<InputAction> InputActions { get => inputActions;}
        public static List<MouseAction> MouseActions { get => mouseActions;}
        public static List<DirectionAction> DirectionActions { get => directionActions;}
        public static List<Keys> Keys { get => keys; set => keys = value; }

        internal static void AddAction(InputAction action)
        {
            InputActions.Add(action);
            Keys.Add(action.InputKey);
            InputChecker.KeyStates.Add(action.InputKey, new KeyState());
        }
        internal static void AddAction(MouseAction action)
        {
            MouseActions.Add(action);
        }
        internal static void AddAction(DirectionAction action)
        {
            DirectionActions.Add(action);

            Keys.AddRange(action.DirectionKeys);

            foreach (Keys key in action.DirectionKeys)
            {
                Debug.WriteLine($"Adding {key} to dictionary");
                InputChecker.KeyStates.Add(key, new KeyState());
            }
        }

    }

    /// <summary>
    /// used to store everything about a key
    /// </summary>
    internal class KeyState
    {
        private bool down;
        private bool up;
        private bool heldDown;
        private float heldTime;

        public bool Down { get => down; set => down = value; }
        public bool Up { get => up; set => up = value; }
        public float HeldTime { get => heldTime; set => heldTime = value; }
        public bool HeldDown { get => heldDown; set => heldDown = value; }

        public KeyState(float heldTime = 0f, bool down = false, bool up = false, bool currentDown = false)
        {
            this.down = down;
            this.up = up;
            this.heldTime = 0f;
            this.heldDown = currentDown;
            this.heldTime = heldTime;
        }

        public bool Hold(float time)
        {
            if (heldTime > time)
            {
                heldTime = 0f;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    internal class MouseKeyState
    {
        private Vector2 mousePosition;
        private bool leftClicked;
        private bool rightClicked;
        private bool leftHeld;
        private bool rightHeld;

        private float leftHeldTime;
        private float rightHeldTime;

        public Vector2 MousePosition { get => mousePosition; set => mousePosition = value; }
        public bool LeftClicked { get => leftClicked; set => leftClicked = value; }
        public bool RightClicked { get => rightClicked; set => rightClicked = value; }
        public bool LeftHeld { get => leftHeld; set => leftHeld = value; }
        public bool RightHeld { get => rightHeld; set => rightHeld = value; }
        public float LeftHeldTime { get => leftHeldTime; set => leftHeldTime = value; }
        public float RightHeldTime { get => rightHeldTime; set => rightHeldTime = value; }

        public MouseKeyState()
        {
            MousePosition = Vector2.Zero;

            LeftClicked = false;
            LeftHeld = false;
            LeftHeldTime = 0f;

            RightClicked = false;
            RightHeld = false;
            RightHeldTime = 0f;
        }

        public bool LeftHoldDuration(float time)
        {
            if (leftHeldTime >= time)
            {
                leftHeldTime = 0f;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RightHoldDuration(float time)
        {
            if (rightHeldTime >= time)
            {
                RightHeldTime = 0f;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}