using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GVRInteractiveItem : MonoBehaviour, IGvrGazeResponder {

    public Action OnPointerEnter;
    public Action OnPointerExit;
    public Action OnPointerClick;

    // Implement IGvrGazeResponder
    public void OnGazeEnter()
    {
        if (OnPointerEnter != null)
        {
            OnPointerEnter();
        }
    }

    public void OnGazeExit()
    {
        if (OnPointerExit != null)
        {
            OnPointerExit();
        }
    }

    public void OnGazeTrigger()
    {
        if (OnPointerClick != null)
        {
            OnPointerClick();
        }
    }
}
