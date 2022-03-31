using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogSet : MonoBehaviour
{
    public int currentPhase = 0;
    public bool hasStartDialog = true;
    public bool hasEndDialog = true;
    public List<string> startDialogs = new List<string>();
    public List<string> endDialogs = new List<string>();
    public string StartNextPhase()
    {
        if (currentPhase < startDialogs.Count - 1)
        {
            currentPhase += 1;
            return startDialogs[currentPhase];
        }
        return "last";
    }

    public string StartFirstPhase()
    {
        currentPhase = 0;
        if (startDialogs.Count > 0)
        {
            return startDialogs[0];
        }
        return "如果你看到这句话，就代表程序将遭受一顿毒打。";
    }

    public string EndNextPhase()
    {
        if (currentPhase < endDialogs.Count - 1)
        {
            currentPhase += 1;
            return endDialogs[currentPhase];
        }
        return "last";
    }

    public string EndFirstPhase()
    {
        currentPhase = 0;
        if (endDialogs.Count > 0)
        {
            return endDialogs[0];
        }
        return "如果你看到这句话，就代表程序将遭受一顿毒打。";
    }
}