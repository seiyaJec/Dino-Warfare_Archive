using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRightMove : MonoBehaviour
{
    private Vector3 moveDir;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        moveDir = new Vector3(1.0f, 0.0f, 0.0f);
        speed = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += speed * Time.deltaTime * moveDir;
    }
}
