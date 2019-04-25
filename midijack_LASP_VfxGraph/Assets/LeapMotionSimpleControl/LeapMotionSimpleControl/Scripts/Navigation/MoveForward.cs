using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveForward : MonoBehaviour {

	public Image loadingBar;
	private float _speedToMove = 0.5f; // seconds 
	private Navigation _manager;
	private Vector3 _preDir;

	// Use this for initialization
	void Start () {
		loadingBar.fillAmount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		updateLoadingBar (checkMove());
	
	}

	public void SetUp(Navigation m){
		_preDir = m.GetDirection ();
		_manager = m;
	}

	void updateLoadingBar(bool isMove){
		if (isMove) {
			loadingBar.fillAmount += Time.deltaTime * _speedToMove;
			if (loadingBar.fillAmount >= 1) {
				_manager.StartMovement ();
			}
		} else {
			loadingBar.fillAmount = 0;
			_manager.StopMovement ();
		}
	}

	bool checkMove(){
		Vector3 dir = _manager.GetDirection ();
		bool isSameDir = Vector3.Angle(dir,_preDir) < 1;
		_preDir = dir;
		return isSameDir;
	}
}
