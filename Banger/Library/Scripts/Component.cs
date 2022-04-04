using System;
using System.Collections.Generic;

namespace JuicyChicken
{
    public abstract class Component
    {
        public GameObject Parent { get; private set; }
        public bool Enabled { get; set; } = true;

        public void SetParent(GameObject parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Called after all components have been added to the parent GameObject
        /// </summary>
        public virtual void Setup() { }

        /// <summary>
        /// Called each frame
        /// </summary>
        public virtual void Update() { }

        public virtual void Remove() { }

        /// <summary>
        /// Called each frame after Update
        /// </summary>
        public virtual void Draw(Space space) { }
    }
}
