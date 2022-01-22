using GameKit;
using UnityEngine;

public class Letter1Item : Character
{
    public override void Action()
    {
        EventManager.instance.EventTrigger("Show_Letter_1");
    }
}