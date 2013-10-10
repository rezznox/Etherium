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
				GameObject clon = (GameObject)Instantiate(Fireball,transform.position, transform.rotation);
				Poder p = (Poder)clon.GetComponent(typeof(Poder));
				
				rayH = Camera.main.ScreenPointToRay (Input.mousePosition);
				if(Physics.Raycast(rayH, out hit, 50))
				{
					p.Disparar(hit.point.x, hit.point.z );
				}
				
				poderSeleccionado = false;
				mov.EncenderMovimiento();
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
