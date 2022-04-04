using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using JuicyChicken.Input;

namespace JuicyChicken.Input
{
    public class InputAction
    {
        Keys inputKey;
        float holdTime;

        public Keys InputKey { get => inputKey; set => inputKey = value; }

        public event Action OnDownEvent;
        public event Action OnUpEvent;
        public event Action OnHoldEvent;

        public InputAction(Keys inputKey, float holdTime = 1f)
        {
            this.InputKey = inputKey;
            this.holdTime = holdTime;

            InputEvents.AddAction(this);
        }

        public void CheckInput()
        {
            if (InputChecker.KeyStates[inputKey].Down)
            {
                OnDownEvent?.Invoke();
            }
            else if (InputChecker.KeyStates[inputKey].Up)
            {
                OnUpEvent?.Invoke();
            }
            else if (InputChecker.KeyStates[inputKey].Hold(holdTime))
            {
                OnHoldEvent?.Invoke();
            }
        }

    }

    public class MouseAction
    {
        public event Action OnLeftClickDown;
        public event Action OnRightClickDown;
        public event Action OnLeftClickHold;
        public event Action OnRightClickHold;

        private float leftHoldTime;
        private float rightHoldTime;

        public MouseAction(float leftHoldTime = 1f, float rightHoldTime = 1f)
        {
            this.leftHoldTime = leftHoldTime;
            this.rightHoldTime = rightHoldTime;

            InputEvents.AddAction(this);

        }

        public void CheckMouseInput()
        {
            if (InputChecker.MouseKeyState.LeftClicked)
            {
                OnLeftClickDown?.Invoke();
            }
            else if (InputChecker.MouseKeyState.RightClicked)
            {
                OnRightClickDown?.Invoke();
            }
            else if (InputChecker.MouseKeyState.LeftHoldDuration(leftHoldTime))
            {
                OnLeftClickHold?.Invoke();
            }
            else if (InputChecker.MouseKeyState.RightHoldDuration(rightHoldTime))
            {
                OnRightClickHold?.Invoke();
            }
        }
    }

    public class DirectionAction
    {
        private Keys keyUp;
        private Keys keyDown;
        private Keys keyLeft;
        private Keys keyRight;

        public List<Keys> DirectionKeys { get; private set; }

        public event Action<Vector2> OnDirectionInput;

        /// <summary>
        /// Set up an direction action
        /// </summary>
        /// <param name="keyUp">Up key</param>
        /// <param name="keyDown">Down key</param>
        /// <param name="keyLeft">Left key</param>
        /// <param name="keyRight">Right key</param>
        public DirectionAction(Keys keyUp = Keys.W, Keys keyDown = Keys.S, Keys keyLeft = Keys.A, Keys keyRight = Keys.D)
        {
            this.keyUp = keyUp;
            this.keyDown = keyDown;
            this.keyLeft = keyLeft;
            this.keyRight = keyRight;

            DirectionKeys = new List<Keys> { keyUp, keyDown, keyLeft, keyRight };

            InputEvents.AddAction(this);
        }

        public void CheckDirection()
        {
            Vector2 dir = Vector2.Zero;

            Dictionary<Direction, bool> dirList = new Dictionary<Direction, bool>();

            dirList.Add(Direction.Up, InputChecker.KeyStates[keyUp].HeldDown);
            dirList.Add(Direction.Down, InputChecker.KeyStates[keyDown].HeldDown);
            dirList.Add(Direction.Left, InputChecker.KeyStates[keyLeft].HeldDown);
            dirList.Add(Direction.Right, InputChecker.KeyStates[keyRight].HeldDown);

            foreach (Direction direction in dirList.Keys)
            {
                switch (direction)
                {
                    case Direction.Up:
                        if (dirList[Direction.Up]) dir += new Vector2(0, -1);
                        break;
                    case Direction.Down:
                        if (dirList[Direction.Down]) dir += new Vector2(0, 1);
                        break;
                    case Direction.Left:
                        if (dirList[Direction.Left]) dir += new Vector2(-1, 0);
                        break;
                    case Direction.Right:
                        if (dirList[Direction.Right]) dir += new Vector2(1, 0);
                        break;
                    default:
                        break;
                }
            }


            if (dir.Length() > 0)
            {
                OnDirectionInput?.Invoke(dir);
            }
        }
    }

    internal enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
