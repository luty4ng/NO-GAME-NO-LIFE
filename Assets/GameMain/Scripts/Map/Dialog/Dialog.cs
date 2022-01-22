using UnityEngine;

public class Dialog : Phase
{
    public Dialog(string text) : base("Dialog")
    {
        Text = text;
    }
}