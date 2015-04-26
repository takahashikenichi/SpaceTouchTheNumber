using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

		// 残り時間の初期値
		public int timeLimit = 30;

		// 残り時間
		private float timeRemaining;
		// タイマー動作フラグ
		private bool timerStarted;

	// Use this for initialization
	void Start () {
				ResetTimer();
	}

		// タイマーをリセット
		public void ResetTimer() {
				timeRemaining = timeLimit;
				timerStarted = false;
		}

		// タイマーを開始
		public void StartTimer() {
				timerStarted = true;
		}

		public void StopTimer() {
				timerStarted = false;
		}

		public float GetTimeRemaining() {
				return timeRemaining;
		}
	
	// Update is called once per frame
	void Update () {
				if (timerStarted) {
						// 残り時間を引いてゆく
						timeRemaining -= Time.deltaTime;
						if (timeRemaining <= 0) {
								// 残り時間が0以下あらタイマーを停止する
								timeRemaining = 0;
								timerStarted = false;
						}
				}
				// テキストを更新
				GetComponent<GUIText>().text = "Time:" + timeRemaining;
	
	}
}
