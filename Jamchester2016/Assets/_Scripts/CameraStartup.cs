using UnityEngine;
using System.Collections;
using UnityEngine.VR;
using System;
using System.Linq;

public class CameraStartup : MonoBehaviour {

	// Use this for initialization
	public Camera headVrCamera;
	public Camera headMouseCamera;
	public Camera sceneCamera;

	Boolean headState;

	void Awake()
	{
		Boolean VRDeviceIsPresent = SteamVR.active;


		foreach(var go in GameObject.FindGameObjectsWithTag("VrOnly"))
		{
			go.SetActive(VRDeviceIsPresent);
		}

		foreach(var go in GameObject.FindGameObjectsWithTag("MouseOnly"))
		{
			go.SetActive(!VRDeviceIsPresent);
		}

		headState = MonitorCameraOnHead(head: true);

	}



	// Update is called once per frame
	void Update()
	{
		if(Input.GetButtonDown("Swap"))
			headState = MonitorCameraOnHead(!headState);

	}

	public Boolean MonitorCameraOnHead(Boolean head)
	{
		if(sceneCamera != null)
			sceneCamera.enabled = !head;

		if(headMouseCamera != null)
			headMouseCamera.enabled = head;

		if(headVrCamera != null)
			headVrCamera.enabled = head;

		return head;
	}
}
