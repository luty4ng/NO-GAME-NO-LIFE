using System.Collections.Generic;
using GameKit;
using UnityEngine;
using UnityEngine.Serialization;

public class Character : Interactable
{
    public override void OnEnter()
    {
        GetComponentInChildren<InteractHint>()?.Show();
    }
    
    public override void OnExit()
    {
        GetComponentInChildren<InteractHint>()?.Hide();
    }
}