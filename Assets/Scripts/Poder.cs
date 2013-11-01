using UnityEngine;
using System.Collections;

/*
 * Controla un poder que haya sido disparado por un personaje
 */
public class Poder: MonoBehaviour{
	
//-----------------------------------------------------------------
// Atributos
//-----------------------------------------------------------------
	
	public float 		fuerza;				//Fuerza de empuje del poder		
	public float 		dano;				//Daño que ocaciona el poder
	public float 		velocidad;			//Velocidad de movimiento del poder
	public float 		cooldown;			//Tiempo de cooldown del poder
	
	private float 		mousePosX = 0;		//Posicion del poder en x 
    private float 		mousePosZ = 0;		//Posicion del poder en y
	
	private GameObject 	caster;				//Personaje que disparo el poder
	private bool 		disparar = false;	//Determina si puede disparar o no un poder
	private Vector3 destino;
	
//-----------------------------------------------------------------
// Metodos
//-----------------------------------------------------------------
	
	void Update(){
		if(disparar){
			transform.position = Vector3.MoveTowards(transform.position, destino, Time.deltaTime * 7);
		}
		
		if(mousePosX-transform.position.x> -10E-2 && mousePosX-transform.position.x < 10E-2
					&& mousePosZ-transform.position.z> -10E-2 && mousePosZ-transform.position.z < 10E-2)
		{
			Destroy(this.gameObject);	
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
		if (objetivo.CompareTag ("Enemigo")){
			//objetivo.rigidbody.AddForce(transform.forward*fuerza);
		/*
			Vector3 empuje = transform.forward;
			Vector3 actual = transform.position;
			
			Debug.Log(empuje);
			float addX = 0.0f;
			float addZ = 0.0f;
			
			if(empuje.x <0.0f)
				addX-=fuerza/2;
			if(empuje.x <= -1.0f)
				addX-=fuerza/2;
			if(empuje.z < 0.0f)
				addZ-=fuerza/2;
			if(empuje.z <= -1.0f)
				addZ-=fuerza/2;
			
			
			if(empuje.x >0.0f)
				addX+=fuerza/2;
			if(empuje.x >= 1.0f)
				addX+=fuerza/2;
			if(empuje.z > 0.0f)
				addZ+=fuerza/2;
			if(empuje.z >= 1.0f)
				addZ+=fuerza/2;
			*/
			Vector3 target = new Vector3((objetivo.transform.position.x + posicionColision.x*4), objetivo.transform.position.y,(objetivo.transform.position.z + posicionColision.z*4));
			iTween.MoveTo(objetivo, target, 20);
			//iTween.mo
			/*if(empuje.x < 0.0f ){
				Debug.Log("X -1");
				Vector3 target = new Vector3(actual.x - fuerza, actual.y,actual.z);
				iTween.MoveTo(objetivo,iTween.Hash("position",target,"speed",fuerza));
			}
			else if(empuje.z < 0.0f ){
				Debug.Log("Z -1");
				Vector3 target = new Vector3(actual.x, actual.y,actual.z - fuerza);
				iTween.MoveTo(objetivo,iTween.Hash("position",target,"speed",fuerza));
			}
			else if(empuje.x == 1 ){
				Debug.Log("X 1");
				Vector3 target = new Vector3(actual.x + fuerza, actual.y,actual.z);
				iTween.MoveTo(objetivo,iTween.Hash("position",target,"speed",fuerza));
			}
			else if(empuje.z >= 1 ){
				Debug.Log("Z 1");
				Vector3 target = new Vector3(actual.x, actual.y,actual.z + fuerza);
				iTween.MoveTo(objetivo,iTween.Hash("position",target,"speed",fuerza));
			}*/
			
			vidaObjetivo.hayDanio(dano);
			Destroy(this.gameObject);
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
	}
}