using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GameKit
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIGroup : UIBehaviour
    {
        protected enum FindType
        {
            All,
            SubChildren
        }
        private Dictionary<string, List<UIBehaviour>> uiComponet = new Dictionary<string, List<UIBehaviour>>();
        protected virtual FindType findType { get { return FindType.All; } }
        protected CanvasGroup canvasGroup;
        public virtual bool isActive { get { return this.gameObject.activeInHierarchy; } }
        protected override void Start()
        {
            UIManager.instance.RegisterUI(this as UIGroup);
            FindChildrenByType<Button>();
            FindChildrenByType<Image>();
            FindChildrenByType<Text>();
            FindChildrenByType<Toggle>();
            FindChildrenByType<Slider>();
            FindChildrenByType<UIForm>();
            FindChildrenByType<LayoutGroup>();
            if (findType == FindType.SubChildren)
                FindChildrenByType<UIGroup>();
            canvasGroup = GetComponent<CanvasGroup>();
            OnStart();
        }

        protected virtual void OnStart()
        {
            canvasGroup.alpha = 0;
            this.gameObject.SetActive(false);
        }
        public virtual void Show(UnityAction callback = null)
        {
            canvasGroup.alpha = 1;
            this.gameObject.SetActive(true);
        }
        public virtual void Hide(UnityAction callback = null)
        {
            canvasGroup.alpha = 0;
            this.gameObject.SetActive(false);
        }
        public T GetUIComponent<T>(string name) where T : UIBehaviour
        {
            if (uiComponet.ContainsKey(name))
            {
                for (int i = 0; i < uiComponet[name].Count; ++i)
                {
                    if (uiComponet[name][i] is T)
                    {
                        return uiComponet[name][i] as T;
                    }
                }
            }
            return null;
        }

        public List<UIBehaviour> GetUIComponents()
        {
            List<UIBehaviour> uiComps = new List<UIBehaviour>();
            foreach (var uiComp in uiComponet.Values)
            {
                foreach (var comp in uiComp)
                {
                    uiComps.Add(comp);
                }
            }
            return uiComps;
        }

        protected void FindChildrenByType<T>() where T : UIBehaviour
        {
            T[] components = this.GetComponentsInChildren<T>(true);
            for (int i = 0; i < components.Length; ++i)
            {
                if (this.findType != FindType.All && components[i].transform.parent != this.transform)
                    continue;
                string objName = components[i].gameObject.name;
                if (uiComponet.ContainsKey(objName))
                    uiComponet[objName].Add(components[i]);
                else
                    uiComponet.Add(objName, new List<UIBehaviour>() { components[i] });
            }
        }

        protected override void OnDestroy()
        {
            UIManager.instance.RemoveUI(this);
        }
    }

}

