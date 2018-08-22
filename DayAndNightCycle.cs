using UnityEngine;
using System.Collections;

public class DayAndNightCycle : MonoBehaviour
{
	public Gradient dayLightColor;
	public GameObject stars;
	private Light mainLight;
	public Vector3 dayRotateSpeed = new Vector3 (5, 0, 0);
	public float maxIntensity = 3;
	public float minIntensity = 0;
	private float percent;
	private float tempIntensity;
	private bool isLightOn = false;
	public Renderer lightRenderer;
	// Use this for initialization
	void Start ()
	{
		mainLight = GetComponent<Light> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		stars.transform.rotation = transform.rotation;
		percent = Mathf.Clamp01 (Vector3.Dot (transform.forward, Vector3.down));
		tempIntensity = (maxIntensity - minIntensity) * percent + minIntensity;
		mainLight.intensity = tempIntensity;
		mainLight.color = dayLightColor.Evaluate (percent);


		if (percent > 0.3f) {
			if (isLightOn) {
				Color offColor = Color.white * Mathf.LinearToGammaSpace (0);
				DynamicGI.SetEmissive (lightRenderer, offColor);
				isLightOn = false;
			}
		} else {
			if (!isLightOn) {
				Color onColor = Color.white * Mathf.LinearToGammaSpace (5);
				DynamicGI.SetEmissive (lightRenderer, onColor);
				isLightOn = true;
			}
		}
		transform.Rotate (dayRotateSpeed * Time.deltaTime);
	}
}
