using System;
using System.Collections.Generic;
using GameKit;
using UnityEngine;
using UnityEngine.UI;

public class DialogUIController : MonoBehaviour
{
    public GameObject window;
    private Animator _windowAnimator;
    public GameObject leftImage;
    private ImageController _leftImageController;
    public GameObject rightImage;
    private ImageController _rightImageController;
    public GameObject text;
    public GameObject overlay;

    private List<Phase> _currentList;
    private int _currentIndex;
    
    private class ImageController
    {
        private Image _imageComponent;
        public ImageController(GameObject imageObject)
        {
            _imageComponent = imageObject.GetComponent<Image>();
        }

        public void SetImage(Sprite sprite)
        {
            if (sprite != null)
            {
                _imageComponent.enabled = true;
                _imageComponent.sprite = sprite;
            }
            else
            {
                _imageComponent.enabled = false;
            }
        }
    }
    
    private void Start()
    {
        _windowAnimator = window.GetComponent<Animator>();
        _leftImageController = new ImageController(leftImage);
        _rightImageController = new ImageController(rightImage);
        EventManager.instance.AddEventListener<List<Phase>>(EventConfig.SHOW_DIALOG, ShowDialog);
    }

    private void ShowDialog(List<Phase> phases)
    {
        if (_currentList == null)
        {
            _currentList = phases;
            _currentIndex = 0;
            SetDialogActive(true);
            _windowAnimator.SetTrigger("Show");
            MapGlobalStatus.DialogUIActive = true;
        }
    }

    private void DisplayNext()
    {
        if (_currentIndex < _currentList.Count)
        {
            var phase = _currentList[_currentIndex];
            _currentIndex += 1;
            text.GetComponent<Text>().text = phase.Text;
            _leftImageController.SetImage(phase.LeftImage);
            _rightImageController.SetImage(phase.RightImage);
        }
        else
        {
            _currentList = null;
            SetDialogActive(false);
            MapGlobalStatus.DialogUIActive = false;
        }
    }

    private void SetDialogActive(bool active)
    {
        leftImage.SetActive(active);
        rightImage.SetActive(active);
        text.SetActive(active);
        overlay.SetActive(active);
        window.GetComponent<Animator>().SetTrigger(active ? "Show" : "Hide");
    }

    private void LateUpdate()
    {
        if (_currentList != null && Input.GetKeyDown(KeyCode.L))
        {
            DisplayNext();
        }
    }
}