using UnityEngine;
using System.Collections;

/*
 * Maneja el movimeinto de la camara
 */
public class MovimientoCamara : MonoBehaviour
{

//------------------------------------------------------------------
// Metodos
//------------------------------------------------------------------
	
	void Update () {
		//Determina la posicion del mouse en un momento dado
    	float mousePosX = Input.mousePosition.x; 
    	float mousePosY = Input.mousePosition.y;
		
		//Constantes de movimiento
    	int scrollDistance = 5; 
   		float scrollSpeed = 45;
 		
		//Mueve la camara con respecto a la posicion del mouse
    	if (mousePosX < scrollDistance) 
        { 
        	transform.Translate(Vector3.right * -scrollSpeed * Time.deltaTime); 
        } 
 
    	if (mousePosX >= Screen.width - scrollDistance) 
        { 
        	transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime); 
        }
 
    	if (mousePosY < scrollDistance) 
        { 
        	transform.Translate(new Vector3(0,1,0.6f) * -scrollSpeed * Time.deltaTime); 
        } 
 
    	if (mousePosY >= Screen.height - scrollDistance) 
        { 
        	transform.Translate(new Vector3(0,1,0.6f) * scrollSpeed * Time.deltaTime); 
        }
    }
}