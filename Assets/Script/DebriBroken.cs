using UnityEngine;
using System.Collections;

public class DebriBroken : MonoBehaviour {
	private Vector3 targetRot;
	private Vector3 targetPos;
	private bool targetWay;
	// 1秒辺りの回転角度
	private float angle = 30f;

	// Use this for initialization
	void Start () {
		// シーン中のEarthオブジェクトにアクセスして、EarthオブジェクトをTransformコンポーネントにアクセス
		Transform target = GameObject.Find ("Moon").transform;
		// Earthオブジェクトの位置情報を取得しておく
		targetPos = target.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(targetRot != null) {
			if (targetWay) {
				transform.RotateAround (targetPos, targetRot, -angle * Time.deltaTime / 5);
			} else {
				transform.RotateAround (targetPos, targetRot, angle * Time.deltaTime / 5);
			}
			// 軸に沿って少し回転させる
			transform.Rotate (targetRot, Time.deltaTime * 360 / 5, Space.Self);
		}
	}

	public void SetTargetRot (Vector3 targetRot, bool targetWay, float angle) {
		this.targetRot = targetRot;
		this.targetWay = targetWay;
		this.angle = angle;
	}
}
