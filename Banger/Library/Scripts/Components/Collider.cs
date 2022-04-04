using System;
using System.Linq;
using System.Collections.Generic;

namespace JuicyChicken
{
    public abstract class Collider : Component
    {
        private HashSet<Collider> collisions = new HashSet<Collider>();

        public event Action<Collider> OnCollision;
        public event Action<Collider> OnCollisionEnter;
        public event Action<Collider> OnCollisionExit;
        public bool IsColliding { get => collisions.Count > 0; }

        public bool Static { get; set; } = false;

        public override void Setup()
        {
            Collision.Register(this);
        }

        public abstract bool Intersects(Collider other);

        public override void Remove()
        {
            Collision.Deregister(this);

            OnCollision = null;
            OnCollisionEnter = null;
            OnCollisionExit = null;
            collisions.Clear();
        }

        //public void AddOther(Collider other)
        //{
        //    OnCollision?.Invoke(other);

        //    if (!collisions.Contains(other))
        //    {
        //        collisions.Add(other);
        //        OnCollisionEnter?.Invoke(other);
        //    }
        //}

        //public void RemoveOther(Collider other)
        //{
        //    if (collisions.Contains(other))
        //    {
        //        collisions.Remove(other);
        //        OnCollisionExit?.Invoke(other);
        //    }
        //}

        public void UpdateCollision(Collider other)
        {
            if (Static)
                return;

            if (Enabled == false || other == this || other.Enabled == false)
                return;

            if (Parent.Enabled == false || other.Parent.Enabled == false)
                return;

            if (Intersects(other))
            {
                OnCollision?.Invoke(other);
                if (!collisions.Contains(other))
                {
                    collisions.Add(other);
                    OnCollisionEnter?.Invoke(other);
                }
            }
            else
            {
                if (collisions.Contains(other))
                {
                    collisions.Remove(other);
                    OnCollisionExit?.Invoke(other);
                }
            }
        }

        public void CleanUp()
        {
            List<Collider> toBeRemoved = new List<Collider>();
            foreach (Collider other in collisions)
            {
                if (other.Parent == null && collisions.Contains(other))
                {
                    toBeRemoved.Add(other);
                }
            }

            for (int i = 0; i < toBeRemoved.Count; i++)
                collisions.Remove(toBeRemoved[i]);
        }
    }
}
