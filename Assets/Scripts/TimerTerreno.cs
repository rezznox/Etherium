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
	public float	reduccion;		//Cantidad que se reduce el terreno cada contador
	public int		numContadores;	//Numero de veces que se reduce el terreno en una partida
	
	private float 	tiempoInicio;	//Inicio real de la partida
	private float	tiempoAnterior;	//Marca anterior de tiempo
	private float	marca;			//Marca de tiempo para hacer una reduccion
	
//---------------------------------------------------------------
// Metodos
//---------------------------------------------------------------
	
	void Start () {
		tiempoInicio = Time.time;
		tiempoAnterior = 0.0f;
		marca = tiempo/numContadores;
	}
	
	void Update () {
		
		float tiempoActual = Time.time - tiempoInicio;
		float dif = tiempoActual - tiempoInicio;
	
		if(dif >= marca){
			tiempoInicio = tiempoActual;
			float escalaActual = transform.localScale.x;
			if(!(tiempoActual >= tiempo)){
				transform.localScale = new Vector3(escalaActual - reduccion, 0,escalaActual - reduccion);
			}
			else
				Debug.Log("Se acabo el tiempo");
		}
	}
}