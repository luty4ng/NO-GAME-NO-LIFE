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
            _imageComponent.preserveAspect = true;
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
    private Animator windowAnimator;
    public GameObject leftImage;
    private ImageController leftImageController;
    public GameObject rightImage;
    private ImageController rightImageController;
    public GameObject text;
    public GameObject overlay;
    public GameObject confirmButton;
    public GameObject cancelButton;

    private AudioSource[] audioSources;
    private AudioSource GetClickOpenSource() => audioSources[0];
    private AudioSource GetPageFlipSource() => audioSources[1];
    
    private List<Phase> currentList;
    private int currentIndex;
    private string switchScene = null;

    private bool skipNext = false;

    private void Start()
    {
        window.SetActive(true);  // to aid debugging, window is often set to inactive during development
        windowAnimator = window.GetComponent<Animator>();
        leftImageController = new ImageController(leftImage);
        rightImageController = new ImageController(rightImage);

        audioSources = GetComponents<AudioSource>();
        
        EventManager.instance.AddEventListener<List<Phase>>(EventConfig.SHOW_DIALOG, ShowDialog);
    }

    private void OnDestroy()
    {
        EventManager.instance.RemoveEventListener<List<Phase>>(EventConfig.SHOW_DIALOG, ShowDialog);
    }

    private void ShowDialog(List<Phase> phases)
    {
        if (currentList == null)
        {
            GetClickOpenSource().Play();
            currentList = phases;
            currentIndex = 0;
            SetDialogActive(true);
            windowAnimator.SetTrigger("Show");
            MapRegulator.current.DialogIn();
            DisplayNext();
            skipNext = true;
        }
    }

    private void DisplayNext()
    {
        if (currentIndex != 0)  // avoid play together with ClickOpen effect
        {
            GetPageFlipSource().Play();
            currentList[currentIndex - 1].callback();
        }
        if (currentIndex < currentList.Count)
        {
            var phase = currentList[currentIndex];
            currentIndex += 1;
            
            text.GetComponent<Text>().text = phase.Text;
            leftImageController.SetImage(phase.LeftImage);
            rightImageController.SetImage(phase.RightImage);
            
            if (phase.Type == BattleEntry.PhaseType)
            {
                switchScene = ((BattleEntry) phase).BattleScene;
                SetShowButtons(true);
            }
            else
            {
                switchScene = null;
                SetShowButtons(false);
            }
        }
        else
        {
            switchScene = null;
            currentList = null;
            SetDialogActive(false);
            MapRegulator.current.DialogOut();
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
        if (switchScene != null)
        {
            Scheduler.instance.SwitchSceneSwipe(switchScene);
        }
    }

    private void LateUpdate()  // use late to make sure this happens after possible new feed
    {
        if (!MapRegulator.current.gamePaused && currentList != null)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                AttemptSwitchScene();
            }
            if (skipNext)
            {
                skipNext = false;
            }
            else if (Input.GetKeyDown(KeyCode.L))
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