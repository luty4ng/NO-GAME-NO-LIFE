using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using DG.Tweening;

namespace GameKit
{

    public class Scheduler : MonoBehaviour
    {
        public enum SceneSwitchType
        {
            Swipe,
            Switch,
            LoadingBar
        }
        public static Scheduler instance;
        public string startScene = "S_Menu";
        private string loadingScene = "S_Loading";
        private Queue<string> previousScenes = new Queue<string>();
        public string currentScene { get; private set; }
        private Transform swipePanel;
        private void Awake()
        {
            instance = this;
            if (SceneManager.sceneCount > 1)
                currentScene = SceneManager.GetSceneAt(1).name;
            else
                LoadSceneAsyn(startScene, callback: () => { currentScene = startScene; });
            swipePanel = GameObject.Find("SwipePanel").transform;
        }

        public void SwitchSceneSwipe(string name, UnityAction callback = null)
        {
            swipePanel.DOLocalMoveX(0, 0.5f).OnComplete(() =>
            {
                LoadSceneAsyn(name, () =>
                {
                    previousScenes.Enqueue(currentScene);
                    currentScene = name;
                    UnloadSceneAsyn(previousScenes.Peek(), () =>
                    {
                        swipePanel.DOLocalMoveX(-1920, 0.5f).OnComplete(() =>
                        {
                            swipePanel.localPosition = new Vector3(1920, swipePanel.localPosition.y, swipePanel.localPosition.z);
                        });
                    });
                });
            });
        }

        public void SwitchScene(string name, UnityAction callback = null)
        {
            previousScenes.Enqueue(currentScene);
            LoadSceneAsyn(name, () =>
            {
                currentScene = name;
                UnloadSceneAsyn(previousScenes.Peek());
            });
        }

        public void ReloadCurrentSceneSwipe()
        {
            string tmpScene = currentScene;
            swipePanel.DOLocalMoveX(0, 0.5f).OnComplete(() =>
            {
                LoadSceneAsyn(currentScene, () =>
                {
                    currentScene = currentScene;
                    UnloadSceneAsyn(tmpScene, () =>
                    {
                        swipePanel.DOLocalMoveX(-1920, 0.5f).OnComplete(() =>
                        {
                            swipePanel.localPosition = new Vector3(1920, swipePanel.localPosition.y, swipePanel.localPosition.z);
                        });
                    });
                });
            });
        }

        public void LoadPreviousSceneSwipe()
        {
            string pScene = previousScenes.Dequeue();
            string tmpScene = currentScene;
            swipePanel.DOLocalMoveX(0, 0.5f).OnComplete(() =>
            {
                LoadSceneAsyn(pScene, () =>
                {
                    currentScene = pScene;
                    UnloadSceneAsyn(tmpScene, () =>
                    {
                        swipePanel.DOLocalMoveX(-1920, 0.5f).OnComplete(() =>
                        {
                            swipePanel.localPosition = new Vector3(1920, swipePanel.localPosition.y, swipePanel.localPosition.z);
                        });
                    });
                });
            });
        }

        public void SwitchSceneLoadingBar(string name, UnityAction callback = null)
        {
            ScenesManager.instance.TryGetScene(name, out Scene scene);
            if (scene == null)
            {
                Debug.LogWarning("No such scene in build settings.");
                return;
            }
            SwitchScene(loadingScene, callback);
        }

        private void LoadSceneAsyn(string name, UnityAction callback = null)
        {
            ScenesManager.instance.LoadSceneAsynAdd(name, callback);
        }
        private void UnloadSceneAsyn(string name, UnityAction callback = null)
        {
            ScenesManager.instance.UnloadSceneAsyn(name, callback);
        }

        public int GetActiveSceneNumber()
        {
            if (SceneManager.sceneCount > 1)
            {
                int order = 0;
                string[] levelSplit = SceneManager.GetSceneAt(1).name.Split('_');
                if (levelSplit.Length <= 1)
                    return 0;
                System.Int32.TryParse(levelSplit[1], out order);
                return order;
            }
            Debug.LogWarning("No Active Level in Scene");
            return 0;
        }
    }

}

