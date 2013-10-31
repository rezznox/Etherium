using UnityEngine;
using System.Collections;

/*
 * Inicia un foco de fuego
 */
public class Emitir : MonoBehaviour {
	
//-----------------------------------------------------------------------------
// Atributos
//-----------------------------------------------------------------------------
	
	// Guarda todos los ParticleEmitters que hacen parte del prefab de particulas
	private Component[] emitters;
	
//------------------------------------------------------------------------------
// Metodos
//------------------------------------------------------------------------------
	
	/*
	 * Busca todos los Emitters que hagan parte del sistema de particulas
	 */
	void Start () {
		emitters = GetComponentsInChildren(typeof(ParticleEmitter));	
	}
	
	/*
	 * Activa la emision de todos los emitters encontrados
	 */
	public void Activar(){
		Debug.Log("Emitiendo");
		for(int i=0;i<emitters.Length;i++){
			ParticleEmitter actual = (ParticleEmitter)emitters[i];
			actual.emit = true;
		}
	}
}