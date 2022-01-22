using UnityEngine;

public class Dialog : Phase
{
    public static string PhaseType = "Dialog";
    public Dialog(string text) : base(PhaseType)
    {
        Text = text;
    }
}