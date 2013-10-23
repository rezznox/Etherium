using UnityEngine;
using System.Collections;

public class PoderesPersonaje : MonoBehaviour {
	
	public GameObject Fireball;
	
	private Transform cast;
	private GameObject poderActual;
	private bool poderSeleccionado = false;
	private Vector3 posDestino;
	private Cooldown poderesEnCool;
	
	private Movimiento mov;
	private Ray rayH;
	private RaycastHit hit;
	
	private int fireballOC = 1;
	// Use this for initialization
	void Start () {
		mov = (Movimiento)GetComponent(typeof(Movimiento));
		poderesEnCool = (Cooldown)GetComponent(typeof(Cooldown));
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			if(poderSeleccionado){
				//Crea el poder
				GameObject clon = (GameObject)Instantiate(poderActual,transform.position, transform.rotation);
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
					fireballOC = 0;
					mov.NoMoverA(new Vector3(hit.point.x,0,hit.point.z));
					mov.EncenderMovimiento();
					poderesEnCool.PonerEnCooldown(Cooldown.FIREBALL, p.darCooldown());
				}		
			}
		}
		
		if(Input.GetKeyDown(KeyCode.Q)){
			if(fireballOC == 1){
				poderActual = Fireball;
				poderSeleccionado = true;
				mov.ApagarMovimiento();
				Debug.Log("Poder seleccionado");
			}
		}
	}
	
	void OnGUI()
	{
  		GUI.Box(new Rect(20, Screen.height-80,25, 25), "Fire");
        GUI.Box(new Rect(20*fireballOC, (Screen.height-80)*fireballOC, 25*fireballOC, 25*fireballOC), "");
	}
	
	public void FinCooldown(int IDPoder){
		if(IDPoder == Cooldown.FIREBALL){
			fireballOC = 1;	
		}
	}
}
