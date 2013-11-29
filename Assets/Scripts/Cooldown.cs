using UnityEngine;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

[RequireComponent(typeof(PhotonView))]
/*
 * Este script controla el cooldown de todos los poderes de los personajes
 */
public class Cooldown : Photon.MonoBehaviour {
	
//------------------------------------------------------------------------
// Atributos
//------------------------------------------------------------------------
	
	/*
	 * Constantes para identificar todos los poderes existentes
	 */
	public const int FIREBALL = 0;
	public const int TELEPORT = 1;
	public const int ESCUDO = 2;
	public const int BOLT = 3;
	
	// Lista que maneja los poderes que se encuentran en cooldown
	float[] listaCooldowns = new float[10];
	
	
//-------------------------------------------------------------------------
// Metodos
//-------------------------------------------------------------------------
	
	void Update () {
		this.enabled = true;   // due to this, Update() is not called on the owner client.

		if (!photonView.isMine)
        {
            //MINE: local player, simply enable the local scripts
            this.enabled = false;
		}
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
				if(i == 0)
					SendMessage("FinCooldown", FIREBALL);
				else if(i == 1)
					SendMessage("FinCooldown", TELEPORT);
				else if(i == 2)
					SendMessage("FinCooldown", ESCUDO);
				else if(i == 3)
					SendMessage("FinCooldown", BOLT);
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
