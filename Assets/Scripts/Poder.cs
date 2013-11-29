using UnityEngine;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

[RequireComponent(typeof(PhotonView))]
/*
 * Controla un poder que haya sido disparado por un personaje
 */
public class Poder: Photon.MonoBehaviour{
	
//-----------------------------------------------------------------
// Atributos
//-----------------------------------------------------------------
	
	public float 		fuerza;							//Fuerza de empuje del poder		
	public float 		dano;							//Daño que ocaciona el poder
	public float 		velocidad;						//Velocidad de movimiento del poder
	public float 		cooldown;						//Tiempo de cooldown del poder
	
	private int 		Id;								//Identificador del poder
	private float 		mousePosX = 0;					//Posicion del poder en x 
    private float 		mousePosZ = 0;					//Posicion del poder en y
	
	private GameObject 	caster;							//Personaje que disparo el poder
	private bool 		disparar = false;				//Determina si puede disparar o no un poder
	private Vector3 	destino;
	
	private Transform 	particulas;
	private bool 		particulaInstanceadas = false;
	
	private bool timing;
	private float countdown;
	public float tiempo;
	
	public float distanciaLimite;
	private Vector3 posicionAnterior;
	public bool hayLimite;
	public Vector3 latestCorrectPos;
	public Quaternion latestCorretRot;
	public GameObject GOparticulas;
	
	private LightningBolt scriptLight;
	
//-----------------------------------------------------------------
// Metodos
//-----------------------------------------------------------------
	void Awake()
	{
		this.enabled = true;   // due to this, Update() is not called on the owner client.

		if (!photonView.isMine)
        {
            //MINE: local player, simply enable the local scripts
            this.enabled = false;
        }
		scriptLight = null;
	}
	void Update(){
		if(disparar){
			if(Id == Cooldown.FIREBALL){
				//transform.position = Vector3.MoveTowards(transform.position, destino, Time.deltaTime * 7);
			}
			else if(Id == Cooldown.TELEPORT){
			}
		}
		
		//if(mousePosX-transform.position.x> -10E-2 && mousePosX-transform.position.x < 10E-2
		//			&& mousePosZ-transform.position.z> -10E-2 && mousePosZ-transform.position.z < 10E-2)
		//{
		//	Destroy(this.gameObject);	
		//}
		
		//if(!particulaInstanceadas && particulas != null)
		//{
		//		Object objeto = Instantiate(particulas, transform.position, transform.rotation);
		//		Transform t = (Transform)objeto;
		//		particulaInstanceadas = true;
		//		t.parent = transform;
		//		t.name = "Fuego!";
		//}
	}
	
	/*
	 * Maneja la colision del poder con un personje
	 */
	void OnCollisionEnter(Collision collision){
		GameObject objetivo = collision.gameObject;
		Debug.Log ("tag: "+objetivo.tag);
		Vector3 posicionColision = transform.position;
		
		Vida vidaObjetivo = (Vida)caster.GetComponent(typeof(Vida));
		if (objetivo.CompareTag ("Jugador")){
			Vector3 target = new Vector3((objetivo.transform.position.x - posicionColision.x), objetivo.transform.position.y,(objetivo.transform.position.z - posicionColision.z));
			target = target.normalized;
			target = target*800;
			target.y = 0.5f;
			
			//iTween.MoveTo(objetivo, target, 10);
			//PhotonView pv = PhotonView.Get(this);
			//pv.RPC ("AplicarFuerza", PhotonTargets.All, target.x, target.z);
			Movimiento a = (Movimiento) caster.GetComponent(typeof(Movimiento));
			Debug.Log (""+objetivo.name);
			PhotonView b = objetivo.GetPhotonView();
			a.golpe(b, target);
			vidaObjetivo.danio(b, dano);
			if(GOparticulas!=null)
			PhotonNetwork.Destroy(GOparticulas);
			PhotonNetwork.Destroy(this.gameObject);
		}
		else if(objetivo.CompareTag ("Arbol") && !this.name.Equals("Escudo")){
			Debug.Log("Golpee un arbol");
			PhotonNetwork.Destroy(this.gameObject);
		}
		else if(objetivo.CompareTag("Bloqueable"))
		{
			Debug.Log("Angulo colision: ");
			if(objetivo.name.Equals("Rayo"))
			{
			}
			else if(objetivo.name.Contains("Fireball"))
			{
				Vector3 velo = objetivo.rigidbody.velocity;
				Vector3 vectorPerpendicular = velo-transform.position;
				vectorPerpendicular.y = 0.5f;
				float angulo = Mathf.Acos(Vector3.Dot(velo.normalized, vectorPerpendicular.normalized));
				Debug.Log("Angulo colision: "+angulo);
			}
		}
	}
	
	/*
	 * Determina la posicion hacia la cual disparar el poder
	 */
	public void Disparar(float PosX, float PosZ){
		mousePosX = PosX; 
   		mousePosZ = PosZ;
		disparar = true;
		destino = new Vector3(PosX, 0, PosZ);
	}
	
//----------------------------------------------------------------------
// Getters y Setters
//----------------------------------------------------------------------
	
	public float darFuerza(){
		return fuerza;	
	}
	
	public float darDano(){
		return dano;	
	}
	
	public float darVelocidad(){
		return velocidad;	
	}
	
	public float darCooldown(){
		return cooldown;	
	}
	
	public void setCaster(GameObject nCaster){
		caster = nCaster;
		posicionAnterior = transform.position;
	}
	
	public void setParticulas(Transform sistema, GameObject go)
	{
		particulas = sistema;
		GOparticulas = go;
	}
	
	public void setId(int id){
	 	Id = id;	
	}
	
	public int getId(){
		return Id;	
	}
	public void empezarTimer()
	{
		timing = true;
		countdown = 0;
	}
	public void Destroy(LightningBolt c)
	{
		 scriptLight = c;
	}
	void FixedUpdate()
	{
		if(timing)
		{
			//Time.deltaTime es el tiempo que se demora unity en hacer un update
			countdown += Time.deltaTime;
			if((countdown) > tiempo)
			{
				if(GOparticulas!=null)
				PhotonNetwork.Destroy(GOparticulas);
				PhotonNetwork.Destroy(gameObject);
				if(scriptLight != null)
				{
					scriptLight.enabled = false;
				}
			}
		}
		//parte donde se le impone un limite de distancia al spell
		if(hayLimite)
		{
			distanciaLimite -= Vector3.Distance(posicionAnterior, transform.position);
			posicionAnterior = transform.position;
			if(distanciaLimite <= 0)
			{
				PhotonNetwork.Destroy(GOparticulas);
				PhotonNetwork.Destroy(gameObject);
			}
		}
	}
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