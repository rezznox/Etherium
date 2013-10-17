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
		GUI.Box(new Rect(20, Screen.height-40, Screen.width-40, 25), vida+"/100");
        GUI.Box(new Rect(20, Screen.height-40, (float)(vida * partesVida), 25), "");
	}
	
	public void FixedUpdate ()
	{
		if(enLava)
		{
			//gozala
			vida -= 0.01;
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
