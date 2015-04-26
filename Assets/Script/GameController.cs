using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	// タイトル画面
	//	public GUITexture guiTitle;
	public GUIText guiTitle;
	// タイムアップ画面
	//	public GUITexture guiTimeup;
	public GUIText guiTimeup;
	// ゲームの状態
	public enum GameState
	{
		TITLE,
		PLAYING,
		TIMEUP,
		TIMEUP_TO_TITLE,
	}
	// ゲームの状態
	private GameState state;
	// SpawnPoint ゲームオブジェクト
	private GameObject spawnPoint;
	// Score ゲームオブジェクト
	private GameObject score;
	// Timer コンポーネント
	private Timer timer;

	public GameObject cube;

	private bool isFirst = true;

	public static int currentNumber = 1;

	//シャッフルする配列
	int[] numberArray = new int[25];

	private float timeRemaining = 1.0f;

	// Use this for initialization
	void Start ()
	{
		for (int i = 0; i < 25; i++) {
			numberArray[i] = i + 1;
		}

		// 状態をタイトルに
		state = GameState.TITLE;
		// タイトル画像を表示
		guiTitle.enabled = true;
		// タイムアップ画像を非表示
		guiTimeup.enabled = false;

		// SpawnPoint ゲームオブジェクトを取得
		spawnPoint = GameObject.Find ("SpawnPoint");
		// Scoreゲームオブジェクトを取得
		score = GameObject.Find ("Score");
		// TimerゲームオブジェクトのTimerコンポーネントを取得
		timer = GameObject.Find ("Timer").GetComponent<Timer> ();
		// Cubeオブジェクトを5x5作成する // Quaternion.indentity = no Rotation
	}
	// Update is called once per frame
	void Update ()
	{
		switch (state) {
		case GameState.TITLE:
						// タイトル状態でマウスで左クリックされたらプレイ状態へ
			if (Input.GetMouseButtonUp (0)) {
				// ゲームデータをリセット
				currentNumber = 1;
				// ステータスを変更
				state = GameState.PLAYING;
				// SpawnPointゲームオブジェクトにStartSpawn()関数を実行するようにメッセージを送る
				// 宇宙ゴミの発生を開始
				spawnPoint.SendMessage ("StartSpawn");
				// ScoreゲームオブジェクトにInitScore()関数を実行売るようにメッセージを送る
				score.SendMessage ("InitScore");
				// TimerコンポーネントのStartTimer（）関数を実行。タイマーを開始
				timer.StartTimer ();
				// タイトル画面を非表示
				guiTitle.enabled = false;
			}
			break;
		case GameState.PLAYING:
						// プレイ中にTimerコンポーネントの残り時間が0になったらタイムアップ状態に
			if(isFirst) {
				//Fisher-Yatesアルゴリズムでシャッフルする
				System.Random random = new System.Random();
				int n = numberArray.Length;
				while (n > 1)
				{
					n--;
					int k = random.Next(n + 1);
					int tmp = numberArray[k];
					numberArray[k] = numberArray[n];
					numberArray[n] = tmp;
				}
					
				for(int x = 0; x < 5; x++) {
					for(int y = 0; y < 5; y++) {
						GameObject cubeInstance = Instantiate(cube, new Vector3(x * 12.0F - 24F, 74F - y * 12.0F, -100), Quaternion.identity) as GameObject;
						int number = x * 5 + y;
						string numberString = ""  + numberArray[number];
						// cubeInstanceにタグを付ける
						cubeInstance.tag = "cube";
						// TouchRotationというスクリプトを取得する
						TouchRotation t = cubeInstance.GetComponent<TouchRotation> ();
						t.SetNumber(numberString);
					}
				}
				isFirst = false;
			}
			if (timer.GetTimeRemaining () == 0) {
				state = GameState.TIMEUP;
				// SpawnPointゲームオブジェクトにStopSpawn()関数を実行うるようにメッセージを送る
				// 宇宙ゴミの発生を停止する
				spawnPoint.SendMessage ("StopSpawn");
				// TimerコンポーネントのStopTimer()関数を実行。タイマーを停止。
				timer.StopTimer ();
				// 画面内の宇宙ゴミを全て削除する
				DestroyAllObjects ("debri");
				// タイムアップ画像を表示
				guiTimeup.enabled = true;
			}
			break;

		case GameState.TIMEUP:
			// 残り時間を引いてゆく
			timeRemaining -= Time.deltaTime;
			if (timeRemaining > 0) {
				break;
			} else {
				timeRemaining = 0;
				// タイムアップ状態でマウスを左クリックで３秒後にタイトル状態にする
				if (Input.GetMouseButtonUp (0)) {
					state = GameState.TIMEUP_TO_TITLE;
					isFirst = true;
					DestroyAllObjects("cube");
					StartCoroutine ("ShowTitleDelayed", 0f);
					timeRemaining  = 1.0f;
					// 画面内の宇宙ゴミParticleを全て削除する
					DestroyAllObjects ("debribroken");
				}
			}
			break;
		}
	}
	// シーン中の全ての宇宙ゴミを削除
	void DestroyAllDebris ()
	{
		GameObject[] debris = GameObject.FindGameObjectsWithTag ("debri");
		foreach (GameObject debri in debris) {
			Destroy (debri);
		}
	}
	// deleyTime秒後にタイトルを表示
	IEnumerator ShowTitleDelayed (float delayTime)
	{
		// delayTime秒後処理を停止
		yield return new WaitForSeconds (delayTime);
		state = GameState.TITLE;
		// タイマーをリセット
		timer.ResetTimer ();
		// タイトル画面を表示
		guiTitle.enabled = true;
		// タイムアップ画像を非表示
		guiTimeup.enabled = false;
	}

	// 特定タグを全て削除
	void DestroyAllObjects(string tagName) {
		//FindGameObjectsWithTagメソッド指定のタグのインスタンスを配列で取得
		GameObject[] objects = GameObject.FindGameObjectsWithTag(tagName);

		//配列内のオブジェクトの数だけループ
		foreach (GameObject obj in objects) {
			//オブジェクトを削除
			Destroy(obj);
		}
	}

	void ResetCube() {
		//Fisher-Yatesアルゴリズムでシャッフルする
		System.Random random = new System.Random();
		int n = numberArray.Length;
		while (n > 1)
		{
			n--;
			int k = random.Next(n + 1);
			int tmp = numberArray[k];
			numberArray[k] = numberArray[n];
			numberArray[n] = tmp;
		}

		//FindGameObjectsWithTagメソッド指定のタグのインスタンスを配列で取得
		GameObject[] objects = GameObject.FindGameObjectsWithTag("cube");

		int number = 0;
		//配列内のオブジェクトの数だけループ
		foreach (GameObject obj in objects) {
			//オブジェクトを削除
			TouchRotation touchRotation = obj.GetComponent<TouchRotation>();
			if(touchRotation != null) {
				touchRotation.rotateState = TouchRotation.RotateState.TOUCH_ALL_RETURN;
				string numberString = ""  + numberArray[number];
				touchRotation.SetNumber (numberString);
			}
			number++;
		}

		FlashLight flashLight = GameObject.Find ("FlashLight").GetComponent<FlashLight> ();
		flashLight.Flash();
	}
}
