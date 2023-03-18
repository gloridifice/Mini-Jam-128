using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Viewport
{
    public class ViewportUIManager : UIBehaviour
    {
        #region Editor Inspector

        public GameObject trappedPersonPrefab;

        public RectTransform viewportUIParent;

        #endregion

        private Canvas canvas;
        public Canvas Canvas => this.LazyGetComponent(canvas);

        public void Init(List<TrappedPerson> trappedPersons)
        {
            foreach (var trappedPerson in trappedPersons)
            {
                GameObject instance = GameObject.Instantiate(trappedPersonPrefab, viewportUIParent);
                if (instance.TryGetComponent(out ViewportTrappedPerson uiPerson))
                {
                    uiPerson.Init(trappedPerson);
                }
            }
        }
    }
}