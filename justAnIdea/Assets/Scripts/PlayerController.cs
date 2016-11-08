using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour {

    //Movement Variables
    private float gravity = 70;
    private float speed = 12;
    private float acceleration = 60;
    private float jumpHeight = 20;

    private int maxJumps = 2;
    private int currentJumps = 0;

    private PlayerPhysics playerPhysics;

    private SpriteRenderer spriteRenderer;

    private float currentSpeed;
    private float targetSpeed;
    private bool canJumpAgain;

    private Vector2 amountToMove;

	// Use this for initialization
	void Start () {
        playerPhysics = GetComponent<PlayerPhysics>();
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        if (playerPhysics.movementStopped)
        {
            targetSpeed = 0;
            currentSpeed = 0;
        }

        targetSpeed = Input.GetAxisRaw("Horizontal") * speed;
        currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);

        //flip sprite direction to reflect which direction you're moving
        if (targetSpeed < 0)
        {
            spriteRenderer.flipX = true;
        }
        if (targetSpeed > 0)
        {
            spriteRenderer.flipX = false;
        }

        if (playerPhysics.grounded)
        {
            amountToMove.y = 0;
            currentJumps = 0;
            //Jump
            if (Input.GetButtonDown("Jump"))
            {
                amountToMove.y = jumpHeight;
                currentJumps += 1;
                canJumpAgain = false;
            }
        }

        if (Input.GetButtonUp("Jump") && canJumpAgain == false) //in air or on ground, if jump button is released, set bool for double jump availability to true
        {
            canJumpAgain = true;
        }

        if (canJumpAgain == true && currentJumps < maxJumps && Input.GetButtonDown("Jump")) //Double Jump
        {
            amountToMove.y = jumpHeight;
            currentJumps += 1;
        }
        
        amountToMove.x = currentSpeed;
        amountToMove.y -= gravity * Time.deltaTime;
        playerPhysics.Move(amountToMove * Time.deltaTime);
	}

    //Increase n towards target by speed
    private float IncrementTowards(float n, float target, float a)
    {
        if (n == target)
        {
            return n;
        }
        else
        {
            float dir = Mathf.Sign(target - n); //must be increased or decreased to get  closer to target
            n += a * Time.deltaTime * dir;
            return (dir == Mathf.Sign(target - n)) ? n : target; //if n has passed target, return target, otherwise return n
        }
    }
}
