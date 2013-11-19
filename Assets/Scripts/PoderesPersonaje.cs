using UnityEngine;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

[RequireComponent(typeof(PhotonView))]

/*
 * Administra todos los poderes de un personaje
 */
public class PoderesPersonaje : Photon.MonoBehaviour{
	
//-------------------------------------------------------------
// Atributos
//-------------------------------------------------------------
	
	public GameObject 	Fireball;					//Instancia del prefa de la fireball
	public Transform 	ParticulasFireball;			//Sistema de particulas para la fireball
	private GameObject 	poderActual;				//Poder que se va a disparar
	private bool 		poderSeleccionado = false;	//Determina si un poder ha sido seleccionado o no
	private Vector3 	posDestino;					//Vector destino del poder disparado
	private Cooldown 	poderesEnCool;				//Relacion con el script de cooldown
	
	private Movimiento 	mov;						//Relacion con el script de movimiento
	private Ray 		rayH;						//RayCast para determinar el vector de movimiento
	private RaycastHit 	hit;						//Infromacion de la colision del poder
	
	private int 		fireballOC = 1;				//Determina si la fireball esta en cooldown o no
	
//-------------------------------------------------------------
// Metodos
//-------------------------------------------------------------
	
	void Start () {
		//Inicializa los componentes necesarios
		mov = (Movimiento)GetComponent(typeof(Movimiento));
		poderesEnCool = (Cooldown)GetComponent(typeof(Cooldown));
		if (!photonView.isMine)
        {
            //MINE: local player, simply enable the local scripts
            this.enabled = false;
        }
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			if(poderSeleccionado){
				//Crea el poder
				GameObject clon = (GameObject)Instantiate(poderActual,transform.position, transform.rotation);
				Poder p = (Poder)clon.GetComponent(typeof(Poder));
				p.setCaster(this.gameObject);
				p.setParticulas(ParticulasFireball);
				Physics.IgnoreCollision(clon.collider,this.gameObject.collider);
				
				//Lanza el poder
				rayH = Camera.main.ScreenPointToRay (Input.mousePosition);
				if(Physics.Raycast(rayH, out hit, 50))
				{
					//Mira hacia el pbjetivo
					Vector3 newRotation = new Vector3(hit.point.x, 0, hit.point.z);
					transform.LookAt(newRotation);
					
					//Lanzamiento
					p.Disparar(hit.point.x, hit.point.z );
					poderSeleccionado = false;
					fireballOC = 0;
					//Evita que el personaje se mueva al punto de lanzamiento
					mov.NoMoverA(new Vector3(hit.point.x,0,hit.point.z));
					mov.EncenderMovimiento();
					//Coloca el poder en cooldown
					poderesEnCool.PonerEnCooldown(Cooldown.FIREBALL, p.darCooldown());
				}		
			}
		}
		
		//Selecciona la fireball con Q si el poder no esta en cooldown
		if(Input.GetKeyDown(KeyCode.Q)){
			if(fireballOC == 1){
				poderActual = Fireball;
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