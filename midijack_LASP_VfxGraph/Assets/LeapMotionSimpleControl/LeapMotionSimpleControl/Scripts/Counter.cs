/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail.com
 *******************************************************/
using UnityEngine;
using System.Collections;

namespace CustomUtils
{
	public class Counter : MonoBehaviour
	{

		public enum CounterState
		{
			RUN,
			STOP,
			PAUSE
		}

		public CounterState CurrentState
		{
			get
			{
				return currentState;
			}
		}


		public delegate void EndTimer();

		public delegate void EndEverySeconds(int secs);

		public delegate void UpdatingPercentage(float percent);

		CounterState currentState = CounterState.STOP;
		float step;
		float maxTimer;
		float timer;
		float preTimer;

		EndTimer _endTimerFunction;
		EndEverySeconds _endEverySeconds;
		UpdatingPercentage _updating;

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void FixedUpdate()
		{
			switch (CurrentState)
			{
			case CounterState.RUN:

				timer -= Time.fixedDeltaTime * step;

				if (_updating != null)
				{
					_updating(1 - timer * 1.0f / maxTimer);
				}

				if (Mathf.Abs(timer - preTimer) >= 1)
				{
					preTimer = timer;
					if (_endEverySeconds != null)
					{
						_endEverySeconds(Mathf.RoundToInt(preTimer));
					}
				}

				if (timer < 0)
				{
					timer = maxTimer;
					currentState = CounterState.STOP;

					if (_endTimerFunction != null)
					{
						_endTimerFunction();
					}
				}
				break;
			case CounterState.PAUSE:
				break;
			case CounterState.STOP:
				preTimer = timer = maxTimer;
				break;
			}
		}

		public void StartTimer(float _maxTimer, EndTimer endFunc)
		{

			step = 1;
			timer = maxTimer = _maxTimer;
			_endTimerFunction = endFunc;
			_endEverySeconds = null;
			_updating = null;
			currentState = CounterState.RUN;

		}

		public void StartTimerUpdateSeconds(float _maxTimer, EndTimer endFunc, EndEverySeconds endSecs = null)
		{

			step = 1;
			timer = maxTimer = _maxTimer;
			_endTimerFunction = endFunc;
			_endEverySeconds = endSecs;
			_updating = null;
			currentState = CounterState.RUN;

		}

		public void StartTimerUpdatePercentage(float _maxTimer, EndTimer endFunc, UpdatingPercentage updatingFunc = null)
		{

			step = 1;
			timer = maxTimer = _maxTimer;
			_endTimerFunction = endFunc;
			_endEverySeconds = null;
			_updating = updatingFunc;
			currentState = CounterState.RUN;

		}

		public void StopTimer()
		{
			currentState = CounterState.STOP;
			_endTimerFunction = null;
			_endEverySeconds = null;
			//      Debug.Log ("stop timer ");

		}

		public void PauseTimer()
		{
			if (currentState == CounterState.RUN)
			{
				currentState = CounterState.PAUSE;
				//  Debug.Log("Pause");
			}
		}

		public void ContinueTimer()
		{
			if (currentState == CounterState.PAUSE)
			{
				currentState = CounterState.RUN;
				//  Debug.Log("Cont");

			}
		}
	}
}