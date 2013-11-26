using UnityEngine;
using System.Collections;

/*
 * Controla un poder que haya sido disparado por un personaje
 */
public class Poder: MonoBehaviour{
	
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
	
//-----------------------------------------------------------------
// Metodos
//-----------------------------------------------------------------
	
	void Update(){
		if(disparar){
			if(Id == Cooldown.FIREBALL){
				//transform.position = Vector3.MoveTowards(transform.position, destino, Time.deltaTime * 7);
			}
			else if(Id == Cooldown.TELEPORT){
				Debug.Log("Teleport!!!!!");
			}
		}
		
		//if(mousePosX-transform.position.x> -10E-2 && mousePosX-transform.position.x < 10E-2
		//			&& mousePosZ-transform.position.z> -10E-2 && mousePosZ-transform.position.z < 10E-2)
		//{
		//	Destroy(this.gameObject);	
		//}
		
		if(!particulaInstanceadas && particulas != null)
		{
				Object objeto = Instantiate(particulas, transform.position, transform.rotation);
				Transform t = (Transform)objeto;
				particulaInstanceadas = true;
				t.parent = transform;
				t.name = "Fuego!";
		}
	}
	
	/*
	 * Maneja la colision del poder con un personje
	 */
	void OnCollisionEnter(Collision collision){
		GameObject objetivo = collision.gameObject;
		Vector3 posicionColision = transform.position;
		posicionColision = (objetivo.transform.position-posicionColision);
		
		Vida vidaObjetivo = (Vida)objetivo.GetComponent(typeof(Vida));
		if (objetivo.CompareTag ("Jugador")){
			Vector3 target = new Vector3((objetivo.transform.position.x + posicionColision.x*4), objetivo.transform.position.y,(objetivo.transform.position.z + posicionColision.z*4));
			//iTween.MoveTo(objetivo, target, 10);
			vidaObjetivo.hayDanio(dano);
			Destroy(this.gameObject);
		}
		else if(objetivo.CompareTag ("Arbol")){
			Debug.Log("Golpee un arbol");	
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
	
	public void setParticulas(Transform sistema)
	{
		particulas = sistema;
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
	void FixedUpdate()
	{
		if(timing)
		{
			//Time.deltaTime es el tiempo que se demora unity en hacer un update
			countdown += Time.deltaTime;
			if((countdown) > tiempo)
			{
				Destroy(gameObject);
			}
		}
		//parte donde se le impone un limite de distancia al spell
		if(hayLimite)
		{
			distanciaLimite -= Vector3.Distance(posicionAnterior, transform.position);
			posicionAnterior = transform.position;
			if(distanciaLimite <= 0)
				Destroy(gameObject);
		}
	}
}