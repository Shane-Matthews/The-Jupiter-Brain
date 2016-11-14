using UnityEngine;
using System.Collections;

public class BackgroundScroller : MonoBehaviour {

    public float backgroundsize;

    Transform cameraTransform;
    Transform[] layers;
    float viewZone = 35;
    int leftIndex;
    int rightIndex;

    float pSpeed;
    float lastCameraX;

    // Use this for initialization
    void Start () {
        cameraTransform = Camera.main.transform;
        lastCameraX = Camera.main.transform.position.x;
        layers = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            layers[i] = transform.GetChild(i);

        leftIndex = 0;
        rightIndex = layers.Length - 1;
        pSpeed = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
        float dX = cameraTransform.position.x - lastCameraX;
        transform.position += Vector3.right * dX * pSpeed;
        lastCameraX = Camera.main.transform.position.x;


        if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone))
            scrollLeft();

        if (cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone))
            scrollRight();

    }

    void scrollLeft()
    {
        int lastRight = rightIndex;
        layers[rightIndex].position = new Vector3(Vector3.right.x * (layers[leftIndex].position.x - backgroundsize), -35.6f, 0.1f);
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
            rightIndex = layers.Length - 1;
    }

    void scrollRight()
    {
        int lastLeft = leftIndex;
        layers[leftIndex].position = new Vector3(Vector3.right.x * (layers[rightIndex].position.x + backgroundsize), -35.6f, 0.1f);
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == layers.Length)
            leftIndex = 0;
    }
}
