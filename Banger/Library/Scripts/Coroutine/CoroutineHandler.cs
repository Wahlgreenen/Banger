using System.Collections;

namespace JuicyChicken
{
    public class CoroutineHandler
    {
        private IEnumerator method;

        public bool IsDone { get; private set; }

        public CoroutineHandler(IEnumerator method)
        {
            this.method = method;
        }

        public void Update()
        {
            if (method.Current is YieldInstruction instruction)
            {
                if (instruction.IsDone)
                {
                    Step();
                }
                else
                {
                    instruction.Update();
                }
            }
            else
            {
                Step();
            }
        }

        public void Step()
        {
            if (method.MoveNext())
            {
                if (method.Current is YieldInstruction instruction)
                    instruction.Start();
            }
            else
            {
                IsDone = true;
            }
        }
    }
}