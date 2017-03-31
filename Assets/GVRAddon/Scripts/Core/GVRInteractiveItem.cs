using System;
using UnityEngine;

public class GVRInteractiveItem : MonoBehaviour, IGvrGazeResponder {

    public event Action OnPointerEnter;
    public event Action OnPointerExit;
    public event Action OnPointerClick;

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
