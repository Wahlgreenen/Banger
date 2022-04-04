using Microsoft.Xna.Framework;

namespace JuicyChicken
{
    public abstract class UIElement : GameObject
    {
        private Vector2 referenceScale;
        private Vector2 referencePosition;

        protected Sprite sprite;

        protected override void Create()
        {
            Graphics.OnResolutionChanged += AdjustSize;
            sprite = AddComponent<Sprite>();
            sprite.Space = Space.Screen;
            referencePosition = Transform.Position;
            referenceScale = Transform.Scale;
        }

        private void AdjustSize()
        {
            Transform.Position = referencePosition * Graphics.AspectRatio;
            Transform.Scale = referenceScale * Graphics.AspectRatio;
        }
    }
}
