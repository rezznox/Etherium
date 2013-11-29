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
	
	public GameObject 	Fireball;					//Instancia del prefab de la fireball
	public GameObject 	Teleport;					//Instancia del prefab del teleport
	public GameObject   Escudo;						//Instancia del prefab del escudo
	public GameObject 	Bolt;						//Instancia del prefab del bolt
	public Transform 	ParticulasFireball;			//Sistema de particulas para la fireball
	private GameObject 	GOParticulasFireball;
	private GameObject 	GOParticulasTeleport;
	public Transform 	ParticulasTeleport;			//Sistema de particulas para el teleport
	private GameObject 	GOParticulasEscudo;
	public Transform 	ParticulasEscudo;
	public GameObject 	GOBolt;					//Transform objetivo para el Bolt
	
	private GameObject 	poderActual;				//Poder que se va a disparar
	private Poder 		instanciaPoder;				//Instancia del poder que se esta disparando
	private Transform   particulasActual;			//Sistema de particulas del poder actual
	private bool 		poderSeleccionado = false;	//Determina si un poder ha sido seleccionado o no
	private Vector3 	posDestino;					//Vector destino del poder disparado
	private Cooldown 	poderesEnCool;				//Relacion con el script de cooldown
	private GameObject  clon;
	
	private Movimiento 	mov;						//Relacion con el script de movimiento
	private Ray 		rayH;						//RayCast para determinar el vector de movimiento
	private RaycastHit 	hit;						//Infromacion de la colision del poder
	
	private int 		fireballOC = 1;				//Determina si la fireball esta en cooldown o no
	private int 		teleportOC = 1;				//Determina si el teleport esta en cooldown o no
	private int 		boltOC = 1;					//Determina si el bolt esta en cooldown o no
	private int			escudoOC = 1;
	
//-------------------------------------------------------------
// Metodos
//-------------------------------------------------------------
	
	void Start () {
		//Inicializa los componentes necesarios
		mov = (Movimiento)GetComponent(typeof(Movimiento));
		poderesEnCool = (Cooldown)GetComponent(typeof(Cooldown));
		poderActual = new GameObject();
		poderActual.name = "";
		if (!photonView.isMine)
        {
            //MINE: local player, simply enable the local scripts
            this.enabled = false;
        }
	}
	
	void Update () {
		
		//Selecciona la fireball con Q si el poder no esta en cooldown
		if(fireballOC == 1){
			if(Input.GetKeyDown(KeyCode.Q) && !poderActual.name.Equals("Fireball")){
				
					poderActual = Fireball;
					poderSeleccionado = true;
			}
			else if(poderActual.name.Equals("Fireball"))
			{
				poderActual.name = "";
			}
		}
		
		if(Input.GetKeyDown(KeyCode.W) && !poderActual.name.Equals("Teleport")){
			if(teleportOC == 1){
				poderActual = Teleport;
				poderSeleccionado = true;
				clon = (GameObject)PhotonNetwork.Instantiate(poderActual.name,transform.position, Quaternion.identity, 0);
				instanciaPoder = (Poder)clon.GetComponent(typeof(Poder));
				instanciaPoder.setCaster(this.gameObject);
				instanciaPoder.setId(Cooldown.TELEPORT);
				clon.transform.parent = transform;
			}
		}
		
		if(Input.GetKeyDown(KeyCode.E)){
			poderActual = Bolt;
			poderSeleccionado = true;
		}
		if(Input.GetKeyDown(KeyCode.R)){
			if(escudoOC == 1){
				poderActual = Escudo;
				poderSeleccionado = true;
				clon = PhotonNetwork.Instantiate(poderActual.name,transform.position, Quaternion.identity, 0);
				instanciaPoder = (Poder)clon.GetComponent(typeof(Poder));
				instanciaPoder.setCaster(this.gameObject);
				instanciaPoder.setId(Cooldown.ESCUDO);
				instanciaPoder.setParticulas(clon.transform, clon);
				clon.transform.parent = transform;
			}
		}
		
		if(Input.GetMouseButtonDown(0)){
			if(poderSeleccionado){
				//Lanza el poder
				rayH = Camera.main.ScreenPointToRay (Input.mousePosition);
				if(Physics.Raycast(rayH, out hit, 50) && poderActual.name.Contains("Fireball") && fireballOC == 1)
				{
					//Mira hacia el pbjetivo
					Vector3 newRotation = new Vector3(hit.point.x, 0.5f, hit.point.z);
					transform.LookAt(newRotation);
					clon = PhotonNetwork.Instantiate(poderActual.name,transform.position, Quaternion.identity, 0);
					Physics.IgnoreCollision(clon.collider,this.gameObject.collider);
					instanciaPoder = (Poder)clon.GetComponent(typeof(Poder));
					instanciaPoder.setCaster(this.gameObject);
					GOParticulasFireball = (GameObject)PhotonNetwork.Instantiate(ParticulasFireball.name,transform.position, Quaternion.identity, 0);
					GOParticulasFireball.transform.parent = clon.transform;
					particulasActual = GOParticulasFireball.transform;
					instanciaPoder.setParticulas(particulasActual, GOParticulasFireball);
					instanciaPoder.setId(Cooldown.FIREBALL);
					instanciaPoder.tag = "Bloqueable";
					Vector3 v3 = new Vector3(hit.point.x, 0.5f, hit.point.z);
					v3 = v3-transform.position;
					v3 = v3.normalized;
					v3 = v3*600;
					v3.y=0.5f;
					instanciaPoder.gameObject.rigidbody.AddForce(v3);
					
					//Lanzamiento
					//instanciaPoder.Disparar(hit.point.x, hit.point.z );
					poderSeleccionado = false;
					//Evita que el personaje se mueva al punto de lanzamiento
					mov.NoMoverA(new Vector3(hit.point.x,0.5f,hit.point.z));
					mov.EncenderMovimiento();
					//Coloca el poder en cooldown
					if(instanciaPoder.getId() == Cooldown.FIREBALL){
						fireballOC = 0;
						poderesEnCool.PonerEnCooldown(Cooldown.FIREBALL, instanciaPoder.darCooldown());
					}
				}
				else if(Physics.Raycast(rayH, out hit, 50) && poderActual.name.Contains("Teleport") && teleportOC == 1)
				{
					Vector3 elVectorcito = new Vector3(hit.point.x, 0.5f ,hit.point.z);
					elVectorcito = elVectorcito-transform.position;
					GOParticulasTeleport = (GameObject)PhotonNetwork.Instantiate(ParticulasTeleport.name,transform.position, Quaternion.identity, 0);
					particulasActual = GOParticulasTeleport.transform;
					instanciaPoder.setParticulas(particulasActual, GOParticulasTeleport);
					if(elVectorcito.magnitude > instanciaPoder.distanciaLimite)
					{
						elVectorcito = elVectorcito.normalized;
						elVectorcito = elVectorcito*instanciaPoder.distanciaLimite;
						elVectorcito.y = 0.5f;
						transform.position += elVectorcito;
						elVectorcito = transform.position;
						elVectorcito.y = 0.5f;
						transform.position = elVectorcito;
					}
					else
					transform.position = new Vector3(hit.point.x, 0.5f ,hit.point.z);
					
					mov.NoMoverA(new Vector3(hit.point.x,0.5f,hit.point.z));
					mov.EncenderMovimiento();
					instanciaPoder.setId(Cooldown.TELEPORT);
					instanciaPoder.empezarTimer();
					poderSeleccionado = false;
					
					if(instanciaPoder.getId() == Cooldown.TELEPORT){
						teleportOC = 0;
						poderesEnCool.PonerEnCooldown(Cooldown.TELEPORT, instanciaPoder.darCooldown());
					}
				}
				else if(poderActual.name.Contains("Escudo") && escudoOC == 1)
				{
					mov.NoMoverA(new Vector3(hit.point.x,0.5f,hit.point.z));
					mov.EncenderMovimiento();
					instanciaPoder.empezarTimer();
					poderSeleccionado = false;
					if(instanciaPoder.getId() == Cooldown.ESCUDO){
						escudoOC = 0;
						poderesEnCool.PonerEnCooldown(Cooldown.ESCUDO, instanciaPoder.darCooldown());
					}
				}
				else if(poderActual.name.Contains("Bolt") && boltOC == 1){
					rayH.origin = new Vector3(transform.position.x, 0.5f , transform.position.z);
					Vector3 target = new Vector3(hit.point.x, 0.5f ,hit.point.z);
					rayH.direction = target;
					Physics.IgnoreCollision(this.collider, hit.collider);
					RaycastHit[] hits;
			        	hits = Physics.RaycastAll(rayH);
			        	int i = 0;
			        	while (i < hits.Length) {
			            RaycastHit hitss = hits[i];
			            Debug.Log ("tag Ray: " + hitss.collider.tag);
						Debug.Log ("tag Ray: " + hitss.transform.tag);
			            i++;
			        }
					bool collision = Physics.Raycast(rayH, out hit, 50);
					
					GOBolt = PhotonNetwork.Instantiate("BoltVacio",target, Quaternion.identity, 0);

					clon = PhotonNetwork.Instantiate(poderActual.name,transform.position, Quaternion.identity, 0);
					LightningBolt boltScript = (LightningBolt)clon.GetComponent(typeof(LightningBolt));
					boltScript.SetTarget(GOBolt.transform);

					instanciaPoder = (Poder)clon.GetComponent(typeof(Poder));
					instanciaPoder.setParticulas(GOBolt.transform, GOBolt);
					instanciaPoder.setCaster(this.gameObject);
					instanciaPoder.empezarTimer();
					instanciaPoder.Destroy(boltScript);
					instanciaPoder.setId(Cooldown.BOLT);
					if(collision)
					{
						GameObject colision = hit.collider.gameObject;
						// Aqui se aplica la fuerza
						Debug.Log("Le pego a algo: "+colision.name);
					}
					poderSeleccionado = false;
					//Evita que el personaje se mueva al punto de lanzamiento
					mov.NoMoverA(new Vector3(hit.point.x,0.5f,hit.point.z));
					mov.EncenderMovimiento();
					//Coloca el poder en cooldown
					if(instanciaPoder.getId() == Cooldown.BOLT){
						boltOC = 0;
						poderesEnCool.PonerEnCooldown(Cooldown.BOLT, instanciaPoder.darCooldown());
					}
				}
			}
		}
		//Debug.DrawLine(rayH.origin, rayH.direction, Color.cyan);
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
		else if(IDPoder == Cooldown.TELEPORT)
		{
			teleportOC = 1;
		}
		else if(IDPoder == Cooldown.ESCUDO)
		{
			escudoOC = 1;
		}
		else if(IDPoder == Cooldown.BOLT)
		{
			boltOC = 1;
		}
	}
	public bool getPoderSeleccionado()
	{
		return poderSeleccionado;
	}
}