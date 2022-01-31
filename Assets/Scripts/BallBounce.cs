using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBounce : MonoBehaviour
{
    public Rigidbody2D rb;
    private Vector3 lastVelocity;
    private float baseBounce = 0.75f;

    private Dictionary<string, float> bounceValues = new Dictionary<string, float>();
    private Dictionary<string, float> additiveValues = new Dictionary<string, float>();

    

    // Start is called before the first frame update
    void Start()
    {
        bounceValues["Floor"] = baseBounce;
        bounceValues["Trampoline"] = 1.5f;
        bounceValues["Spike"] = 0f;
        bounceValues["TNT"] = 2f;
        additiveValues["TNT"] = 15f;
        if(false){
            bounceValues["BS"] = 2f;
        }

    }

    // Update is called once per frame
    void Update()
    {
        lastVelocity = rb.velocity;
        var speed = lastVelocity.magnitude;

        speed *= 0.75f;
        if (speed < 1)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("HIT " + collision.gameObject.tag);
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

        var bounceValue = baseBounce;
        if(bounceValues.ContainsKey(collision.gameObject.tag))
        {
            bounceValue = bounceValues[collision.gameObject.tag];
        }

        var addValue = 0f;
        if (additiveValues.ContainsKey(collision.gameObject.tag))
        {
            addValue = additiveValues[collision.gameObject.tag];
        }

        speed *= bounceValue;
        speed += addValue;

        rb.velocity = direction * Mathf.Max(speed, 0f);

    }
    
}
