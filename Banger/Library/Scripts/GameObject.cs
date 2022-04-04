using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace JuicyChicken
{
    public class GameObject
    {
        private HashSet<Component> components = new HashSet<Component>();
        private bool enabled = true;

        /// <summary>
        /// Static list of all GameObjects currently in the game
        /// </summary>
        private static List<GameObject> gameObjects = new List<GameObject>();
        private static int currentIndex;

        /// <summary>
        /// Name reference to this GameObject
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A unique ID of this GameObject
        /// </summary>
        public int Identifier { get; private set; }

        /// <summary>
        /// Invoked when Destroy is called
        /// </summary>
        public event Action OnDestroy;

        /// <summary>
        /// Invoked when Enabled is changed to true
        /// </summary>
        public event Action OnEnable;

        /// <summary>
        /// Invoked when Enabled is changed to false
        /// </summary>
        public event Action OnDisable;

        /// <summary>
        /// Whether or not this GameObject should be updated and drawed (including it's components)
        /// </summary>
        public bool Enabled
        {
            get => enabled;

            set
            {
                if (value && !enabled)
                    OnEnable?.Invoke();
                else if (!value && enabled)
                    OnDisable?.Invoke();

                enabled = value;
            }
        }

        /// <summary>
        /// Transform of the object, containing position, rotation and scale
        /// </summary>
        public Transform Transform { get; private set; } = new Transform();

        /// <summary>
        /// Default Sprite component that gets added when a GameObject is instantiated
        /// </summary>
        public Sprite Sprite { get; private set; }

        /// <summary>
        /// Constructs a new GameObject and subscribes to the update and draw event from GameLoop
        /// </summary>
        public GameObject()
        {
            GameLoop.OnUpdate += UpdateSelf;
            GameLoop.OnUpdate += UpdateComponents;
            GameLoop.OnDraw += DrawSelf;
            GameLoop.OnDraw += DrawComponents;

            Sprite = AddComponent<Sprite>();
        }

        /// <summary>
        /// Creates a new instance of the specified object type
        /// </summary>
        /// <returns>The instantiated object after setup</returns>
        public static T Instantiate<T>(Vector2 position = default, float rotation = default, Vector2 scale = default, string name = "") where T : GameObject, new()
        {
            T obj = new T();
            obj.Identifier = currentIndex++;
            obj.Name = name;
            obj.Transform.Position = position;
            obj.Transform.Rotation = rotation;
            obj.Transform.Scale = scale == default ? Vector2.One : scale;

            obj.Create();
            gameObjects.Add(obj);
            obj.OnDestroy += () => gameObjects.Remove(obj);

            Coroutine.Start(InvokeStart());
            IEnumerator InvokeStart()
            {
                yield return null;
                obj.Start();
            }

            return obj;
        }

        /// <summary>
        /// Finds a GameObject of type T
        /// </summary>
        /// <returns>The found GameObject of type T</returns>
        public static T Find<T>() where T : GameObject
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i] is T obj)
                    return obj;
            }

            return null;
        }

        /// <summary>
        /// Finds a GameObject of type T, matching a specified condition
        /// </summary>
        /// <returns>The found GameObject of type T</returns>
        public static T Find<T>(Predicate<T> match) where T : GameObject
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i] is T obj && match(obj))
                    return obj;
            }

            return null;
        }

        /// <summary>
        /// Finds all GameObjects of type T
        /// </summary>
        /// <returns>A list containing the found GameObjects</returns>
        public static List<T> FindAll<T>() where T : GameObject
        {
            List<T> list = new List<T>();

            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i] is T obj)
                    list.Add(obj);
            }

            return list;
        }

        /// <summary>
        /// Finds all GameObjects of type T, matching a specified condition
        /// </summary>
        /// <returns>A list containing the found GameObjects</returns>
        public static List<T> FindAll<T>(Predicate<T> match) where T : GameObject
        {
            List<T> list = new List<T>();

            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i] is T obj && match(obj))
                    list.Add(obj);
            }

            return list;
        }

        /// <summary>
        /// Called right after the object is instantiated (add components here)
        /// </summary>
        protected virtual void Create() { }

        /// <summary>
        /// Called on the next frame after Create
        /// </summary>
        protected virtual void Start() { }

        /// <summary>
        /// Called each frame
        /// </summary>
        protected virtual void Update() { }

        /// <summary>
        /// Called each frame after update
        /// </summary>
        protected virtual void Draw(Space space) { }

        private void UpdateSelf()
        {
            if (!Enabled)
                return;

            Update();
        }

        private void DrawSelf(Space space)
        {
            if (!Enabled)
                return;

            Draw(space);
        }

        /// <summary>
        /// Loops over all current components and updates them
        /// </summary>
        private void UpdateComponents()
        {
            if (!Enabled)
                return;

            foreach(Component component in components)
            {
                if (component.Enabled)
                    component.Update();
            }
        }

        /// <summary>
        /// Loops over all current components and draws them
        /// </summary>
        private void DrawComponents(Space drawSpace)
        {
            if (!Enabled)
                return;

            foreach (Component component in components)
            {
                if (component.Enabled)
                    component.Draw(drawSpace);
            }
        }

        /// <summary>
        /// Unsubscribes all events, thereby destroying the object
        /// </summary>
        public static void Destroy(GameObject gameObject)
        {
            foreach (Component component in gameObject.components)
                component.Remove();

            gameObject.components.Clear();

            GameLoop.OnUpdate -= gameObject.UpdateSelf;
            GameLoop.OnUpdate -= gameObject.UpdateComponents;
            GameLoop.OnDraw -= gameObject.DrawSelf;
            GameLoop.OnDraw -= gameObject.DrawComponents;

            gameObject.OnEnable = null;
            gameObject.OnDisable = null;
            gameObject.OnDestroy?.Invoke();
            gameObject.OnDestroy = null;

            gameObjects.Remove(gameObject);
        }

        /// <summary>
        /// Adds a new component to the list
        /// </summary>
        public T AddComponent<T>() where T : Component, new()
        {
            T component = new T();
            component.SetParent(this);
            component.Setup();
            components.Add(component);

            return component;
        }

        /// <summary>
        /// Retrieves component of type T from the list
        /// </summary>
        public T GetComponent<T>() where T : Component
        {
            return (T)components.First(x => x is T);
        }

        /// <summary>
        /// Tries to retrieve a component of type T from the list
        /// </summary>
        public bool TryGetComponent<T>(out T component) where T : Component
        {
            component = (T)components.First(x => x is T);
            return component != null;
        }

        /// <summary>
        /// Returns all components of this game object
        /// </summary>
        public List<Component> GetComponents()
        {
            return components.ToList();
        }

        /// <summary>
        /// Checks if the list contains a component of type T
        /// </summary>
        /// <returns>Whether or not a component was found</returns>
        public bool HasComponent<T>() where T : Component
        {
            return components.Any(x => x is T);
        }

        /// <summary>
        /// Removes a component of type T from the list, if it contains such type
        /// </summary>
        public void RemoveComponent<T>() where T : Component
        {
            Component component = components.First(x => x is T);
            if (component == null)
                return;

            component.Remove();
            components.Remove(component);
        }

        public void RemoveComponent(Component component)
        {
            if (components.Contains(component))
            {
                component.Remove();
                components.Remove(component);
            }
        }
    }
}
