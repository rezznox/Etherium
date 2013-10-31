using UnityEngine;
using System.Collections;

/*
 * Script que controla el movimiento de los personajes
 */
public class Movimiento : MonoBehaviour {

//----------------------------------------------------------------
// Atributos
//----------------------------------------------------------------
	
	public float 		velocidadNormal;		//Velocidad normal del personaje
	public float 		velocidadLava;			//Velocidad en lava del personaje
	
	private float 		vel;					//Velocidad actual del personaje
	private bool 		movPermitido = true;	//Determina si le es permitido al personaje moverse o no
	private float 		mousePosX = 0; 			//Determiana la posicion en x del movimiento
    private float 		mousePosZ = 0;			//Determina la posicion en z del movimiento
	private Ray 		rayH;					//RayCast para determinar la posciion del personaje
	private RaycastHit 	hit;					//Guarda la informacion de colision del RayCast
	private Vector3 	noMover;				//Controla glitches de movimiento en el plano
	
	void Start () {
		vel = velocidadNormal;	
	}
	
//----------------------------------------------------------------
// Metodos
//----------------------------------------------------------------
	
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			//Determina la posicion objetivo
			rayH = Camera.main.ScreenPointToRay (Input.mousePosition);
			EncenderMovimiento();
			if(Physics.Raycast(rayH, out hit, 50))
			{
				mousePosX = hit.point.x; 
		    	mousePosZ = hit.point.z;
			}
		}
		//Si es posible, mueve al perosnaje
		if(movPermitido){
			Vector3 destino = new Vector3(mousePosX, 0, mousePosZ);
			if(!destino.Equals(noMover)){
				//Determina la posicion objetivo
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(mousePosX, 0.5f, mousePosZ), Time.deltaTime * vel);
				//Rota al personaje
				Quaternion newRotation = Quaternion.LookRotation(new Vector3(mousePosX, 0, mousePosZ) - transform.position, Vector3.forward);
				newRotation.x = 0;
				newRotation.z = 0;
				transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 5);
			}
		}
		// Detiene al personaje si ya llego al destino
		if(mousePosX-transform.position.x> -10E-2 && mousePosX-transform.position.x < 10E-2
					&& mousePosZ-transform.position.z> -10E-2 && mousePosZ-transform.position.z < 10E-2){
			ApagarMovimiento();
		}
	}
	
	/*
	 * Inhabilita el movimeinto
	 */
	public void ApagarMovimiento(){
		movPermitido = false;	
	}
	
	/*
	 * Habilita el movimiento
	 */
	public void EncenderMovimiento(){
		mousePosX = 0;
		mousePosZ = 0;
		movPermitido = true;
	}
	
	/*
	 * No permite que el personaje se mueva a una posicion especifica
	 */
	public void NoMoverA(Vector3 no){
			noMover = no;
	}
	
	/*
	 * Cambia la velocidad si se encuentra en lava
	 */
	public void EnLava(bool enLava){
		if(enLava)
			vel=velocidadLava;
		else
			vel = velocidadNormal;
	}
}