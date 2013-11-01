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
	private bool hayFuerzaExterna;
	private int fuerzaExterna;
	private Vector3 destinoCalculado;
	private Vector3 movimiento;
	
	void Start () {
		vel = velocidadNormal;
		destinoCalculado = new Vector3(30,0.5f,-30);
		movimiento = new Vector3(0,0,0);
		hayFuerzaExterna = true;
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
				movimiento.x = mousePosX;
				movimiento.z = mousePosZ;
				//No hay ninguna fuerza externa actuando por lo que el personaje se mueve normal
				if(!hayFuerzaExterna)
				{
					transform.position = Vector3.MoveTowards(transform.position, movimiento, Time.deltaTime * vel);
				}
				//Hay una fuerza externa por lo tanto se hace un calculo de nueva posicion destino depdendiendo hacia donde
				//el jugador este queriendo ir
				else
				{
					//se calcula la nueva posicion destino tomando en cuenta donde se da click
					destinoCalculado = (destinoCalculado + (movimiento.normalized));
					Debug.Log(destinoCalculado);
					//se hace la animacion
					iTween.MoveUpdate(gameObject, destinoCalculado, Mathf.Abs((destinoCalculado-movimiento).magnitude*2));
				}
				//El cuerpo ha llegado a su destino por una fuerza externa por lo que ya no se ejerce nada contra el
				if(destinoCalculado.x-transform.position.x> -10E-2 && destinoCalculado.x-transform.position.x < 10E-2
					&& destinoCalculado.z-transform.position.z> -10E-2 && destinoCalculado.z-transform.position.z < 10E-2){
					hayFuerzaExterna = false;
				}
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
	public void HayFuerzaExterna()
	{
		hayFuerzaExterna = true;
	}
}