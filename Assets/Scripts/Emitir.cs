using UnityEngine;
using System.Collections;

public class Emitir : MonoBehaviour {
	
	private Component[] emitters;
	
	void Start () {
		emitters = GetComponentsInChildren(typeof(ParticleEmitter));	
	}
	
	public void Activar(){
		Debug.Log("Emitiendo");
		for(int i=0;i<emitters.Length;i++){
			ParticleEmitter actual = (ParticleEmitter)emitters[i];
			actual.emit = true;
		}
	}
}
