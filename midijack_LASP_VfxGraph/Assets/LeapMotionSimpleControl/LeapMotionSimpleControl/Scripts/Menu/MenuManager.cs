/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail.com
 *******************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using CustomUtils;

namespace LeapMotionSimpleControl
{
	[RequireComponent (typeof(Counter))]
	public class MenuManager : MonoBehaviour
	{

		public Transform content;
		public GameObject leftArrow;
		public GameObject rightArrow;
		public Text title;

		List<MenuItem1> _listContent;
		Counter _counterTransition;
		float _distanceBetween2Items;
		int _currentIdx;
		bool _isTransition;

		// Use this for initialization
		void Start ()
		{
			init ();
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void init ()
		{
			_counterTransition = GetComponent<Counter> ();

			GridLayoutGroup layout = content.GetComponent<GridLayoutGroup> ();
			_distanceBetween2Items = layout.cellSize.x + layout.spacing.x;

			_listContent = new List<MenuItem1> ();
			_currentIdx = 0;

			foreach (Transform t in content) {
				MenuItem1 item = t.GetComponent<MenuItem1> ();
				if (item != null) {
					item.Init (this);
					_listContent.Add (item);
				}
			}


		}

		void transition (int fromIdx, int toIdx)
		{
			if (fromIdx >= 0 && fromIdx < _listContent.Count
			   && toIdx >= 0 && toIdx < _listContent.Count) {
			
				float originalX = content.localPosition.x;
				int sign = ((fromIdx < toIdx) ? -1 : 1);
				if (_counterTransition.CurrentState == Counter.CounterState.STOP) {

					_counterTransition.StartTimerUpdatePercentage (0.75f, () => {
						_currentIdx -= sign;
						_currentIdx = Mathf.Clamp (_currentIdx, 0, _listContent.Count - 1);

						content.localPosition = new Vector3 (originalX + sign * _distanceBetween2Items, 0);
			
						_isTransition = false;
						leftArrow.SetActive (_currentIdx != 0);
						rightArrow.SetActive (_currentIdx != _listContent.Count - 1);
					}, (float percent) => {
						_isTransition = true;
						_listContent [fromIdx].SetAlphaChanel (1 - percent);
						_listContent [toIdx].SetAlphaChanel (percent);

						float delta = Mathf.Lerp (originalX, originalX + sign * _distanceBetween2Items, percent);
						content.localPosition = new Vector3 (delta, 0);
					});
				}
			}
		}

		public void SwipeLeft ()
		{
			transition (_currentIdx, _currentIdx + 1);
		}

		public void SwipeRight ()
		{
			transition (_currentIdx, _currentIdx - 1);
		}

		public void Select ()
		{
			if (_counterTransition.CurrentState == Counter.CounterState.STOP) {
				_counterTransition.StartTimerUpdatePercentage (1, () => {
					title.text = _listContent [_currentIdx].GetText ();
				}, (float percent) => {
					_listContent [_currentIdx].SetCircleProgress (percent);
				});
			}
		}

		public void DeSelect ()
		{
			if (!_isTransition) {
				_counterTransition.StopTimer ();
				_listContent [_currentIdx].SetCircleProgress (0);
			}
		}
	}
}