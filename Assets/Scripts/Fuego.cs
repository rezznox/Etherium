using UnityEngine;
using System.Collections;

/*
 * Script que detecta si un elemento esta en lava para encender fuego
 */
public class Fuego : MonoBehaviour {
	
//--------------------------------------------------------------------
// Atributos
//--------------------------------------------------------------------
	
	/*
	 * Determina si el objeto se encunetra sobre lava o no
	 */
	private bool enLava = false;
	
//--------------------------------------------------------------------
// Metodos
//--------------------------------------------------------------------
	
	public void FixedUpdate ()
	{
		if(!enLava){
			
			bool tierra = false;
			bool lava = false;
			// Lanza un RayCast hacia abajo para dtectar collliders
			RaycastHit[] hits = Physics.RaycastAll(transform.position,-Vector3.up,1);
		
			int i = 0;
			// Identifica con que objetos colisiono 
			while(i < hits.Length){
				GameObject suelo = hits[i].transform.gameObject;
				if(suelo.CompareTag("Terreno")){
					tierra = true;
				}
				if(suelo.CompareTag("Lava"))
					lava = true;
				i++;
			}
			// Cambia el estado si unicamente esta sobre lava
			if(!tierra && lava){
				enLava = true;
				Quemar ();
			}
		}
	}
	
	/*
	 * Metodo que da la orden de emitir el fuego
	 */
	public void Quemar(){
		//gameObject.renderer.enabled = false;
		Emitir em = (Emitir)GetComponentInChildren(typeof(Emitir));
		em.Activar();
	}
}