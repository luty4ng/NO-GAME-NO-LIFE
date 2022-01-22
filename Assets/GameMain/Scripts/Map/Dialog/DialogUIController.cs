using System;
using System.Collections.Generic;
using GameKit;
using UnityEngine;
using UnityEngine.UI;

class ImageController
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
    public GameObject confirmButton;
    public GameObject cancelButton;

    private List<Phase> _currentList;
    private int _currentIndex;
    private string _switchScene = null;

    private void Start()
    {
        window.SetActive(true);  // to aid debugging, window is often set to inactive during development
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
            MapGlobals.DialogUIActive = true;
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
            
            if (phase.Type == BattleEntry.PhaseType)
            {
                _switchScene = ((BattleEntry) phase).BattleScene;
                SetShowButtons(true);
            }
            else
            {
                _switchScene = null;
                SetShowButtons(false);
            }
        }
        else
        {
            _switchScene = null;
            _currentList = null;
            SetDialogActive(false);
            MapGlobals.DialogUIActive = false;
        }
    }

    private void SetDialogActive(bool active)
    {
        leftImage.SetActive(active);
        rightImage.SetActive(active);
        text.SetActive(active);
        overlay.SetActive(active);
        window.GetComponent<Animator>().SetTrigger(active ? "Show" : "Hide");
        if (active == false)  // only disable buttons when close
        {
            confirmButton.SetActive(false);
            cancelButton.SetActive(false);
        }
    }

    private void SetShowButtons(bool show)
    {
        confirmButton.SetActive(show);
        cancelButton.SetActive(show);
    }

    private void AttemptSwitchScene()
    {
        if (_switchScene != null)
        {
            Scheduler.instance.SwitchSceneSwipe(_switchScene);
        }
    }

    private void LateUpdate()  // use late to make sure this happens after possible new feed
    {
        if (_currentList != null)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                AttemptSwitchScene();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                DisplayNext();
            }
        }
    }

    public void OnConfirmButtonClicked()
    {
        AttemptSwitchScene();
    }
    
    public void OnCancelButtonClicked()
    {
        DisplayNext();
    }
}