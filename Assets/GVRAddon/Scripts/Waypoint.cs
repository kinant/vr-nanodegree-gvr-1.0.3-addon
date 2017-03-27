﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

	[SerializeField]
	private GVRInteractiveItem m_GVRInteractiveItem;

	// Color properties
	public bool allowColorChanges = false;
	public Color highlightColor;
	public Color triggerColor;

	private Renderer m_Renderer;
	private Color m_OriginalColor;

	// Rotation properties
	public bool rotate = false;
	public bool rotateOnHighlight = false;
	public float rotationSpeed = 1.0f;
	public Vector3 rotationAngle = Vector3.zero;

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
	bool shouldPulse;
	bool shouldPulseOnHighlight;

	private Transform m_Transform;

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
		if (GetComponent<Renderer> ()) {
			m_Renderer = GetComponent<Renderer> ();
			m_OriginalColor = m_Renderer.material.color;
		}
		if (rotate) {
			shouldRotate = true;
		}

		m_Transform = transform;
		originalScale = m_Transform.localScale;
		targetScale = new Vector3(originalScale.x * scaleByVector.x, originalScale.y * scaleByVector.y, originalScale.z * scaleByVector.z);
	}

	void Update(){
		shouldRotate = rotate;
		shouldPulse = scalePulse;

		if (shouldRotate && (!isHighlighted && !shouldRotateHighlight) || (shouldRotateHighlight && isHighlighted)) {
			m_Transform.Rotate (rotationAngle * rotationSpeed * Time.deltaTime);
		}
			
		if (scalePulse && (!isHighlighted && !shouldPulseOnHighlight) || (shouldPulseOnHighlight && isHighlighted)) {
			currScale = m_Transform.localScale;

			Vector3 newScale = Vector3.Lerp (currScale, targetScale, pulseSpeed * Time.deltaTime);
			m_Transform.localScale = newScale;

			if (scalingStep) {
				if (m_Transform.localScale.x >= targetScale.x - 0.1f &&
				    m_Transform.localScale.y >= targetScale.y - 0.1f &&
				    m_Transform.localScale.z >= targetScale.z - 0.1f) {

					targetScale = originalScale;
					scalingStep = false;
				}
			} else {
				if (m_Transform.localScale.x <= targetScale.x + 0.1f &&
					m_Transform.localScale.y <= targetScale.y + 0.1f &&
					m_Transform.localScale.z <= targetScale.z + 0.1f) {

					targetScale = new Vector3(originalScale.x * scaleByVector.x, originalScale.y * scaleByVector.y, originalScale.z * scaleByVector.z);
					scalingStep = true;
				}
			}
		}
	}

	void OnReticleEnter(){
		// Change the material color
		if(allowColorChanges) m_Renderer.material.color = highlightColor;

		shouldRotateHighlight = rotateOnHighlight;
		shouldPulseOnHighlight = scalePulseOnHighlight;
		isHighlighted = true;
	}

	void OnReticleExit(){
		// Change the material color
		m_Renderer.material.color = m_OriginalColor;
		shouldRotateHighlight = false;
		shouldPulseOnHighlight = false;
		isHighlighted = false;
	}

	void OnReticleHit(){
		// Change the material color
		if (allowColorChanges) {
			m_Renderer.material.color = triggerColor;

			// Invoke OnReticleExit method to reset the waypoint material color to its
			// Original color after 0.2 seconds.
			Invoke ("OnReticleExit", 0.2f);
		}
	}
}