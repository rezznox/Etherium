﻿using UnityEngine;
using System.Collections;

public class Movimiento : MonoBehaviour {

	// Use this for initialization
	
	public float velocidad;
	private bool movPermitido = true;
	private float mousePosX = 0; 
    private float mousePosZ = 0;
	private Ray rayH;
	private RaycastHit hit;
	private Vector3 noMover;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
			if(movPermitido){
				if(Input.GetMouseButtonDown(0))
				{
						rayH = Camera.main.ScreenPointToRay (Input.mousePosition);
						//EncenderMovimiento();
						if(Physics.Raycast(rayH, out hit, 50))
						{
							mousePosX = hit.point.x; 
		    				mousePosZ = hit.point.z;
						}
				}
				if(mousePosX != 0 && mousePosZ != 0){
					Vector3 destino = new Vector3(mousePosX, 0, mousePosZ);
					if(!destino.Equals(noMover)){
						transform.position = Vector3.MoveTowards(transform.position, new Vector3(mousePosX, 0, mousePosZ), Time.deltaTime * velocidad);
						Quaternion newRotation = Quaternion.LookRotation(transform.position - new Vector3(mousePosX, 0, mousePosZ), Vector3.up);
						transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 5);
					}
				}
				if(mousePosX-transform.position.x> -10E-2 && mousePosX-transform.position.x < 10E-2
					&& mousePosZ-transform.position.z> -10E-2 && mousePosZ-transform.position.z < 10E-2)
				{
					Debug.Log("se apago el movimiento");
					//ApagarMovimiento();
				}
			}
	}
	
	public void ApagarMovimiento(){
		Debug.Log("movimiento apagado");
		movPermitido = false;	
	}
	
	public void EncenderMovimiento(){
		Debug.Log("movimiento encendido");
		mousePosX = 0;
		mousePosZ = 0;
		movPermitido = true;
	}
	
	public void NoMoverA(Vector3 no){
			noMover = no;
	}
}
