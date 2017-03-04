#Add-On For GVR 1.0.3

This is just a basic addon package that simplifies the workflow for a Google Carboard Project, which I use for my Udacity VR Nanodegree Projects. The main reason I use it is to remove the hastle of all the setup. It accomplishes this by:

1. Creating a camera rig prefab with the appropiate scripts attached. Camera Rig also includes all the required GVR prefabs, such as the GvrViewerMain and the GvrReticle.
2. The camera is a child of the rig, so it can be rotated to face wherever I want. 
3. I have removed the necessity to add the Event System and EventTriggers (I found it very tedious to use this method). This is done by using the GVRInteractiveItem script, which implements the IGvrGazeResponderInterface. To create an interactive GameObject, you just add the GVRInteractiveItem component to the object, and then create the appropiate script to handle all the interactions. This script can then  be used register the appropiate methods that you create for the Actions that you want for it. The available Actions are: OnPointerEnter, OnPointerExit and OnPointerClick. These Actions are declared in the GVRInteractiveItem component. The included demo scene has a sample game object so that you can see how it works.

#Usage Instructions:

1. First import GVR 1.0.3 SDK to your project. 
2. Download and Import the "GVRAddon.unitypackage" package.
3. Drag a camera rig into your scene.
4. To add an interactive item, add it to the scene and make sure it has a collider attached to it. Then just add the GVRInteractiveItem script component to it. 
5. To handle interactions, create your custom script, and in it, create a variable to reference the GVRInteractiveItem component. You can do this however you want. Then, just register the appropiate methods to the actions on the GVRInteractiveItem component. Refer to example script below.
6. Open the demo scene to look at an example. 

#Example Interactive Item Script:

```
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
```