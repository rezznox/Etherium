using UnityEngine;
using System.Collections;

/*
 * Controla la desaparicion paulatina del terreno
 */
public class TimerTerreno : MonoBehaviour {
	
//---------------------------------------------------------------
// Atributos
//---------------------------------------------------------------
	
	public float 	tiempo;			//Tiempo total que toma la partida
	public float 	dif;			//Diferencia de tiempo en cada frame
	
	private float 	tiempoInicio;	//Inicio real de la partida
	
//---------------------------------------------------------------
// Metodos
//---------------------------------------------------------------
	
	void Start () {
		tiempoInicio = Time.time;
	}
	
	void Update () {
		float tiempoActual = Time.time - tiempoInicio;
		float tiempoRestante = tiempo - tiempoActual;
		
		//Disminuye periodicamente la escala del terreno
		float escalaActual = transform.localScale.x;
		
		//Detienen la disminucion del terreno al llegar a cierta escala
		if(escalaActual > 15){
			transform.localScale = new Vector3(escalaActual - dif, 0,escalaActual - dif);
		}
	}
}