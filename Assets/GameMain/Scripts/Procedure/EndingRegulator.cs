using UnityEngine;
using GameKit;

public class EndingRegulator : Regulator<EndingRegulator>
{
    public string finishScene;
    private void Start()
    {
        StartCoroutine(DelayedExcute(() =>
            {
               SwitchSceneSwipe(finishScene);
            }, 5f));
    }
}