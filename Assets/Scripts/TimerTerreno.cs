using UnityEngine;
using System.Collections;

public class TimerTerreno : MonoBehaviour {
	
	public float tiempo;
	public float dif;
	private float tiempoInicio;
	
	// Use this for initialization
	void Awake () {
		tiempoInicio = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		float tiempoActual = Time.time - tiempoInicio;
		float tiempoRestante = tiempo - tiempoActual;
		float escalaActual = transform.localScale.x;
		
		if(escalaActual > 1){
			transform.localScale = new Vector3(escalaActual - dif, escalaActual - dif,escalaActual - dif);
		}
	}
}
