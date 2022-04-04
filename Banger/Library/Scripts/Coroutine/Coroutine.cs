using System.Collections;
using System.Collections.Generic;

namespace JuicyChicken
{
    public static class Coroutine
    {
        private static readonly List<CoroutineHandler> handlers = new List<CoroutineHandler>();

        static Coroutine()
        {
            handlers = new List<CoroutineHandler>();
            GameLoop.OnUpdate += Update;
        }

        public static void Start(IEnumerator method)
        {
            CoroutineHandler handler = new CoroutineHandler(method);
            handlers.Add(handler);
            handler.Step();
        }

        public static void Update()
        {
            for (int i = 0; i < handlers.Count; i++)
            {
                CoroutineHandler handler = handlers[i];

                handler.Update();

                if (handler.IsDone)
                    handlers.Remove(handler);
            }
        }
    }
}
