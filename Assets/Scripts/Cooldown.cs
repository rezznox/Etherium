using UnityEngine;
using System.Collections;

public class Cooldown : MonoBehaviour {
	
	public const int FIREBALL = 0;
	
	float[] listaCooldowns = new float[10];
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		int i = 0;
		while(i < listaCooldowns.Length){
			float remain = listaCooldowns[i];
			if(remain > 0){
				remain -= Time.deltaTime;
				listaCooldowns[i] = remain;
			}
			else if(remain < 0){
				remain = 0;
				listaCooldowns[i] = remain;
				SendMessage("FinCooldown", FIREBALL);
			}
			i++;
		}
	}
	
	public void PonerEnCooldown(int IDPoder, float cooldown){
		listaCooldowns[IDPoder] = cooldown;
		Debug.Log("Puse " + IDPoder + " en cooldown");
	}
}
