using System;

namespace JuicyChicken
{
    public abstract class YieldInstruction
    {
        public bool IsDone { get; protected set; }
        public virtual void Start()
        {
            IsDone = false;
        }
        public virtual void Update() { }
    }

    public class WaitForSeconds : YieldInstruction
    {
        private float seconds;
        private float elapsedTime;

        public WaitForSeconds(float seconds)
        {
            this.seconds = seconds;
        }

        public override void Start()
        {
            base.Start();
            elapsedTime = 0f;
        }

        public override void Update()
        {
            if (elapsedTime < seconds)
            {
                elapsedTime += Time.DeltaTime;
            }
            else
            {
                IsDone = true;
            }
        }
    }

    public class WaitUntil : YieldInstruction
    {
        private Func<bool> condition;

        public WaitUntil(Func<bool> condition)
        {
            this.condition = condition;
        }

        public override void Update()
        {
            IsDone = condition();
        }
    }

    public class WaitWhile : YieldInstruction
    {
        private Func<bool> condition;

        public WaitWhile(Func<bool> condition)
        {
            this.condition = condition;
        }

        public override void Update()
        {
            IsDone = !condition();
        }
    }
}
