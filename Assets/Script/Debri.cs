using UnityEngine;
using System.Collections;

public class Debri : MonoBehaviour
{
	// 1秒辺りの回転角度
	public float angle = 30f;
	// 破壊時の得点
	public int score = 10;
	// 回転の中心座標
	private Vector3 targetPos;
	private Vector3 targetRot;
	private bool targetWay;

	// 破壊時のパーティクル
	public GameObject brokenPrefab;

	// Use this for initialization
	void Start ()
	{
		// シーン中のEarthオブジェクトにアクセスして、EarthオブジェクトをTransformコンポーネントにアクセス
		Transform target = GameObject.Find ("Moon").transform;
		// Earthオブジェクトの位置情報を取得しておく
		targetPos = target.position;
		// 宇宙ゴミの向きをEarthの方向に向ける
		//				transform.LookAt(target);
		// ローテーションを決定する
		// ランダムに軸を決定
		targetRot = new Vector3 (Random.Range (-180, 180), Random.Range (-180, 180), 0);
		transform.Rotate (targetRot, Space.World);
		targetWay = (Random.Range (0, 2) == 1);
	}
	// Update is called once per frame
	void Update ()
	{
		// Earthを中心に宇宙ゴミの現在の上方向に 毎秒angle分だけ回転する
		//				Vector3 axis = transform.TransformDirection(Vector3.up);
		if (targetWay) {
			transform.RotateAround (targetPos, targetRot, -angle * Time.deltaTime * 2);
		} else {
			transform.RotateAround (targetPos, targetRot, angle * Time.deltaTime * 2);
		}
		// 軸に沿って少し回転させる
		transform.Rotate (targetRot, Time.deltaTime * 360 * 2, Space.Self);
	}

	void OnDestroy() {
		// 破壊時のオブジェクトをインスタンス化
		GameObject brokenParticle = Instantiate(brokenPrefab, transform.position, brokenPrefab.transform.rotation) as GameObject;
		DebriBroken db = brokenParticle.GetComponent<DebriBroken> ();
		db.SetTargetRot(targetRot, targetWay, angle);
	}
}
