/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail.com
 *******************************************************/
using UnityEngine;
using System.Collections;
using Leap;
using Leap.Unity;

namespace LeapMotionSimpleControl
{
	public class GrabHand : BehaviorHand
	{
	
		public GameObject CurrentHoldingObj;

		Vector3 _savedBallPosition;
		bool _isHoldingBall = false;

		// Use this for initialization
		protected void Awake ()
		{
			base.Awake ();
			CurrentType = GestureManager.GestureTypes.Grab;
			specificEvent = grabBall;
			_savedBallPosition = CurrentHoldingObj.transform.position;
		}
	
		// Update is called once per frame
		void Update ()
		{
		}

		protected void FixedUpdate ()
		{
			base.FixedUpdate ();
			updateBall ();
		}

		protected override bool checkConditionGesture ()
		{
			Hand hand = GetCurrent1Hand ();
			if (hand != null) {
				if (isGrabHand (hand) && !_isHoldingBall)
					return true;
			}

			return false;
		}


		void grabBall ()
		{
			//Debug.Log ("Grab");
			_isHoldingBall = true;
		}

		void releaseBall ()
		{
			//Debug.Log ("Release");
			_isHoldingBall = false;
			CurrentHoldingObj.transform.position = _savedBallPosition;

		}

		void updateBall ()
		{
			bool isUpdating = false;
			if (_isHoldingBall) {
				Hand hand = GetCurrent1Hand ();
				if (hand != null) {
					if (isGrabHand (hand)) {
						if (CurrentHoldingObj != null) {
							CurrentHoldingObj.transform.position = UnityVectorExtension.ToVector3 (hand.PalmPosition + hand.PalmNormal.Normalized * 0.03f);
							isUpdating = true;
						}
					}
				}

				if (!isUpdating)
					releaseBall ();
			}
		}


	}
}