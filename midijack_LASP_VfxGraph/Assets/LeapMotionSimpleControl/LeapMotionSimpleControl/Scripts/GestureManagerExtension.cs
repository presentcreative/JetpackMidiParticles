using UnityEngine;
using System.Collections;
using Leap.Unity;
namespace LeapMotionSimpleControl{
	// use for extension example only 
	public class GestureManagerExtension : GestureManager {
		GameManager _gameManager;



		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public void InitGesture (GameManager manager)
		{
			_gameManager = manager;
			_leapHandProvider = _gameManager.GetTransformGestureManagerBasedMode ().GetComponentInChildren<LeapProvider> ();

			initGesture ();

		}

		public override bool ReceiveEvent (GestureTypes type)
		{
			if (_gameManager.IsReadyUI ()) {
				_currentType = type;
				_gameManager.UpdateUIBlockingGesture (type, TimeBetween2Gestures, unBlockGesture);
				_gameManager.NavigateMenu (type);
				return true;
			} 
			return false;
		}

		public override void LoadingGestureProgress (GestureTypes type, float percent)
		{
			if(_gameManager != null)
				_gameManager.UpdateUILoadingGesture (type, percent);
		}

		public GameManager.GameMode GetCurrentGameMode ()
		{
			return _gameManager.CurrentMode;
		}
	}
}
