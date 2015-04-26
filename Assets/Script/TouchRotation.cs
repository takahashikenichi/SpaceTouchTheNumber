using UnityEngine;
using System.Collections;

public class TouchRotation : MonoBehaviour
{
	public Vector3 angle;
	public float stopAngleY = 90;
	//	bool mouseDown = false;
	TextMesh numberGUIText;
	int score = 1;
	string number = "1";

	public enum RotateState
	{
		NONE,
		TOUCH_TRUE,
		TOUCH_FALSE_ROTATING,
		TOUCH_FALSE_RETURN,
		TOUCH_ALL_RETURN,
	}

	public RotateState rotateState;

	public void SetNumber (string numberString)
	{
		number = numberString;
		score = int.Parse (number); // 文字列を数値に変換
	}

	void Awake ()
	{
		foreach (Transform child in transform) {
			if (child.name == "Number") {
				numberGUIText = (TextMesh)child.GetComponent (typeof(TextMesh));
			}
		}
	}
	// Use this for initialization
	void Start ()
	{
		rotateState = RotateState.NONE;
	}
	// Update is called once per frame
	void Update ()
	{
		if (numberGUIText != null) {
			numberGUIText.text = number;
		}

		switch (rotateState) {
		case RotateState.TOUCH_TRUE: 
			if (numberGUIText != null) {
				numberGUIText.text = number;
				numberGUIText.color = Color.yellow;
			}

			if (stopAngleY < 0) {
				if (transform.rotation.eulerAngles.y > 360 + stopAngleY || transform.rotation.eulerAngles.y == 0) {
					transform.Rotate (angle * Time.deltaTime);
				} else {
					// 指定した値でぴったり止める
					transform.eulerAngles = new Vector3 (0, 360 + stopAngleY, 0);
					rotateState = RotateState.NONE;
				}
			} else {
				if (transform.rotation.eulerAngles.y < stopAngleY) {
					transform.Rotate (angle * Time.deltaTime);	
				} else {
					// 指定した値でぴったり止める
					transform.eulerAngles = new Vector3 (0, stopAngleY, 0);
					rotateState = RotateState.NONE;
				}
			}
			break;
		case RotateState.TOUCH_FALSE_ROTATING:
			if (numberGUIText != null) {
				numberGUIText.text = number;
				numberGUIText.color = Color.blue;
			}

			if (stopAngleY < 0) {
				if (transform.rotation.eulerAngles.y > 360 + stopAngleY || transform.rotation.eulerAngles.y == 0) {
					transform.Rotate (angle * Time.deltaTime);
				} else {
					// 指定した値でぴったり止める
					transform.eulerAngles = new Vector3 (0, 360 + stopAngleY, 0);
					rotateState = RotateState.TOUCH_FALSE_RETURN;
				}
			} else {
				if (transform.rotation.eulerAngles.y < stopAngleY) {
					transform.Rotate (angle * Time.deltaTime);	
				} else {
					// 指定した値でぴったり止める
					transform.eulerAngles = new Vector3 (0, stopAngleY, 0);
					rotateState = RotateState.TOUCH_FALSE_RETURN;
				}
			}
			break;

		case RotateState.TOUCH_FALSE_RETURN: // 戻る
		case RotateState.TOUCH_ALL_RETURN: // 戻る
			if (stopAngleY < 0) {
				if (transform.rotation.eulerAngles.y >= 360 + stopAngleY && transform.rotation.eulerAngles.y < 360) {
					transform.Rotate (-angle * Time.deltaTime);
				} else {
					// 0でぴったり止める
					transform.eulerAngles = new Vector3 (0, 0, 0);
					rotateState = RotateState.NONE;
					if (numberGUIText != null) {
						numberGUIText.text = number;
						numberGUIText.color = Color.white;
					}
				}
			} else {
				if (transform.rotation.eulerAngles.y <= stopAngleY && transform.rotation.eulerAngles.y > 0 ) {
					transform.Rotate (-angle * Time.deltaTime);	
				} else {
					// 0でぴったり止める
					transform.eulerAngles = new Vector3 (0, 0, 0);
					rotateState = RotateState.NONE;
					if (numberGUIText != null) {
						numberGUIText.text = number;
						numberGUIText.color = Color.white;
					}
				}
			}
			break;

		}
	}

	void OnMouseDown ()
	{
		if (GameController.currentNumber == score) {
			rotateState = RotateState.TOUCH_TRUE;
			// 点数を追加
			GameObject.Find ("Score").SendMessage ("AddScore", 10);
			if(score == 25) {
				GameObject gameController = GameObject.Find ("GameController");
				gameController.SendMessage("ResetCube");
				GameController.currentNumber = 1;
			} else {
				GameController.currentNumber += 1;
			}
		} else if(GameController.currentNumber < score) {
			// 番号が違う場合
			rotateState = RotateState.TOUCH_FALSE_ROTATING;
		} else {
			// 番号が小さい場合は何もしない
		}
	}
}
