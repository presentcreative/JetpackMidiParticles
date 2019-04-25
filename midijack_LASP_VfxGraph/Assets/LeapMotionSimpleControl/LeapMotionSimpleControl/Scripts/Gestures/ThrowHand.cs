/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail.com
 *******************************************************/
using UnityEngine;
using System.Collections;
using Leap;
using Leap.Unity;

namespace LeapMotionSimpleControl
{
	public class ThrowHand : BehaviorHand
	{
		public GameObject prefabBall;
		public float forceToAdd = 400;

		// Use this for initialization
		protected void Awake ()
		{
			base.Awake ();
			CurrentType = GestureManager.GestureTypes.Throw;
			specificEvent = throwBall;
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		protected override bool checkConditionGesture ()
		{
			Hand hand = GetCurrent1Hand ();
			if (hand != null) {
				if (isPalmNormalSameDirectionWith (hand, UnityVectorExtension.ToVector3 (hand.PalmVelocity))
				   && !isStationary (hand)) {
					return true;
				}
			}
			return false;
		}

		void throwBall ()
		{
			Hand hand = GetCurrent1Hand ();
			if (hand != null) {
				GameObject go = GameObject.Instantiate (prefabBall);
				go.transform.position = UnityVectorExtension.ToVector3 (hand.PalmPosition);
				setupGravity (go);
				addForce (go, UnityVectorExtension.ToVector3 (hand.PalmVelocity * forceToAdd));
			}
		}

		void setupGravity (GameObject go)
		{
			go.GetComponent<Rigidbody> ().useGravity = true;
		}

		void addForce (GameObject go, Vector3 force)
		{
			go.GetComponent<Rigidbody> ().AddForce (force);
		}
	}

}