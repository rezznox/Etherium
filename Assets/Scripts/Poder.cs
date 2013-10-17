using UnityEngine;
using System.Collections;

public class Poder: MonoBehaviour{
	
	public float fuerza;
	public float dano;
	public float velocidad;
	
	private Ray rayH;
	private RaycastHit hit;
	private float mousePosX = 0; 
    private float mousePosZ = 0;
	
	private GameObject caster;
	private bool disparar = false;
	
	void Update(){
		if(disparar){
			iTween.MoveTo(this.gameObject, iTween.Hash("position",new Vector3(mousePosX,0,mousePosZ),"speed",velocidad));
			iTween.LookTo(this.gameObject, new Vector3(mousePosX,0,mousePosZ),0.1f);
		}
		
		if(transform.position.x == mousePosX && transform.position.z == mousePosZ){
			Destroy(this.gameObject);	
		}
	}
	
	void OnCollisionEnter(Collision collision){
		GameObject objetivo = collision.gameObject;
		Vida vidaObjetivo = (Vida)objetivo.GetComponent(typeof(Vida));
		if (objetivo.CompareTag ("Enemigo")){
			objetivo.rigidbody.AddForce(transform.forward*fuerza);
			vidaObjetivo.hayDanio(dano);
			this.renderer.enabled = false;
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
	
	public void setCaster(GameObject nCaster){
		caster = nCaster;	
	}
}
