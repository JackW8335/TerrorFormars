using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class SliderEmissionBehaviour : MonoBehaviour {

	public Slider slider; //In this case I am using a UI Slider to control the health, but you use any method you want.
	//Just make healthValue a float decimal from 0 to 1, 0 being no health and 1 being full health.


	private float healthValue = 0.0f;
	private Material m;

	public void Start() {
		m = GetComponent<MeshRenderer> ().material; //Getting the Material
	}

	public void Update() {
		healthValue = slider.value; //Setting health value to slider. CHANGE THIS TO YOUR HEALTH FUNCTION
		m.SetFloat ("_Threshold", healthValue); //Setting _Threshold in the shader to healthValue
	}


}
