using UnityEngine;
using System.Collections;

public class Movimiento : MonoBehaviour {

	// Use this for initialization
	
	public float velocidad;
	private bool movPermitido = true;
	private float mousePosX = 0; 
    private float mousePosZ = 0;
	private Ray rayH;
	private RaycastHit hit;
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
			if(Input.GetMouseButtonDown(0))
			{
					rayH = Camera.main.ScreenPointToRay (Input.mousePosition);
					if(Physics.Raycast(rayH, out hit, 50))
					{
						mousePosX = hit.point.x; 
		    			mousePosZ = hit.point.z;
					}
			}
			if(mousePosX != 0 && mousePosZ != 0 && movPermitido){
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(mousePosX, 0, mousePosZ), Time.deltaTime * velocidad);
				transform.LookAt(new Vector3(mousePosX,0, mousePosZ));
			}
	}
	
	public void ApagarMovimiento(){
		movPermitido = false;	
	}
	
	public void EncenderMovimiento(){
		mousePosX = 0;
		mousePosZ = 0;
		movPermitido = true;
	}
}
