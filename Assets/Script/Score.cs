using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
	// スコア
	static public int score;
	// スコアを初期化する
	void InitScore ()
	{
		score = 0;
	}
	// スコアを加算する
	void AddScore (int addScore)
	{
		score += addScore;
	}
	// Use this for initialization
	void Start ()
	{
	
	}
	// Update is called once per frame
	void Update ()
	{
		// GUIテキストを書き換える
		GetComponent<GUIText>().text = "Score:" + score;
	}
}
