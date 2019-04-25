using UnityEngine;
using System.Collections;

public class RotateArrow : MonoBehaviour {

	public enum DirectionRot
	{
		Left, 
		Right
	}
	public GameObject fullArrow;
	public GameObject outlineArrow;

	public DirectionRot direction;

	private bool _isTouch;
	private Navigation _manager;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Touch(){
		_isTouch = true;
		fullArrow.SetActive (_isTouch);
		_manager.StartRotation (direction == DirectionRot.Left);
	}

	public void UnTouch(){
		_isTouch = false;
		fullArrow.SetActive (_isTouch);
		_manager.StopRotation ();
	}

	public void SetUp(Navigation nav)
	{
		_manager = nav;	
	}

	void OnTriggerEnter(Collider other) {
		Touch ();
	}

	void OnTriggerExit(Collider other) {
		UnTouch ();
	}
}
