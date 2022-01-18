using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace GameKit
{
    public class Regulator<T> : MonoBehaviour where T : Regulator<T>
    {
        public static T current;
        private void Awake()
        {
            if (current == null)
                current = this as T;
        }

        private void OnDestroy()
        {
            current = default(T);
        }

        public void Quit()
        {
            Application.Quit();
        }
        public UIGroup GetUI(string name) => UIManager.instance.GetPanel(name);
        public void ShowUI(string name) => GetUI(name).Show();
        public void HideUI(string name) => GetUI(name).Hide();
        public void SwitchSceneSwipe(string name) => Scheduler.instance.SwitchSceneSwipe(name);
        public void SwitchScene(string name) => Scheduler.instance.SwitchScene(name);
    }
}