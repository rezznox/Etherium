using UnityEngine;
using System.Collections;

public class ConnectionManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ConnectToServer();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void ConnectToServer() {
        Network.Connect("127.0.0.1", 123, "QwErTy");
		Debug.Log ("trato de conectarse");
    }
}
