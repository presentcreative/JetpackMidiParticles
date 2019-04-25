/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail.com
 *******************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LeapMotionSimpleControl
{
	public class GameManager : MonoBehaviour
	{

		public GestureManagerExtension gestureManager;
		public UIManager uiManager;
		public MenuManager menuManager;

		public delegate void EndEvent (GestureManager.GestureTypes type);

		public enum GameMode
		{
			Desktop,
			VR
		}

		public GameMode CurrentMode;
		public Transform VRCamera;
		public Transform DesktopCamera;


		// Use this for initialization
		void Awake ()
		{
			init ();
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void init ()
		{
			if (DesktopCamera == null) {
				CurrentMode = GameMode.VR;
				Debug.Log ("Current scene doesn't have Desktop mode");
			} else if (VRCamera == null) {
				CurrentMode = GameMode.Desktop;
				Debug.Log ("Current scene doesn't have VR mode");
			}

			if (DesktopCamera != null)
				DesktopCamera.gameObject.SetActive (CurrentMode == GameMode.Desktop);
			if (VRCamera != null)
				VRCamera.gameObject.SetActive (CurrentMode == GameMode.VR);

			gestureManager.InitGesture (this);
			uiManager.InitUI (this);
		}

		#region Gesture

		public Dictionary<GestureManager.GestureTypes, object> GetCurrentActiveGestures ()
		{
			return gestureManager.GetCurrentActiveGestures ();
		}

		public Transform GetTransformGestureManagerBasedMode ()
		{
			return (CurrentMode == GameMode.Desktop) ? DesktopCamera : VRCamera;
		}

		#endregion

		#region UI

		public bool IsReadyUI ()
		{
			return uiManager.IsReady ();
		}

		public void UpdateUIBlockingGesture (GestureManager.GestureTypes type, float timer, EndEvent endEvent)
		{
			uiManager.RegisterEventEndCountDown (endEvent);
			uiManager.UpdateSliderBlockingGesture (type, timer);
		}

		public void UpdateUILoadingGesture (GestureManager.GestureTypes type, float percent)
		{
			uiManager.UpdateTimerLoadingGesture (type, percent);
		}

		#endregion

		#region Menu

		public void NavigateMenu (GestureManager.GestureTypes type)
		{
			if (menuManager != null) {
				if (type == GestureManager.GestureTypes.SwipingLeft) {
					menuManager.SwipeLeft ();
				} else if (type == GestureManager.GestureTypes.SwipingRight) {
					menuManager.SwipeRight ();
				} 
			}
		}

		#endregion
	}
}