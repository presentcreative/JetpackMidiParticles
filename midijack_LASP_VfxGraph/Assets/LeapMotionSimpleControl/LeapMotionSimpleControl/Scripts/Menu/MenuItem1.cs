/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail.com
 *******************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace LeapMotionSimpleControl
{
	public class MenuItem1: MonoBehaviour
	{

		public Text text;
		public Image circle;
		public Button button;

		MenuManager _manager;

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public void Init (MenuManager manager)
		{
			_manager = manager;
		}

		public string GetText ()
		{
			return text.text;
		}

		public void SetAlphaChanel (float alpha)
		{
			circle.color = new Color (circle.color.r, circle.color.g, circle.color.b, alpha);
			text.color = new Color (text.color.r, text.color.g, text.color.b, alpha);
		}

		public void SetCircleProgress (float percent)
		{
			circle.fillAmount = percent;
		}

		public void OnTriggerEnter (Collider collider)
		{
			_manager.Select ();
		}

		public void OnTriggerExit (Collider collider)
		{
			_manager.DeSelect ();

		}

	}
}