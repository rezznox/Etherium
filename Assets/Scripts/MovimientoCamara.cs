using UnityEngine;
using System.Collections;

public class MovimientoCamara : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	 
	}
 
	// Update is called once per frame
	void Update () {
 
    float mousePosX = Input.mousePosition.x; 
    float mousePosY = Input.mousePosition.y; 
    int scrollDistance = 5; 
    float scrollSpeed = 45;
 
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

