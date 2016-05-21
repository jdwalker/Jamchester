using UnityEngine;
using System.Collections;
using UnityEngine.VR;
using System;
using System.Linq;

public class CameraStartup : MonoBehaviour {

	// Use this for initialization
	public Camera headCamera;
	public Camera sceneCamera;
	Boolean headState;

	

	void Awake()
	{
		Boolean VRDeviceIsPresent = SteamVR.active;

		headState = MonitorCameraOnHead(!VRDeviceIsPresent);

		if(!VRDeviceIsPresent)
		{
			var head = GameObject.Find("Head");
			if(head != null)
				head.transform.Translate(new Vector3(0, 0.8F, 0), Space.Self);
		}

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

		if(headCamera != null)
			headCamera.enabled = head;

		return head;
	}
}
