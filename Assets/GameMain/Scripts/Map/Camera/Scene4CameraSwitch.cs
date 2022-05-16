using System;
using Cinemachine;
using UnityEngine;

public class Scene4CameraSwitch : MonoBehaviour
{
    public CinemachineVirtualCamera wholeCamera;

    private float switchStartTime = 0.0f;
    private float startSwitchInterval = 1.0f;

    private void Start()
    {
        switchStartTime = Time.time + startSwitchInterval;
    }

    private void Update()
    {
        if (!MapRegulator.current.DialogUIActive
            && Time.time > switchStartTime
            && Input.GetAxisRaw("Horizontal") != 0)  // if the player moves
        {
            wholeCamera.gameObject.SetActive(false);
        }
    }
}
