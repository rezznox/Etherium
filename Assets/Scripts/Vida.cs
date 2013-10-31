using UnityEngine;
using System.Collections;

public class Vida : MonoBehaviour {
	
	private double vida;
	private bool  enLava;
	private bool recibeDanio;
	private bool recibeHealing;
	private double danioRecibido;
	private double healingRecibido;
	private float partesVida;
	// Use this for initialization
	void Start () {
		vida = 100;
		recibeDanio = false;
		recibeHealing = false;
		enLava = false;
		danioRecibido = 0;
		healingRecibido = 0;
		float aux = Screen.width-40;
		partesVida = aux/100;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(vida>100)
		{
			vida = 100;
		}
		else if(vida <=0)
		{
			Destroy (gameObject);
		}
		if(recibeDanio)
  		{
  			 vida -= danioRecibido;
   			recibeDanio = false;
  		}
  		if(recibeHealing)
  		{
  			 vida += healingRecibido;
  			 recibeHealing = false;
 		}
		
		
		
	}
	
	void OnGUI()
	{
		int vidaAux = (int) vida;
  		GUI.Box(new Rect(20, Screen.height-40, Screen.width-40, 25), vidaAux+"/100");
        GUI.Box(new Rect(20, Screen.height-40, (float)(vida * partesVida), 25), "");
	}
	
	public void FixedUpdate ()
	{
		bool tierra = false;
		bool lava = false;
		RaycastHit[] hits = Physics.RaycastAll(transform.position,-Vector3.up,1);
		
		int i = 0;
		while(i < hits.Length){
			GameObject suelo = hits[i].transform.gameObject;
			if(suelo.CompareTag("Terreno"))
				tierra = true;
			if(suelo.CompareTag("Lava"))
				lava = true;
			
			i++;
		}
		
		if(tierra && lava){
			enLava = false;
			SendMessage("EnLava",false);
		}
		else{
			enLava = true;
			SendMessage("EnLava",true);
		}
		
		if(enLava)
		{
			vida -= 0.2;
		}
	}
	
	public void estaEnLava()
	{
		enLava = true;
	}
	
	public void noEstaEnLava()
	{
		enLava = false;
	}
	
	public void hayDanio(double danio)
	{
		recibeDanio = true;
		danioRecibido = danio;
	}
	
	public void hayHealing(double healing)
	{
		recibeHealing = true;
		healingRecibido = healing;
	}
}
