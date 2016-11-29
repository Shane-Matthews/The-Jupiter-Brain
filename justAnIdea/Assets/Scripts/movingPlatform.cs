using UnityEngine;
using System.Collections;

public class movingPlatform : MonoBehaviour {

    float moveAmount = 0.08f;
    float amountMoved = 0;
    public float amountToMove = 0.5f;

    bool moveRight;

	// Use this for initialization
	void Start () {
        moveRight = true;
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(amountMoved);
        if (moveRight)
        {
            amountMoved += Time.deltaTime / 4;
            transform.position = new Vector3(transform.position.x + moveAmount, transform.position.y, transform.position.z);
            if (amountMoved >= amountToMove)
            {
                moveRight = false;
            }
        }
        else if (!moveRight)
        {
            amountMoved -= Time.deltaTime / 4;
            transform.position = new Vector3(transform.position.x - moveAmount, transform.position.y, transform.position.z);
            if (amountMoved <= 0)
            {
                moveRight = true;
            }
        }

        
	}
}
