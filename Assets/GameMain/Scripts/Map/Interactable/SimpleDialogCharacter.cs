using System.Collections.Generic;

// This implementation isn't optimal, but it avoids changing existing logic
public abstract class SimpleDialogCharacter : DialogCharacter
{
    public virtual void AddDialogPhases(List<Phase> phases) { }

    public override void AddFirstTimePhases(List<Phase> phases)
    {
        AddDialogPhases(phases);
    }

    public override void AddRepeatPhases(List<Phase> phases)
    {
        AddDialogPhases(phases);
    }

    public override void AddCommonEndPhases(List<Phase> phases) { }
}