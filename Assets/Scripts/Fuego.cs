using UnityEngine;
using System.Collections;

public class Fuego : MonoBehaviour {
	
	private bool enLava = false;
	
	public void FixedUpdate ()
	{
		Debug.Log("Esta en lava: " + enLava);
		if(!enLava){
			
			bool tierra = false;
			bool lava = false;
			RaycastHit[] hits = Physics.RaycastAll(transform.position,-Vector3.up,1);
		
			int i = 0;
			while(i < hits.Length){
				GameObject suelo = hits[i].transform.gameObject;
				if(suelo.CompareTag("Terreno")){
					tierra = true;
				}
				if(suelo.CompareTag("Lava"))
					lava = true;
				i++;
			}
		
			if(!tierra && lava){
				enLava = true;
				Quemar ();
			}
		}
	}
	
	public void Quemar(){
		//gameObject.renderer.enabled = false;
		Emitir em = (Emitir)GetComponentInChildren(typeof(Emitir));
		em.Activar();
	}
}