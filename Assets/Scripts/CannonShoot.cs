using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CannonShoot : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    //private Vector3 point = new Vector3(0, 0, 0);
    public bool fired = false;
    public bool hasStopped = false;
    public int score = 0;
    private int highscore = 0;

    private int topOfScreen = 18;
    public Text offscreenDistance;
    public Text scoreboard;
    public GameObject ball;
    public Text angleText;
    public Slider powerBar;

    public float powerMultiplier = 1;
    private int powerDir = 1;
    private int maxPower = 2;

    private Vector3 startingPosition;

    private int maxCoveredArea = 0;
    public GameObject obstacleParent;
    public GameObject trampoline;
    public GameObject spikes;
    public GameObject tnt;

    private List<GameObject> obstacles = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        startingPosition = ball.transform.position;
        hasStopped = false;
        fired = false;
        score = 0;

        // fill list
        obstacles.Add(trampoline);
        obstacles.Add(spikes);
        obstacles.Add(tnt);

        SpawnObstacles(0);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePower();
        var angleChange = Input.GetAxis("Vertical");
        var angle = transform.rotation.eulerAngles.z;// * Mathf.Rad2Deg;

        //transform.RotateAround(point, Vector3.up, angle);
        if (!((angle >= 85 && angleChange > 0) || (angle <= 10 && angleChange < 0)))
        {
            transform.Rotate(0, 0, angleChange * 25 * Time.deltaTime);
            angleText.text = ((int)angle) + "°";
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (!fired)
                Shoot(angle);
            else if (hasStopped)
            {
                Reset();
            }
        }
        var ballY = ball.transform.position.y;
        if ( ballY > topOfScreen)
        {
            offscreenDistance.enabled = true;
            offscreenDistance.text = "^" + ((int)ballY) + "m^";
        } else
        {
            offscreenDistance.enabled = false;
        }

        var ballX = ball.transform.position.x;
        if(ballX > maxCoveredArea-50)
        {
            SpawnObstacles(maxCoveredArea);
        }


        if (fired && rb.velocity.magnitude <= 1)
        {
            HandleEnd();
        } 
    }

    void Shoot(float angle)
    {
        angleText.enabled = false;
        powerBar.gameObject.SetActive(false);
        fired = true;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.velocity = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad),0) * speed * powerMultiplier;
    }

    void Reset()
    {
        fired = false;
        scoreboard.enabled = false;
        powerBar.gameObject.SetActive(true);
        angleText.enabled = true;
        hasStopped = false;
        score = 0;
        rb.constraints = RigidbodyConstraints2D.None;
        ball.transform.position = startingPosition;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        ClearObstacles();
        maxCoveredArea = 0;
        SpawnObstacles(0);
    }

    void UpdatePower()
    {
        if (powerMultiplier >= maxPower)
        {
            powerDir = -1;
        } else if (powerMultiplier <= 0.5)
        {
            powerDir = 1;
        }
        powerMultiplier += powerDir * Time.deltaTime;
        powerBar.value = (powerMultiplier - 0.5f) / (maxPower - 0.5f);

    }

    void HandleEnd()
    {
        hasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        score = ((int)ball.transform.position.x);
        if(score > highscore)
        {
            highscore = score;
        }
        scoreboard.text = "Final Score: " + score + "m\n Highscore: " + highscore + "m";
        scoreboard.enabled = true;

    }

    void ClearObstacles()
    {
        foreach (Transform child in obstacleParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    void SpawnObstacles(int startSection)
    {

        foreach (Transform child in obstacleParent.transform)
        {
            if(child.gameObject.transform.position.x < startSection-100)
                GameObject.Destroy(child.gameObject);
        }
        for (int i=0; i<10; i++)
        {
            // pick random object
            var randomIndex = Random.Range(0, obstacles.Count);
            var randomOffset = Random.Range(-9f, 9f);
            var location = new Vector3(startSection + 10 + randomOffset + 20 * i, 0, 0);
            var obs = Instantiate(obstacles[randomIndex], location, Quaternion.identity);
            obs.transform.parent = obstacleParent.transform;
        }
        maxCoveredArea += 200;
    }

}
