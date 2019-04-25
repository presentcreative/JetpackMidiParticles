/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail.com
 *******************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;

namespace LeapMotionSimpleControl
{
	public class ClapHand : BehaviorHand
	{

		// Use this for initialization
		protected void Awake ()
		{
			base.Awake ();
			CurrentType = GestureManager.GestureTypes.ClapHand;
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		protected override bool checkConditionGesture ()
		{
			List<Hand> currentList = GetCurrent2Hands ();
			if (currentList != null) {
				Hand leftHand = currentList [0].IsLeft ? currentList [0] : currentList [1];
				Hand rightHand = currentList [0].IsRight ? currentList [0] : currentList [1];
				if (leftHand == null || rightHand == null) {
					Debug.Log ("Please present the correct left hand and right hand");
				} else {
					if (isOpenFullHand (leftHand) && isOpenFullHand (rightHand)
					  && isOppositeDirection (leftHand.PalmNormal, rightHand.PalmNormal)
					  && isOppositeDirection (leftHand.PalmVelocity, rightHand.PalmVelocity)
					  && isHandMoveForward (leftHand) && isHandMoveForward (rightHand)) {
						return true;
					}

				}
			}
			return false;
		}


	}
}
