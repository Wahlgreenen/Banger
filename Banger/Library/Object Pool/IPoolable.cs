namespace TopDownShooter
{
    public interface IPoolable
    {
        /// <summary>
        /// Checks if this object came from the pool (set by the pool)
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Checks if this object is currently spawned (set by the pool)
        /// </summary>
        public bool IsActive { get; set; }

        public void OnSpawn();

        public void OnDespawn();
    }
}
