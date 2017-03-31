using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNetworkNavigation : MonoBehaviour {

	[SerializeField] private Waypoint[] Waypoints;
	public bool DisableWaypointNeighbors = false;

	private enum MovementType {
		Teleport, Translate
	}

	[SerializeField]
	private MovementType movementType = MovementType.Teleport;

	public float movementSpeed = 10.0f;

	private Waypoint currentWaypoint;
	private Waypoint previousWaypoint;

	private Vector3 cameraDestination;
	private bool isMoving = false;

	private string _ErrorTag = "WAYPOINT NETWORK ERROR: ";

	private GameObject _camera;
	private Vector3 cameraPosition;

	// Use this for initialization
	void Start () {

		if (DisableWaypointNeighbors) {
			DisableAllWaypoints ();

			if (Waypoints != null && Waypoints.Length > 0) {
				if (Waypoints [0] != null) {
					Waypoints [0].gameObject.SetActive (true);
				} else {
					Debug.LogError (_ErrorTag + "First waypoint is not set in array. Check the Waypoints array first element is set.");
				}
			} else {
				Debug.LogError (_ErrorTag + "No waypoints added to array!");
			}
		}

		_camera = GameObject.Find ("CameraRig");

		if (_camera == null) {
			Debug.LogError (_ErrorTag + "No CameraRig found. Please add one to the scene.");
		} else {
			cameraPosition = _camera.transform.position;
			cameraDestination = cameraPosition;
		}
	}
	
	// Update is called once per frame
	void Update () {

		// move the camera to the destination
		if(cameraPosition == cameraDestination){
			isMoving = false;

            if (currentWaypoint != null)
            {
                currentWaypoint.gameObject.SetActive(false);
            }
			return;
		}

		if (movementType == MovementType.Teleport) {
			cameraPosition = cameraDestination;
			_camera.transform.position = cameraDestination;
			isMoving = false;
		} else {

			isMoving = true;

			Vector3 newPos = Vector3.Lerp(cameraPosition, cameraDestination, movementSpeed * Time.deltaTime);
			cameraPosition = newPos;
			_camera.transform.position = newPos;

			if(Vector3.Distance(cameraPosition, cameraDestination) <= 0.1f){
				Debug.Log("Destination Reached!");
				_camera.transform.position = cameraDestination;
				cameraPosition = cameraDestination;
			}
		}
	}

	void DisableAllWaypoints(){
		if (Waypoints != null) {
			foreach (Waypoint w in Waypoints) {
				w.gameObject.SetActive (false);
			}
		}
	}

	public void SetNewDestination(Vector3 newDest, Waypoint w){

		if (isMoving)
			return;

        cameraDestination = newDest;

		if (DisableWaypointNeighbors) {
			if (currentWaypoint != null)
				currentWaypoint.DisableNeighbors ();

			w.EnableNeighbors ();
			currentWaypoint = w;
			// w.gameObject.SetActive (false);
		}
	}
}
