using UnityEngine;
using System.Collections;

/*This class provides a movement system that has a Seeking algorithm
 * The enemy will detect the player, and when in range, will move towards them
 * when they are close enough, they will stop. futher implimentations can be made
 */

public class EnemyController : MonoBehaviour {

    /*Variable declairation. All these variables are used in the
     * movement calculations, or for holding player movement or
     * lists of game objects*/

    public SpriteRenderer spriteRenderer;
    private GameObject target;
    private CharacterController eController;
    private playerController pController;

    public float mass;
    public float speed = 8;

    //maximum variables to control movement
    public float maxSpeed;
    public float maxForce;
    public float maxVelocity;

    private float approachRadius = 14;

    private Transform targetTransform;
    private Vector3 velocity;

    private Vector3 targetDifference;
    private float targetDistance;

    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.FindWithTag("Player");
        eController = GetComponent<CharacterController>();
        pController = target.GetComponent<playerController>();
    }
	
	// Update is called once per frame
	void Update () {
        targetTransform = target.transform;

        //get distance from the player
        targetDifference = targetTransform.position - transform.position;
        targetDistance = targetDifference.magnitude;

        //moves the position
        velocity.y -= 1000.0f * Time.deltaTime;
        eController.Move(velocity * speed * Time.deltaTime);
        float distance = Vector3.Distance(transform.position, targetTransform.position);
        if (distance < approachRadius && distance > 1.5)
        {
            if (pController.isGrounded)
            {
                seek();
            }
        }
        else
        {
            velocity = Vector3.zero;
        }
    }

    void steering(Vector3 desiredVelocity)
    {
        if (desiredVelocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        if (desiredVelocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        //make velocity clamped to the max
        desiredVelocity = Vector3.ClampMagnitude(desiredVelocity, maxVelocity);

        //get difference between desired velocity and current velocity
        Vector3 steer = desiredVelocity - velocity;
        //clamp steer velocity to max force
        steer = Vector3.ClampMagnitude(steer, maxForce);
        //add mass to steer velocity
        steer = steer / mass;

        //add steering force to the velocity
        velocity = Vector3.ClampMagnitude(velocity + steer, maxSpeed);

        Debug.DrawRay(transform.position, velocity, Color.red);
        Debug.DrawRay(transform.position, desiredVelocity, Color.blue);
    }

    void approach()
    {
        //desired velocity becomes towads the target, but is slowed by distance
        Vector3 desiredVelocity = targetDifference.normalized * maxVelocity * (targetDistance / approachRadius);
        //steer towards wanted velocity
        steering(desiredVelocity);
    }

    void seek()
    {
        Vector3 tmpVelocity = velocity;
        /*check if target is outside the approach radius
         * if so, set the desired velocity to be towards the target at max velocity
         * and steer towards the target. otherwise, approach them*/
        if (targetDistance > approachRadius)
        {
            Vector3 desiredVelocity = targetDifference.normalized * maxVelocity;
            steering(desiredVelocity);
        }
        else
        {
            approach();
        }

    }
}
