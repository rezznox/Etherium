using UnityEngine;
using System.Collections;

public class ConnectionManager : MonoBehaviour {

	public GameObject playerPrefab1;
	public GameObject playerPrefab2;
	
	private Movimiento mo;
	// Use this for initialization
	void Start () {
		ConnectToServer();
		instancear();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void ConnectToServer() {
        if (PhotonNetwork.connectionState == ConnectionState.Disconnected)
        {
                PhotonNetwork.ConnectUsingSettings("1");
        }
    }
	void instancear()
	{
		if(PhotonNetwork.room.playerCount <=1)
		{
			//instancear jugador
			PhotonNetwork.Instantiate(this.playerPrefab1.name, new Vector3(7.785223f, 0.5f, -7.0f), Quaternion.identity, 0);
			Debug.Log("isntanceo primero");
		}
		else
		{
			PhotonNetwork.Instantiate(this.playerPrefab2.name, new Vector3(-7.88836f, 0.5f, -7.247649f), Quaternion.identity, 0);
			Debug.Log("isntanceo segundo");
			mo = playerPrefab1.GetComponent<Movimiento>();
			mo.enabled = false;
		}
	}
}
