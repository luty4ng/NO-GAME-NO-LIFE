using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogSet : MonoBehaviour
{
    public int currentPhase = 0;
    public List<string> dialogs = new List<string>();
    public string NextPhase()
    {
        if (currentPhase < dialogs.Count - 1)
        {
            currentPhase += 1;
            return dialogs[currentPhase];
        }
        return "last";
    }

    public string FirstPhase()
    {
        if (dialogs.Count > 0)
        {
            return dialogs[0];
        }
        return "none";
    }
}