using UnityEngine;
using System.Collections;

public class PowerUpController : MonoBehaviour {

    private CharacterController pUpC;
    private Vector3 velocity;

    // Use this for initialization
    void Start () {
        velocity.y += 0.0f * Time.deltaTime;
        pUpC = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {
        velocity.y -= 15.0f * Time.deltaTime;
        pUpC.Move(velocity  * Time.deltaTime);
    }
}
