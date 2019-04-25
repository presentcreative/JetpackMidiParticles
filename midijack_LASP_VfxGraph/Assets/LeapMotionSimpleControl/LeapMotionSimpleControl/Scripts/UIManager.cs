/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail.com
 *******************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using CustomUtils;
using System;

namespace LeapMotionSimpleControl
{
	[RequireComponent (typeof(Counter))]
	public class UIManager : MonoBehaviour
	{
		public Text CurrentGestureText;
		public Transform ListGesturePivot;
		public GameObject prefabSliderUI;

		GameManager _gameManager;
		Counter _countDownSlider;

		Dictionary<GestureManager.GestureTypes, Slider> _listSliders;

		GameManager.EndEvent _endEventCountDown;

		// Use this for initialization
		void Start ()
		{
			_countDownSlider = GetComponent<Counter> ();
		}
	
		// Update is called once per frame
		void Update ()
		{

		}

		#region UI

		public bool IsReady ()
		{
			return _countDownSlider.CurrentState == Counter.CounterState.STOP;
		}

		public void RegisterEventEndCountDown (GameManager.EndEvent end)
		{
			_endEventCountDown = end;
		}

		public void InitUI (GameManager manager)
		{
			_gameManager = manager;

			Dictionary<GestureManager.GestureTypes, object> listActiveGestures = _gameManager.GetCurrentActiveGestures ();
			_listSliders = new Dictionary<GestureManager.GestureTypes, Slider> ();
			foreach (KeyValuePair<GestureManager.GestureTypes, object> gesture in listActiveGestures) {
				GameObject go = GameObject.Instantiate (prefabSliderUI);
				go.transform.SetParent (ListGesturePivot);
				go.transform.localScale = Vector3.one;
				go.transform.localPosition = Vector3.zero;
				go.name = gesture.Key.ToString ();
				go.GetComponentInChildren<Text> ().text = go.name;

				_listSliders.Add (gesture.Key, go.GetComponentInChildren<Slider> ());
			}
		}

		public void UpdateTimerLoadingGesture (GestureManager.GestureTypes type, float percent)
		{
			Slider currentSlider = GetSliderBasedType (type);
			currentSlider.image.color = Color.green;
			currentSlider.value = percent;
		}

		public void UpdateSliderBlockingGesture (GestureManager.GestureTypes type, float timer)
		{
			Slider currentSlider = GetSliderBasedType (type);

			_countDownSlider.StartTimerUpdatePercentage (timer, () => {
				currentSlider.value = 0;
				currentSlider.image.color = Color.green;

				if (_endEventCountDown != null)
					_endEventCountDown (type);
			
			}, (float percent) => {
				currentSlider.image.color = Color.red;
				currentSlider.value = Mathf.Clamp01 (1 - percent);
			});

			CurrentGestureText.text = type.ToString ();
			Debug.Log ("Change Gesture: " + type.ToString ());
		}

		#endregion

		#region Addition

		Slider GetSliderBasedType (GestureManager.GestureTypes type)
		{
			if (_listSliders.ContainsKey (type)) {
				return _listSliders [type];
			}
			return _listSliders [0];
		}

		#endregion
	}
}