using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, -2.5f, 0);
    public Transform follower;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        transform.position = new Vector3(follower.position.x, 0, 0) + offset;
    }
}
