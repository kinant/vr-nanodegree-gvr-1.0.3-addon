using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

	[SerializeField]
	private GVRInteractiveItem m_GVRInteractiveItem;

	public Waypoint[] neighbors;

	// Color properties
	public bool allowColorChanges = false;
	public Color highlightColor;
	public Color triggerColor;

    private Transform m_ChildTransform;
	private Renderer m_ChildRenderer;
	private Color m_OriginalColor;

	// Rotation properties
	public bool rotate = false;
	public bool rotateOnHighlight = false;
	public float rotationSpeed = 1.0f;
	public Vector3 rotationAngle = new Vector3(45.0f, 45.0f, 45.0f);

	private bool shouldRotate = false;
	private bool shouldRotateHighlight = false;
	private bool isHighlighted = false;

	// Pulsating Scale Properties
	public bool scalePulse = false;
	public bool scalePulseOnHighlight = false;
	public float pulseSpeed = 1.0f;
	public Vector3 scaleByVector = new Vector3(1.0f, 1.0f, 1.0f);

	private Vector3 targetScale;
	private Vector3 originalScale;
	private Vector3 currScale;

	bool scalingStep = true;
	bool shouldPulse = false;
	bool shouldPulseOnHighlight = false;

	// private Transform m_ChildTransform;
	private WaypointNetworkNavigation _parentNetwork;

	public void OnEnable(){
		m_GVRInteractiveItem.OnPointerClick += OnReticleHit;
		m_GVRInteractiveItem.OnPointerEnter += OnReticleEnter;
		m_GVRInteractiveItem.OnPointerExit += OnReticleExit;
	}

	public void OnDisable(){
		m_GVRInteractiveItem.OnPointerClick -= OnReticleHit;
		m_GVRInteractiveItem.OnPointerEnter -= OnReticleEnter;
		m_GVRInteractiveItem.OnPointerExit -= OnReticleExit;
	}

	// Use this for initialization
	void Start () {

        m_ChildTransform = gameObject.transform.GetChild(0);

        if (m_ChildTransform != null && m_ChildTransform.gameObject.GetComponent<Renderer> ()) {
            m_ChildRenderer = m_ChildTransform.gameObject.GetComponent<Renderer>();
            m_OriginalColor = m_ChildRenderer.material.color;
		}
		if (rotate) {
			shouldRotate = true;
		}

		originalScale = m_ChildTransform.localScale;
		targetScale = new Vector3(originalScale.x * scaleByVector.x, originalScale.y * scaleByVector.y, originalScale.z * scaleByVector.z);

		_parentNetwork = GetComponentInParent<WaypointNetworkNavigation> ();

		if (_parentNetwork == null) {
			Debug.LogError ("WAYPOINT ERROR: Waypoint is not a child of a Waypoint Network! Make sure parent has Waypoint Network component attached!");
		}
	}

	void Update(){
		shouldRotate = rotate;
		shouldPulse = scalePulse;

		if (shouldRotate && (!isHighlighted && !shouldRotateHighlight) || (shouldRotateHighlight && isHighlighted)) {
			m_ChildTransform.Rotate (rotationAngle * rotationSpeed * Time.deltaTime);
		}
			
		if (shouldPulse && (!isHighlighted && !shouldPulseOnHighlight) || (shouldPulseOnHighlight && isHighlighted)) {
			currScale = m_ChildTransform.localScale;

			Vector3 newScale = Vector3.Lerp (currScale, targetScale, pulseSpeed * Time.deltaTime);
			m_ChildTransform.localScale = newScale;

			if (scalingStep) {
				if (m_ChildTransform.localScale.x >= targetScale.x - 0.1f &&
				    m_ChildTransform.localScale.y >= targetScale.y - 0.1f &&
				    m_ChildTransform.localScale.z >= targetScale.z - 0.1f) {

					targetScale = originalScale;
					scalingStep = false;
				}
			} else {
				if (m_ChildTransform.localScale.x <= targetScale.x + 0.1f &&
					m_ChildTransform.localScale.y <= targetScale.y + 0.1f &&
					m_ChildTransform.localScale.z <= targetScale.z + 0.1f) {

					targetScale = new Vector3(originalScale.x * scaleByVector.x, originalScale.y * scaleByVector.y, originalScale.z * scaleByVector.z);
					scalingStep = true;
				}
			}
		}
	}

	void OnReticleEnter(){
		// Change the material color
		if(allowColorChanges) m_ChildRenderer.material.color = highlightColor;

		shouldRotateHighlight = rotateOnHighlight;
		shouldPulseOnHighlight = scalePulseOnHighlight;
		isHighlighted = true;
	}

	void OnReticleExit(){
		// Change the material color
		m_ChildRenderer.material.color = m_OriginalColor;
		shouldRotateHighlight = false;
		shouldPulseOnHighlight = false;
		isHighlighted = false;
	}

	void OnReticleHit(){
		// Change the material color
		if (allowColorChanges) {
			m_ChildRenderer.material.color = triggerColor;

			// Invoke OnReticleExit method to reset the waypoint material color to its
			// Original color after 0.2 seconds.
			Invoke ("OnReticleExit", 0.2f);
		}

		_parentNetwork.SetNewDestination (m_ChildTransform.position, this);
	}

	public void EnableNeighbors(){
		if (neighbors != null) {
			foreach (Waypoint w in neighbors) {
				w.gameObject.SetActive (true);
			}
		}
	}

	public void DisableNeighbors(){
		if (neighbors != null) {
			foreach (Waypoint w in neighbors) {
				w.gameObject.SetActive (false);
			}
		}
	}

    public void DisableWaypoint() {

    }

}