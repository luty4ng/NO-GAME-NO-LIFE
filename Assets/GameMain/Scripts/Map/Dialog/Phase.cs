using Packages.Rider.Editor.UnitTesting;
using UnityEngine;

public delegate void Callback();

public class Phase
{
    public string Type;
    public string Text = "";

    public Sprite LeftImage;
    public Sprite RightImage;

    public Callback callback = () => { };

    public Phase(string type)
    {
        Type = type;
    }
    
    public Phase SetLeftImage(Sprite sprite)
    {
        LeftImage = sprite;
        return this;
    }
    
    public Phase SetRightImage(Sprite sprite)
    {
        RightImage = sprite;
        return this;
    }

    public Phase SetCallback(Callback callback)
    {
        this.callback = callback;
        return this;
    }
}