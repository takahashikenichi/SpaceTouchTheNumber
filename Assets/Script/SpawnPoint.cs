using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
	public GameObject debri;
	public float interval = 0.1f;
	// 宇宙ゴミ発生フラグ
	private bool spawnStarted = false;
	// 宇宙ゴミ発生開始
	void StartSpawn ()
	{
		if (!spawnStarted) {
			spawnStarted = true;
			StartCoroutine ("SpawnDebris");
		}
	}
	// 宇宙ゴミ発生停止
	void StopSpawn ()
	{
		if (spawnStarted) {
			spawnStarted = false;
			StopCoroutine ("SpawnDebris");
		}
	}
	// Use this for initialization
	void Start ()
	{
		//				StartCoroutine("SpawnDebris");
	}

	IEnumerator SpawnDebris ()
	{
		while (true) {
			// 宇宙ゴミプレファブを SpawnPoint オブジェクトの位置にインスタンス化する
			Instantiate (debri, transform.position, Quaternion.identity);
			// interval 分だけ停止する
			yield return new WaitForSeconds (interval);
		}
	}
}
