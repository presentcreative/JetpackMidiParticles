/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail.com
 *******************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;

namespace LeapMotionSimpleControl
{
	public class RotateHand : BehaviorHand
	{
		public Transform cube;
		public float speedRotate = 10f;
		private Vector3 _targetRot;
		private Vector3 _fromRot;
		private float _delta;

		// timer 
		private float _timer;
		// Use this for initialization
		protected void Awake ()
		{
			base.Awake ();
			CurrentType = GestureManager.GestureTypes.RotateVert;
			specificEvent = onReceiveRotateAction;
			_targetRot = cube.localEulerAngles;
			CheckingTimeBeforeToggle = 0;
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (!isSameDirection (cube.eulerAngles, _targetRot) && _timer > 0) {
				rotateCube ();
				_timer -= Time.deltaTime;
				_isBlock = true;
			} else {
				_isBlock = false;
			}
		}

		protected override bool checkConditionGesture ()
		{
			if (!_isBlock) {
				Hand hand = GetCurrent1Hand ();
				if (hand != null) {
					if (isOpenFullHand (hand)) {
						if (isMoveLeft (hand) || isMoveRight (hand)) {
							CurrentType = GestureManager.GestureTypes.RotateHonz;
							return true;
						} else if (isMoveUp (hand) || isMoveDown (hand)) {
							CurrentType = GestureManager.GestureTypes.RotateVert;
							return true;
						} 
					}
				}
			}
			return false;
		}

		void onReceiveRotateAction(){
			
			Hand hand = GetCurrent1Hand ();
			Vector3 delta; 
			if (CurrentType == GestureManager.GestureTypes.RotateHonz) {
				_delta = -getHandVelocity (hand).x;
				delta = new Vector3 (0,  -getHandVelocity (hand).x * 100, 0);
			} else {
				_delta = getHandVelocity (hand).y;
				delta = new Vector3 (getHandVelocity (hand).y * 100, 0, 0);
			}

			_fromRot = cube.localEulerAngles;
			_targetRot = clampRotVector(cube.eulerAngles + delta);
			_timer = 2f;
			//Debug.Log ("_targetRot from " + cube.eulerAngles + " to " + _targetRot);
		}

		void rotateCube(){
			// rotate
			cube.Rotate((CurrentType == GestureManager.GestureTypes.RotateHonz) ? Vector3.up : Vector3.right,
									Mathf.Sign (_delta) * Time.deltaTime * speedRotate, Space.World);

			float temp = Mathf.Sign (_delta) * Time.deltaTime * speedRotate;
			//Debug.Log ("current " + cube.eulerAngles + " || " + temp.ToString() + " || " + 
			//	Vector3.Angle(cube.eulerAngles , _targetRot));
		}

		bool isSameDirection(Vector3 a, Vector3 b){
			return Vector3.Distance (a , b) < 1f;
		}

		Vector3 clampRotVector(Vector3 a){
			float x = a.x % 360;
			float y = a.y % 360;
			float z = a.z % 360;

			if (x < 0) {
				x = 360 + x;
			} 
			if (x == 360) {
				x = 0;
			} 
			if (y < 0) {
				y = 360 + y;
			}
			if (y == 360) {
				y = 0;
			} 
			if (z < 0) {
				z = 360 + z;
			}
			if (z == 360) {
				z = 0;
			} 
			return new Vector3 (x, y, z);
		}
	}
}
