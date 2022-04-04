using System;
using System.Collections.Generic;
using System.Text;

namespace JuicyChicken
{
    public abstract class UIGroup : GameObject
    {
        protected List<UIElement> elements = new List<UIElement>();
        
        protected void AddElements(params UIElement[] elements)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                this.elements.Add(elements[i]);
            }
        }

        protected override void Create()
        {
            OnEnable += EnableAll;
            OnDisable += DisableAll;
            Setup();
        }

        private void DisableAll()
        {
            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].Enabled = false;
            }
        }

        private void EnableAll()
        {
            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].Enabled = true;
            }
        }
        protected abstract void Setup();
    }
}
