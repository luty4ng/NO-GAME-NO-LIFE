using UnityEngine;

public class Phase
{
    public string Type;
    public string Text = "";

    public Sprite LeftImage;
    public Sprite RightImage;

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
}