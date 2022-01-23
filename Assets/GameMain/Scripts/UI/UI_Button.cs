using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour
{
    public bool enabled;

    private static readonly Color EnableColor = new Color(255, 255, 255, 255);
    private static readonly Color DisableColor = new Color(255, 255, 255, 100);

    private Image _image;
    
    private void Start()
    {
        _image = GetComponent<Image>();
        _image.color = enabled ? EnableColor : DisableColor;
    }
}