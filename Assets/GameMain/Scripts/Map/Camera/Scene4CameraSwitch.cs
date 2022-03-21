using System;
using Cinemachine;
using UnityEngine;

public class Scene4CameraSwitch : MonoBehaviour
{
    public CinemachineVirtualCamera wholeCamera;

    private void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)  // if the player moves
        {
            wholeCamera.gameObject.SetActive(false);
        }
    }
}
