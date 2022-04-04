using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;

    public float zPos = -10f;
    public float xPos = 2f;
    public float yPos = 2f;

    public float lerpSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = target.position;
        targetPos += new Vector3(xPos, yPos, zPos);

        Vector3 pos = transform.position;

        pos = Vector3.Lerp(pos, targetPos, lerpSpeed);

        transform.position = pos;
    }


}
