using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace GameKit
{
    public class UIManager : SingletonBase<UIManager>
    {
        private Dictionary<string, UIGroup> panels = new Dictionary<string, UIGroup>();
        public void ShowPanel<T>(string panelName, UnityAction<T> callback = null) where T : UIGroup
        {

        }

        public void HidePanel(string panelName)
        {

        }

        public void RegisterUI(UIGroup panel)
        {
            if (panels == null)
                panels = new Dictionary<string, UIGroup>();
            // Debug.Log("Register UI: " + panel.gameObject.name);
            if (!panels.ContainsKey(panel.gameObject.name))
                panels.Add(panel.gameObject.name, panel);
            else
                panels[panel.gameObject.name] = panel;
        }

        public void RemoveUI(UIGroup panel)
        {
            if (panels.ContainsKey(panel.gameObject.name))
                panels.Remove(panel.gameObject.name);
        }

        public void Clear()
        {
            if (panels == null)
                return;
            if (panels.Count > 0)
                panels.Clear();
        }
        public UIGroup GetPanel(string name)
        {
            if (panels.ContainsKey(name))
                return panels[name];
            return null;
        }

        public T GetPanel<T>(string name) where T : UIGroup
        {
            if (panels.ContainsKey(name))
                return panels[name] as T;
            return null;
        }
    }
}

