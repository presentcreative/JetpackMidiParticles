/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail.com
 *******************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Leap.Unity;
using UnityEngine.UI;
using CustomUtils;

namespace LeapMotionSimpleControl
{
	public class GestureManager : MonoBehaviour
	{

		public enum GestureTypes
		{
			SwipingLeft,
			SwipingRight,
			SwipingUp,
			SwipingDown,
			ThumbUp,
			ThumbDown,
			Fist,
			FaceUp,
			FaceDown,
			ClapHand,
			Grab,
			Throw, 
			RotateHonz,
			RotateVert, 
			ZoomHand
		}

		protected GestureTypes _currentType;

		public GestureTypes GteCurrentGestureType ()
		{
			return _currentType;
		}

		public LeapProvider _leapHandProvider;

		public LeapProvider GetLeapHand ()
		{
			return _leapHandProvider;
		}

		public float TimeBetween2Gestures;

		public Dictionary<GestureTypes, object> _listActiveGestures;

		public Dictionary<GestureTypes, object> GetCurrentActiveGestures ()
		{
			return _listActiveGestures;
		}

		public Transform player;

		// Use this for initialization
		void Start ()
		{
			Invoke("initGesture",3f);
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		protected void initGesture(){
			_listActiveGestures = new Dictionary<GestureTypes, object> ();
			foreach (Transform t in transform) {
				BehaviorHand hand = t.GetComponent<BehaviorHand> ();
				if (hand != null) {
					hand.SetPlayerTransform(player);
					foreach (GestureTypes type in Enum.GetValues(typeof(GestureTypes))) {
						if (hand.GetCurrentType () == type && !_listActiveGestures.ContainsKey(type)) {
							if (type == GestureTypes.RotateHonz || type == GestureTypes.RotateVert) {
								_listActiveGestures.Add (GestureTypes.RotateHonz, t.GetComponent<BehaviorHand> () as object);
								_listActiveGestures.Add (GestureTypes.RotateVert, t.GetComponent<BehaviorHand> () as object);
							} else {
								_listActiveGestures.Add (type, t.GetComponent<BehaviorHand> () as object);
							}
						}
					}
					t.GetComponent<BehaviorHand> ().Init (this);
				}
			}
		}

		public virtual bool ReceiveEvent (GestureTypes type)
		{
			Debug.Log ("ReceiveEvent " + type.ToString ());
			_currentType = type;
			Invoke ("unBlockCurrentGesture", TimeBetween2Gestures);
			return true;
		}

		protected void unBlockCurrentGesture ()
		{
			BehaviorHand behavior = (BehaviorHand)_listActiveGestures [_currentType];
			behavior.UnBlockGesture ();
		}

		protected void unBlockGesture (GestureTypes type)
		{
			BehaviorHand behavior = (BehaviorHand)_listActiveGestures [type];
			behavior.UnBlockGesture ();
		}

		public virtual void LoadingGestureProgress (GestureTypes type, float percent)
		{
			
		}
	}
}