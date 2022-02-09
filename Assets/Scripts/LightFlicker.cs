using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enColorchannels {
	all = 0,
	red = 1,
	blue = 2,
	green = 3
}
public enum enWaveFunctions {
	sinus = 0,
	triangle = 1,
	square = 2,
	sawtooth = 3,
	inverted_saw = 4,
	noise = 5
}
public class LightFlicker : MonoBehaviour {

	public enColorchannels colorChannel = enColorchannels.all;
	public enWaveFunctions waveFunction = enWaveFunctions.sinus;
	public float offset = 0.0f;
	public float amplitude = 1.0f;
	public float phase = 0.0f;
	public float frequency = 0.5f;
	public bool affectsIntensity = true;


	private Color originalColor;
	private float originalIntensity;

	private Light light;

	// Use this for initialization
	void Start() {
		originalColor = GetComponent<Light>().color;
		originalIntensity = GetComponent<Light>().intensity;
		light = GetComponent<Light>();
	}

	// Update is called once per frame
	void Update() {

		if (affectsIntensity) {
			light.intensity = originalIntensity * EvalWave();
		}

		Color o = originalColor;
		Color c = light.color;

		if (colorChannel == enColorchannels.all) {
			light.color = originalColor * EvalWave();
		} else if (colorChannel == enColorchannels.red) {
			light.color = new Color(o.r * EvalWave(), c.g, c.b, c.a);
		} else if (colorChannel == enColorchannels.green) {
			light.color = new Color(c.r, o.g * EvalWave(), c.b, c.a);
		} else {
			light.color = new Color(c.r, c.g, o.b * EvalWave(), c.a);
		}
	}

	float EvalWave() {
		float x = (Time.time + phase) * frequency;
		float y;

		x = x - Mathf.Floor(x);

		if (waveFunction == enWaveFunctions.sinus) {
			y = Mathf.Sin(x * 2f * Mathf.PI);
		} else if (waveFunction == enWaveFunctions.triangle) {
			if (x < 0.5)
				y = 4.0f * x - 1.0f;
			else
				y = -4.0f * x + 3.0f;
		} else if (waveFunction == enWaveFunctions.square) {
			if (x < 0.5)
				y = 1.0f;
			else
				y = -1.0f;
		} else if (waveFunction == enWaveFunctions.sawtooth) {
			y = x;
		} else if (waveFunction == enWaveFunctions.inverted_saw) {
			y = -x;
		} else if (waveFunction == enWaveFunctions.noise) {
			y = 1f - (Random.value * 2f);
		} else {
			y = 1.0f;
		}
		return (y * amplitude) + offset;

	}
}
