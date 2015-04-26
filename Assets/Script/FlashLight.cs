using UnityEngine;
using System.Collections;

public class FlashLight : MonoBehaviour {

	public float duration = 0.5F;

	// ゲームの状態
	public enum LightState
	{
		OFF,
		LEAP_ON,
		LEAP_OFF,
	}
	private LightState state;

	float time = 0.0f;
	public float limitIntensity = 0.38f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update() 
	{
		/*
		// 点滅させる場合
		float phi = Time.time / duration * 2 * Mathf.PI;
		float amplitude = Mathf.Cos(phi) * 0.5F + 0.5F;
		light.intensity = amplitude;
		*/

		if (state == LightState.LEAP_ON) {
			time += Time.deltaTime;
			float t = time / duration;

			if (t >= 1.0f) {
				t = 1.0f;
				state = LightState.LEAP_OFF;
				time = 0.0f;
			}
			GetComponent<Light>().intensity = limitIntensity * t;

		} else if (state == LightState.LEAP_OFF) {
			time += Time.deltaTime;
			float t = 1 - time / duration;

			if (t <= 0.0f) {
				t = 0.0f;
				state = LightState.OFF;
				time = 0.0f;
			}
			GetComponent<Light>().intensity = limitIntensity * t;
		}
	}
	public void Flash ()
	{
		state = LightState.LEAP_ON;
		time = 0.0f;
	}
}

