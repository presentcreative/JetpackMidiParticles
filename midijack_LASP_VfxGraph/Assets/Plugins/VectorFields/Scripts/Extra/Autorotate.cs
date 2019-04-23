using UnityEngine;
using System.Collections;

public class Autorotate : MonoBehaviour
{
	public Vector3 rotationSpeed;

	private Vector3 rotationPos = Vector3.zero;

	void Update()
	{
		rotationPos += rotationSpeed * Time.deltaTime;
		transform.rotation = Quaternion.Euler(rotationPos);
	}
}
