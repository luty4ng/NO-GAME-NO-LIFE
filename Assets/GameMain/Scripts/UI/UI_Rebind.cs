using System;
using GameKit;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public enum ListenState
{
    None,
    Attack,
    Defense
}
public class UI_Rebind : MonoBehaviour
{
    public Text attack;
    public Text defense;
    public ListenState state;
    public void AttackListen()
    {
        state = ListenState.Attack;
        attack.text = "攻击键：--待输入--";
    }

    public void DefenseListen()
    {
        state = ListenState.Defense;
        defense.text = "防御键：--待输入--";
    }

    private void Update()
    {
        if (state == ListenState.Attack)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Reset();
                return;
            }

            if (Input.anyKeyDown && !(Input.GetMouseButtonDown(0)
            || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
            {
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        Debug.Log("Current Attack Key is : " + keyCode.ToString());
                        KeyBind.ATTACK_KEY = keyCode;
                    }
                }
                Reset();
            }

        }
        else if (state == ListenState.Defense)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Reset();
                return;
            }

            if (Input.anyKeyDown && !(Input.GetMouseButtonDown(0)
            || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
            {
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        Debug.Log("Current Defense Key is : " + keyCode.ToString());
                        KeyBind.DEFENSE_KEY = keyCode;
                    }
                }
                Reset();
            }
        }
    }

    public void Reset()
    {
        attack.text = "攻击键：" + KeyBind.ATTACK_KEY.ToString();
        defense.text = "防御键：" + KeyBind.DEFENSE_KEY.ToString();
        EventManager.instance.EventTrigger(EventConfig.UPDATE_KEYBIND);
        state = ListenState.None;
    }

    public void Default()
    {
        KeyBind.ATTACK_KEY = KeyCode.A;
        KeyBind.DEFENSE_KEY = KeyCode.D;
        Reset();
    }
}