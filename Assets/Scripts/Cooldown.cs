using UnityEngine;
using System.Collections;

/*
 * Este script controla el cooldown de todos los poderes de los personajes
 */
public class Cooldown : MonoBehaviour {
	
//------------------------------------------------------------------------
// Atributos
//------------------------------------------------------------------------
	
	/*
	 * Constantes para identificar todos los poderes existentes
	 */
	public const int FIREBALL = 0;
	
	// Lista que maneja los podres que se encuentran en cooldown
	float[] listaCooldowns = new float[10];
	
	
//-------------------------------------------------------------------------
// Metodos
//-------------------------------------------------------------------------
	
	void Update () {
		int i = 0;
		// Recorre toda la lista de poderes y reduce el tiempo de cooldown
		// de cada uno.
		while(i < listaCooldowns.Length){
			float remain = listaCooldowns[i];
			if(remain > 0){//Reduce el tiempo de cada poder
				remain -= Time.deltaTime;
				listaCooldowns[i] = remain;
			}
			else if(remain < 0){//Saca de a lista los poderes que han terminado
				remain = 0;
				listaCooldowns[i] = remain;
				SendMessage("FinCooldown", FIREBALL);
			}
			i++;
		}
	}
	
	/*
	 * Añade un poder a la lista de cooldown
	 */
	public void PonerEnCooldown(int IDPoder, float cooldown){
		listaCooldowns[IDPoder] = cooldown;
	}
}
