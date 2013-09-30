using UnityEngine;
using System.Collections;

public class Movimiento : MonoBehaviour {

	// Use this for initialization
	private float mousePosX; 
    private float mousePosY;
	private Ray rayH;
	private RaycastHit hit;
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0))
		{
			rayH = Camera.main.ScreenPointToRay (Input.mousePosition);
			if(Physics.Raycast(rayH, out hit, 50))
			{
				mousePosX = hit.point.x; 
		    	mousePosY = hit.point.y;
			}
		}
		transform.position = Vector3.Lerp(transform.position, new Vector3(mousePosX, mousePosY, 0), Time.deltaTime * 4);

	}
}
