using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static float currentTime = 0f;
    public static bool isPause = false;
    private float pausedTime = 0f;

    private void Update()
    {
        if (!isPause)
            currentTime = Time.unscaledTime - pausedTime;
        else
            pausedTime += Time.deltaTime;
    }
}
