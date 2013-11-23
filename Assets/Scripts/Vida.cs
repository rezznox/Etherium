using UnityEngine;
using System.Collections;

/*
 * Administra la vida de un personaje
 */
public class Vida : MonoBehaviour {
	
//----------------------------------------------------------------------
// Atributos
//----------------------------------------------------------------------
	
	public 	GUISkin		skin;
	private double 		vida;				//Vida total del personaje
	private bool  		enLava;				//Determina si el personaje esta sobre lava o no
	private bool 		recibeDanio;		//Determina si un personaje esta reciviendo daño o no
	private bool 		recibeHealing;		//Determina si un peersonaje se esta curando o no
	private double 		danioRecibido;		//Determina el total de daño recivido
	private double 		healingRecibido;	//Determina el total de vida curada
	private float 		partesVida;			//Division del HUD de vida
//----------------------------------------------------------------------
// Metodos
//----------------------------------------------------------------------
	
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
	
	void Update () {
		
		if(vida>100)   //Limita el nivel de vida a 100
		{
			vida = 100;
		}
		else if(vida <=0) //Destruye al personaje si la vida llega a 0
		{
			Destroy (gameObject);
		}
		if(recibeDanio) //Reduce el nivel de vida
  		{
  			 vida -= danioRecibido;
   			recibeDanio = false;
  		}
  		if(recibeHealing) //Aumenta el nivel de vida
  		{
  			 vida += healingRecibido;
  			 recibeHealing = false;
 		}
	}
	
	/*
	 * Dibuja la barra de vida en pantalla
	 */
	void OnGUI()
	{
		GUI.skin = skin;
		int vidaAux = (int) vida;
  		//GUI.Label(new Rect(20, Screen.height-40, Screen.width-40, 25), vidaAux+"/100");
        GUI.Box(new Rect(20, Screen.height-40, (float)(vida * partesVida), 25), "");
	}
	
	/*
	 * Causa daños periodicos si el personaje esta en lava
	 */
	public void FixedUpdate ()
	{
		bool tierra = false;
		bool lava = false;
		//Raycast para determinar los objetos de abajo
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
		
		//Determina si se esta en lava o no
		if(tierra && lava){
			enLava = false;
			SendMessage("EnLava",false);
		}
		else{
			enLava = true;
			SendMessage("EnLava",true);
		}
		
		//Causa daños periodicamente si esta en lava
		if(enLava)
		{
			vida -= 0.2;
		}
	}
	
	//Determina si se encuntra en lava
	public void estaEnLava()
	{
		enLava = true;
	}
	
	//Determina si se encuentra sobre terreno
	public void noEstaEnLava()
	{
		enLava = false;
	}
	
	//Determina la cantidad de daño causado
	public void hayDanio(double danio)
	{
		recibeDanio = true;
		danioRecibido = danio;
	}
	
	//Determina la cantidad de daño curado
	public void hayHealing(double healing)
	{
		recibeHealing = true;
		healingRecibido = healing;
	}
}