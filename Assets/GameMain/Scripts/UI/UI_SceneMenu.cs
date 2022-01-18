using UnityEngine;
using GameKit;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class UI_SceneMenu : UIGroup
{
    private Animator animator;
    private bool isOpen = false;
    protected override void OnStart()
    {
        animator = GetComponent<Animator>();
    }
    public void SwitchSceneMenu()
    {
        if (isOpen)
            Hide();
        else
            Show();
    }
    public void SwitchSceneSwipe(string name) => MusicBattleRegulator.current.SwitchSceneSwipe(name);
    public void Restart() => EventManager.instance.EventTrigger("Restart");
    public override void Show(UnityAction callback = null)
    {
        isOpen = true;
        animator.SetTrigger("Open");
    }

    public override void Hide(UnityAction callback = null)
    {
        isOpen = false;
        animator.SetTrigger("Close");
    }

}