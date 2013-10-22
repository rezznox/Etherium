using UnityEngine;
using System.Collections;

public class PoderesPersonaje : MonoBehaviour {
	
	public GameObject Fireball;
	
	private Transform cast;
	private Poder poderActual;
	private bool poderSeleccionado = false;
	private Vector3 posDestino;
	private Poder fireball;
	
	private Movimiento mov;
	private Ray rayH;
	private RaycastHit hit;
	
	// Use this for initialization
	void Start () {
		mov = (Movimiento)GetComponent(typeof(Movimiento));
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			if(poderSeleccionado){
				//Crea el poder
				GameObject clon = (GameObject)Instantiate(Fireball,transform.position, transform.rotation);
				Poder p = (Poder)clon.GetComponent(typeof(Poder));
				p.setCaster(this.gameObject);
				Physics.IgnoreCollision(clon.collider,this.gameObject.collider);
				
				//Lanza el poder
				rayH = Camera.main.ScreenPointToRay (Input.mousePosition);
				if(Physics.Raycast(rayH, out hit, 50))
				{
					
					//Rota hacia el objetivo
					//Quaternion newRotation = Quaternion.LookRotation(new Vector3(hit.point.x, 0, hit.point.z) - transform.position, Vector3.forward);
					//newRotation.x = 0;
					//newRotation.z = 0;
					//transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 5);
					
					Vector3 newRotation = new Vector3(hit.point.x, 0, hit.point.z);
					transform.LookAt(newRotation);
					
					//Lanzamiento
					p.Disparar(hit.point.x, hit.point.z );
					poderSeleccionado = false;
					mov.NoMoverA(new Vector3(hit.point.x,0,hit.point.z));
					mov.EncenderMovimiento();
				}
				
				
			}
		}
		
		if(Input.GetKeyDown(KeyCode.Q)){
			poderActual = fireball;
			poderSeleccionado = true;
			mov.ApagarMovimiento();
			Debug.Log("Poder seleccionado");
		}
	}
}
