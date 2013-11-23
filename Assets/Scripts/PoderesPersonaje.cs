using UnityEngine;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

//[RequireComponent(typeof(PhotonView))]

/*
 * Administra todos los poderes de un personaje
 */
public class PoderesPersonaje : Photon.MonoBehaviour{
	
//-------------------------------------------------------------
// Atributos
//-------------------------------------------------------------
	
	public GameObject 	Fireball;					//Instancia del prefab de la fireball
	public GameObject 	Teleport;					//Instancia del prefab del teleport
	public Transform 	ParticulasFireball;			//Sistema de particulas para la fireball
	public Transform 	ParticulasTeleport;			//Sistema de particulas para el teleport
	
	private GameObject 	poderActual;				//Poder que se va a disparar
	private Poder 		instanciaPoder;				//Instancia del poder que se esta disparando
	private Transform   particulasActual;			//Sistema de particulas del poder actual
	private bool 		poderSeleccionado = false;	//Determina si un poder ha sido seleccionado o no
	private Vector3 	posDestino;					//Vector destino del poder disparado
	private Cooldown 	poderesEnCool;				//Relacion con el script de cooldown
	
	private Movimiento 	mov;						//Relacion con el script de movimiento
	private Ray 		rayH;						//RayCast para determinar el vector de movimiento
	private RaycastHit 	hit;						//Infromacion de la colision del poder
	
	private int 		fireballOC = 1;				//Determina si la fireball esta en cooldown o no
	private int 		teleportOC = 1;				//Determina si el teleport esta en cooldown o no
	
//-------------------------------------------------------------
// Metodos
//-------------------------------------------------------------
	
	void Start () {
		//Inicializa los componentes necesarios
		mov = (Movimiento)GetComponent(typeof(Movimiento));
		poderesEnCool = (Cooldown)GetComponent(typeof(Cooldown));
		/*if (!photonView.isMine)
        {
            //MINE: local player, simply enable the local scripts
            this.enabled = false;
        }*/
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			if(poderSeleccionado){
				//Lanza el poder
				rayH = Camera.main.ScreenPointToRay (Input.mousePosition);
				if(Physics.Raycast(rayH, out hit, 50))
				{
					//Mira hacia el pbjetivo
					Vector3 newRotation = new Vector3(hit.point.x, 0, hit.point.z);
					transform.LookAt(newRotation);
					
					//Lanzamiento
					instanciaPoder.Disparar(hit.point.x, hit.point.z );
					poderSeleccionado = false;
					//Evita que el personaje se mueva al punto de lanzamiento
					mov.NoMoverA(new Vector3(hit.point.x,0,hit.point.z));
					mov.EncenderMovimiento();
					//Coloca el poder en cooldown
					if(instanciaPoder.getId() == Cooldown.FIREBALL){
						fireballOC = 0;
						poderesEnCool.PonerEnCooldown(Cooldown.FIREBALL, instanciaPoder.darCooldown());
					}
					else if(instanciaPoder.getId() == Cooldown.TELEPORT){
						teleportOC = 0;
						poderesEnCool.PonerEnCooldown(Cooldown.TELEPORT, instanciaPoder.darCooldown());
					}
				}		
			}
		}
		
		//Selecciona la fireball con Q si el poder no esta en cooldown
		if(Input.GetKeyDown(KeyCode.Q)){
			if(fireballOC == 1){
				poderActual = Fireball;
				particulasActual = ParticulasFireball;
				poderSeleccionado = true;
				mov.ApagarMovimiento();
				
				GameObject clon = (GameObject)Instantiate(poderActual,transform.position, transform.rotation);
				clon.transform.parent = transform;
				instanciaPoder = (Poder)clon.GetComponent(typeof(Poder));
				instanciaPoder.setCaster(this.gameObject);
				instanciaPoder.setParticulas(particulasActual);
				instanciaPoder.setId(Cooldown.FIREBALL);
				Physics.IgnoreCollision(clon.collider,this.gameObject.collider);
				Debug.Log("Poder seleccionado");
			}
		}
		
		if(Input.GetKeyDown(KeyCode.W)){
			if(teleportOC == 1){
				poderActual = Teleport;
				poderSeleccionado = true;
				mov.ApagarMovimiento();
				Debug.Log("Poder seleccionado");
			}
		}
	}
	
	/*
	 * Dibuja los slots de cada poder en el HUD
	 */
	void OnGUI()
	{
  		GUI.Box(new Rect(20, Screen.height-80,25, 25), "Fire");
        GUI.Box(new Rect(20*fireballOC, (Screen.height-80)*fireballOC, 25*fireballOC, 25*fireballOC), "");
		
		GUI.Box(new Rect(50, Screen.height-80,25, 25), "Tele");
        GUI.Box(new Rect(20*teleportOC, (Screen.height-80)*teleportOC, 25*teleportOC, 25*teleportOC), "");
	}
	
	/*
	 * Saca un poder del cooldown
	 */
	public void FinCooldown(int IDPoder){
		if(IDPoder == Cooldown.FIREBALL){
			fireballOC = 1;	
		}
	}
}