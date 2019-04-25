/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail.com
 *******************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;

namespace LeapMotionSimpleControl
{
	public class ZoomHand : BehaviorHand
	{
		public Transform cube;
		public float speedZoom = 2f;

		private float _preGrabStrength;
		private bool _isZoom;
		// Use this for initialization
		protected void Awake ()
		{
			base.Awake ();
			CurrentType = GestureManager.GestureTypes.ZoomHand;
			specificEvent = onReceiveZoomAction;

		}
	
		// Update is called once per frame
		void Update ()
		{
			if (_isZoom) {
				scaleCube ();
			}
		}

		protected override bool checkConditionGesture ()
		{
			Hand hand = GetCurrent1Hand ();
			if (hand != null) {
				if (isStationary (hand) && isOpenFullHand (hand)) {
					return true;
				} 

				if (!isStationary (hand)) {
					_isZoom = false;
				}
			} else {
				_isZoom = false;
			}

			return false;
		}

		void onReceiveZoomAction(){
			_isZoom = true;
			_preGrabStrength = 0;
		}

		void scaleCube(){
			Hand hand = GetCurrent1Hand ();
			if (hand != null) {
				//Debug.Log ("scale " + (hand.GrabStrength - _preGrabStrength).ToString() );
				float scale = Mathf.Clamp ((_preGrabStrength-hand.GrabStrength)  * speedZoom + cube.localScale.x , 0.25f, 2f);
				_preGrabStrength = hand.GrabStrength;
				cube.localScale = new Vector3(scale,scale,scale);
			}
		}



	}
}
