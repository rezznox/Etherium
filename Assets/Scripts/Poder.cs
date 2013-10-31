using UnityEngine;
using System.Collections;

public class Poder: MonoBehaviour{
	
	public float fuerza;
	public float dano;
	public float velocidad;
	public float cooldown;
	
	private Ray rayH;
	private RaycastHit hit;
	private float mousePosX = 0; 
    private float mousePosZ = 0;
	
	private GameObject caster;
	private bool disparar = false;
	
	void Update(){
		if(disparar){
			iTween.MoveTo(this.gameObject, iTween.Hash("position",new Vector3(mousePosX,0.5f,mousePosZ),"speed",velocidad));
			iTween.LookTo(this.gameObject, new Vector3(mousePosX,0,mousePosZ),0.1f);
		}
		
		if(transform.position.x == mousePosX && transform.position.z == mousePosZ){
			Destroy(this.gameObject);	
		}
	}
	
	void OnCollisionEnter(Collision collision){
		Debug.Log("Boooom");
		GameObject objetivo = collision.gameObject;
		Vida vidaObjetivo = (Vida)objetivo.GetComponent(typeof(Vida));
		if (objetivo.CompareTag ("Enemigo")){
			//objetivo.rigidbody.AddForce(transform.forward*fuerza);
		
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
			
			vidaObjetivo.hayDanio(dano);
			Destroy(this.gameObject);
		}
	}
	
	public void Disparar(float PosX, float PosZ){
		mousePosX = PosX; 
   		mousePosZ = PosZ;
		disparar = true;
	}
	
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
