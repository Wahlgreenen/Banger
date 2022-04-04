using System;
using System.Linq;
using System.Collections.Generic;

namespace TopDownShooter
{
    public class ObjectPool<T> where T : IPoolable, new()
    {
        private Stack<T> stack;
        private int capacity;

        /// <summary>
        /// Constructs a new object pool of type T with a specified capacity
        /// </summary>
        /// <param name="capacity">Predefined start size of the dynamic pool</param>
        public ObjectPool(int capacity)
        {
            stack = new Stack<T>(capacity);
            this.capacity = capacity;

            for (int i = 0; i < capacity; i++)
            {
                AddObject();
            }
        }

        /// <summary>
        /// Instantiates a new object and adds it to the pool for later use
        /// </summary>
        private void AddObject()
        {
            T instance = new T();
            instance.IsValid = true;
            stack.Push(instance);
        }

        /// <summary>
        /// Finds a free object from the pool to spawn
        /// </summary>
        /// <returns></returns>
        public T Spawn()
        {
            // If there is no objects, instantiates a new one
            if (stack.Count == 0)
            {
                AddObject();
            }

            T instance = stack.Pop();
            instance.IsActive = true;
            instance.OnSpawn();
            return instance;
        }

        /// <summary>
        /// Releases an object from the pool
        /// </summary>
        public T Despawn(T instance)
        {
            if (!instance.IsValid)
            {
                throw new Exception($"POOL: {instance} was not instantiated by this pool.");
            }

            instance.IsActive = false;

            if (stack.Count < capacity)
                stack.Push(instance);

            instance.OnDespawn();
            return instance;
        }

        /// <summary>
        /// Despawns all objects
        /// </summary>
        public void DespawnAll()
        {
            List<T> stackToList = stack.ToList();
            for (int i = 0; i < stackToList.Count; i++)
            {
                Despawn(stackToList[i]);
            }
        }
    }
}
