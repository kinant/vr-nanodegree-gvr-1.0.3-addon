using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(GVRInteractiveItem))]
public class SampleInteractiveCube : MonoBehaviour {

    [SerializeField]
    private GVRInteractiveItem m_GvrInteractiveItem;

    private void OnEnable()
    {
        m_GvrInteractiveItem.OnPointerEnter += HandleEnter;
        m_GvrInteractiveItem.OnPointerExit += HandleExit;
        m_GvrInteractiveItem.OnPointerClick += HandleClick;
    }

    private void OnDisable()
    {
        m_GvrInteractiveItem.OnPointerEnter -= HandleEnter;
        m_GvrInteractiveItem.OnPointerExit -= HandleExit;
        m_GvrInteractiveItem.OnPointerClick -= HandleClick;
    }

    private void HandleEnter() {
        Debug.Log("Pointer has entered!");
    }

    private void HandleExit() {
        Debug.Log("Pointer has exited!");
    }

    private void HandleClick() {
        Debug.Log("Pointer has been clicked!");
    }
}
