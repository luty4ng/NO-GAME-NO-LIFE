using System;
using System.Collections.Generic;

public abstract class DialogCharacter : Character
{
    private bool visited = false;

    public override void Action()
    {
        var phases = new List<Phase>();
        if (!visited)
        {
            visited = true;
            AddFirstTimePhases(phases);
        }
        else
        {
            AddRepeatPhases(phases);
        }
        AddCommonEndPhases(phases);
        MapGlobals.FeedDialog(phases);
    }

    public virtual void AddFirstTimePhases(List<Phase> phases) { }

    public virtual void AddRepeatPhases(List<Phase> phases) { }

    public virtual void AddCommonEndPhases(List<Phase> phases) { }
}