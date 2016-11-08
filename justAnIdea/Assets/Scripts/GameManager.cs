using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject player;
    private GameObject spawnPoint;
    private GameCamera cam;

	// Use this for initialization
	void Start () {
        cam = GetComponent<GameCamera>();
        spawnPoint = GameObject.FindGameObjectWithTag("Respawn");
        SpawnPlayer();
        Debug.Log("spawning player at spawn point");
        player.transform.position = spawnPoint.transform.position;
    }
	
	// Update is called once per frame
	private void SpawnPlayer()
    {
        //create the player object, and set the cameras position to the player, and lock it to follow. 
        cam.SetTarget((Instantiate(player, spawnPoint.transform.position, Quaternion.identity) as GameObject).transform);
    }
}
