using UnityEngine;
using System.Collections;

public class Navigation : MonoBehaviour {
	public GameObject player;

	public float rotSpeed;
	public float moveSpeed;
	public RotateArrow leftArrow;
	public RotateArrow rightArrow;
	public MoveForward moveForward;

	private int _isRotation = 0; // 0:stop, -1:left, 1:right 
	private int _isMoving = 0; // 0:stop, 1: move

	// Use this for initialization
	void Start () {
		leftArrow.SetUp (this);
		rightArrow.SetUp (this);
		moveForward.SetUp (this);
	}
	
	// Update is called once per frame
	void Update () {
		updateRot ();
		updateMovement ();
	}

	public void StartRotation(bool isLeft){
		_isRotation = (isLeft == true) ? -1 : 1;
		//player.transform.localEulerAngles += new Vector3(0, (isLeft == true) ? 1 : -1 * rotSpeed, 0);
	}

	public void StopRotation(){
		_isRotation = 0;
	}

	public void StartMovement(){
		_isMoving = 1;
	}

	public void StopMovement(){
		_isMoving = 0;
	}

	public Vector3 GetDirection(){
		return player.transform.forward;
	}

	void updateRot()
	{
		Vector3 euler = player.transform.localRotation.eulerAngles;
		euler.y += _isRotation * rotSpeed; 
		player.transform.localRotation = Quaternion.Euler(euler);
	}

	void updateMovement()
	{
		player.transform.position += player.transform.forward * _isMoving * Time.deltaTime * moveSpeed;
	}
}
