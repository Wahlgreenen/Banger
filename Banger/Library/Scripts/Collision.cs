using System.Collections.Generic;

namespace JuicyChicken
{
    public static class Collision
    {
        private static List<Collider> colliders = new List<Collider>();

        static Collision()
        {
            GameLoop.OnUpdate += DetectCollision;
        }

        public static void Register(Collider collider)
        {
            if (colliders.Contains(collider))
                return;

            colliders.Add(collider);
        }

        public static void Deregister(Collider collider)
        {
            if (colliders.Contains(collider))
                colliders.Remove(collider);
        }

        private static void DetectCollision()
        {
            for (int x = 0; x < colliders.Count; x++)
            {
                for (int y = 0; y < colliders.Count; y++)
                {
                    if (x >= colliders.Count || y >= colliders.Count)
                        continue;

                    colliders[x].UpdateCollision(colliders[y]);
                }
            }

            for (int i = 0; i < colliders.Count; i++)
            {
                colliders[i].CleanUp();
            }
        }
    }
}
