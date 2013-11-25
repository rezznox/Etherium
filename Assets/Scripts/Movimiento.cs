using UnityEngine;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

[RequireComponent(typeof(PhotonView))]

/*
 * Script que controla el movimiento de los personajes
 */
public class Movimiento : Photon.MonoBehaviour{

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
	private bool 		hayFuerzaExterna;
	private int 		fuerzaExterna;
	private Vector3 	destinoCalculado;
	private Vector3 	movimiento;
	private Vector3 	_prevPosition;
	private bool 		inicializacion = false;
	private Vector3 	destinoFuerza;
	private int 		contadorMuestreos;
	private bool 		esSeguido = false;
	private bool 		hayGolpe = true;
	
	/*
	 * Variables de sincronizacion
	 */
	private Vector3 latestCorrectPos;
    private Quaternion latestCorretRot;
	
	void Start () {
		vel = velocidadNormal;
		contadorMuestreos = 0;
		destinoCalculado = new Vector3(30,0.5f,-30);
		destinoFuerza = destinoCalculado;
		movimiento = new Vector3(0,0,0);
		hayFuerzaExterna = true;
		
	}
	public void Awake()
    {
        this.enabled = true;   // due to this, Update() is not called on the owner client.

        latestCorrectPos = transform.position;
        latestCorretRot = transform.rotation;
		if (!photonView.isMine)
        {
            //MINE: local player, simply enable the local scripts
            this.enabled = false;
        }
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
			//rigidbody.AddForce(500,0.5f,500);
			
		}
		if(Input.GetMouseButtonDown(1))
		{
			rigidbody.AddForce(new Vector3(800,0.5f,800));
				//ri
			hayGolpe = false;
		}
		
	}
	public void FixedUpdate()
	{
		//Si es posible, mueve al perosnaje
		if(movPermitido){
			Vector3 destino = new Vector3(mousePosX, 0, mousePosZ);
			if(!destino.Equals(noMover)){
				movimiento.x = mousePosX;
				movimiento.z = mousePosZ;
				movimiento.y = 0.5f;
				
				Vector3 vel = (transform.position - _prevPosition) / Time.deltaTime;
				 _prevPosition = transform.position;
				//No hay ninguna fuerza externa actuando por lo que el personaje se mueve normal
				if(hayFuerzaExterna && vel.magnitude <2)
				{
					//transform.position = Vector3.MoveTowards(transform.position, movimiento, Time.deltaTime *2);
					if((movimiento-vel).magnitude > -1 || (movimiento-vel).magnitude < 1)
					{
						rigidbody.AddForce((movimiento- transform.position).normalized*3);
						//rigidbody.AddForce(new Vector3(10,0.5f,10));
					}
					
				}
				//Hay una fuerza externa por lo tanto se hace un calculo de nueva posicion destino depdendiendo hacia donde
				//el jugador este queriendo ir
				
				//El cuerpo ha llegado a su destino por una fuerza externa por lo que ya no se ejerce nada contra el
				if((destinoCalculado.x-transform.position.x> -10E-2 && destinoCalculado.x-transform.position.x < 10E-2
					&& destinoCalculado.z-transform.position.z> -10E-2 && destinoCalculado.z-transform.position.z < 10E-2)|| (Mathf.Abs((destinoCalculado-transform.position).magnitude)<1  && inicializacion == true)){
					hayFuerzaExterna = false;
				}
				//Rota al personaje
				Quaternion newRotation = Quaternion.LookRotation(new Vector3(mousePosX, 0, mousePosZ) - transform.position, Vector3.forward);
				newRotation.x = 0;
				newRotation.z = 0;
				transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 5);
			}
			inicializacion = true;
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
	//sincronizacion con el servidor
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation); 
        }
        else
        {
            //Network player, receive data
            latestCorrectPos = (Vector3)stream.ReceiveNext();
            latestCorretRot = (Quaternion)stream.ReceiveNext();
        }
    }
}