using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomReticle : MonoBehaviour, IGvrGazePointer {

    public Color gazeStartColor;
    public Color gazeExitColor;
    public Color gazeStayColor;
    public Color gazeTriggerStartColor;
    public Color gazeTriggerEndColor;

    private Material reticleMaterial;
    private GvrReticle reticle;

    void OnEnable()
    {
        GazeInputModule.gazePointer = this;
    }

    void OnDisable()
    {
        if (GazeInputModule.gazePointer == this)
        {
            GazeInputModule.gazePointer = null;
        }
    }

    private void Start()
    {
        reticle = gameObject.GetComponent<GvrReticle>();
        reticleMaterial = gameObject.GetComponent<MeshRenderer>().material;
    }

    public void OnGazeDisabled()
    {
    }

    public void OnGazeEnabled()
    {
    }

    public void OnGazeExit(Camera camera, GameObject targetObject)
    {
    }

    public void OnGazeStart(Camera camera, GameObject targetObject, Vector3 intersectionPosition, bool isInteractive)
    {
        Debug.Log("Gaze Start!");

        if (gazeStartColor != null) {
            reticleMaterial.color = gazeStartColor;
        }
    }

    public void OnGazeStay(Camera camera, GameObject targetObject, Vector3 intersectionPosition, bool isInteractive)
    {
        Debug.Log("Gaze Stay!");

        if (gazeStartColor != null)
        {
            reticleMaterial.color = gazeStartColor;
        }
    }

    public void OnGazeTriggerEnd(Camera camera)
    {
        Debug.Log("Gaze Trigger End!");

        if (gazeStartColor != null)
        {
            reticleMaterial.color = gazeStartColor;
        }
    }

    public void OnGazeTriggerStart(Camera camera)
    {
        Debug.Log("Gaze Trigger Start!");

        if (gazeStartColor != null)
        {
            reticleMaterial.color = gazeStartColor;
        }
    }

    public void GetPointerRadius(out float innerRadius, out float outerRadius)
    {
        ((IGvrGazePointer)reticle).GetPointerRadius(out innerRadius, out outerRadius);
    }
}
