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
	
//-----------------------------------------------------------------
// Metodos
//-----------------------------------------------------------------
	
	void Update(){
		//Dispara el objeto si es posible
		if(disparar){
			//Mueve el poder hacia la posicion seleccionada con el mouse
			iTween.MoveTo(this.gameObject, iTween.Hash("position",new Vector3(mousePosX,0.5f,mousePosZ),"speed",velocidad));
			iTween.LookTo(this.gameObject, new Vector3(mousePosX,0,mousePosZ),0.1f);
		}
		
		// Si el poder llego a su destino, lo destruye
		if(transform.position.x == mousePosX && transform.position.z == mousePosZ){
			Destroy(this.gameObject);	
		}
	}
	
	/*
	 * Maneja la colision del poder con un personje
	 */
	void OnCollisionEnter(Collision collision){
		// Obtiene los componentes del objetivo
		GameObject objetivo = collision.gameObject;
		Vida vidaObjetivo = (Vida)objetivo.GetComponent(typeof(Vida));
		if (objetivo.CompareTag ("Enemigo")){
			//objetivo.rigidbody.AddForce(transform.forward*fuerza);
			
			//Calcula el vector destino de la colision
			Vector3 empuje = transform.forward;
			Vector3 actual = transform.position;
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
			//Mueve el objetivo al vector calculado
			Vector3 target = new Vector3(actual.x + addX, actual.y,actual.z + addZ);
			iTween.MoveTo(objetivo,iTween.Hash("position",target,"speed",fuerza));
			
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
			
			vidaObjetivo.hayDanio(dano); //Causa daño al objetivo
			Destroy(this.gameObject);	//Destruye el poder
		}
	}
	
	/*
	 * Determina la posicion hacia la cual disparar el poder
	 */
	public void Disparar(float PosX, float PosZ){
		mousePosX = PosX; 
   		mousePosZ = PosZ;
		disparar = true;
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