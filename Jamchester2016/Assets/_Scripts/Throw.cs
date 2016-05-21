using System;
using UnityEngine;
using System.Collections;
using System.Linq;

namespace Assets._Scripts
{

	[RequireComponent(typeof(SteamVR_TrackedObject))]
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(Collider))]
	public class Throw : MonoBehaviour
	{
		Collider attachPoint;

		SteamVR_TrackedObject trackedObj;

		GameObject heldObject;

		void Awake()
		{
			enabled = SteamVR.active;

			if(!SteamVR.active)
				return;

			trackedObj = GetComponent<SteamVR_TrackedObject>();
			attachPoint = GetComponent<Collider>();
			attachPoint.isTrigger = true;
			attachPoint.enabled = false;
		}

		void OnTriggerEnter(Collider other)
		{
			var otherRigidBody = other.GetComponent<Rigidbody>();

			if(otherRigidBody == null || heldObject != null)
				return;

			GameObject myObject = otherRigidBody.gameObject;
			myObject.transform.parent = transform;


			//myObject.transform.SetParent(this.transform, false);

			foreach(Rigidbody rb in myObject.GetComponentsInChildren<Rigidbody>())
				rb.useGravity = false;

			heldObject = myObject;
			
		}

		void FixedUpdate()
		{

			var device = SteamVR_Controller.Input((int) trackedObj.index);

			attachPoint.enabled = device.GetTouch(SteamVR_Controller.ButtonMask.Trigger);

			if(heldObject != null && device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
			{
				var myObject = heldObject;
				heldObject = null;

				myObject.transform.SetParent(null, true);
				var rigidBodies = myObject.GetComponentsInChildren<Rigidbody>();

				foreach(Rigidbody rb in rigidBodies)
					rb.useGravity = true;

				var connectedRigidBody = rigidBodies.FirstOrDefault();

				if(connectedRigidBody == null)
					return;

				// We should probably apply the offset between trackedObj.transform.position
				// and device.transform.pos to insert into the physics sim at the correct
				// location, however, we would then want to predict ahead the visual representation
				// by the same amount we are predicting our render poses.

				var origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
				if(origin != null)
				{
					connectedRigidBody.velocity = origin.TransformVector(device.velocity);
					connectedRigidBody.angularVelocity = origin.TransformVector(device.angularVelocity);
				}
				else
				{
					connectedRigidBody.velocity = device.velocity;
					connectedRigidBody.angularVelocity = device.angularVelocity;
				}

				connectedRigidBody.maxAngularVelocity = connectedRigidBody.angularVelocity.magnitude;
			}
		}
	}

}
