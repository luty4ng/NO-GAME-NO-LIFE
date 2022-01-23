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
            current = this as T;
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
        public void ReloadCurrentSceneSwipe() => Scheduler.instance.ReloadCurrentSceneSwipe();
        protected IEnumerator DelayedExcute(UnityAction action, float t)
        {
            yield return new WaitForSeconds(t);
            action?.Invoke();
        }
    }
}