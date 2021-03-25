using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float movementSpeed = 25f;
    public GameObject lightWallPrefab;
    Collider2D currentWall;
    Vector2 lastWallEndPoint;
    public KeyCode upKey, downKey, rightKey, leftKey;



    // Start is called before the first frame update
    void Start()
    {
        float movementDirection = Random.Range(0, 4);
        if (movementDirection == 0)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.right * movementSpeed;
        }
        else if (movementDirection == 1)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.down * movementSpeed;
        }
        else if (movementDirection == 2)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.left * movementSpeed;
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.up * movementSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = 0;
        float inputY = 0;

        if (Input.GetKeyDown(rightKey)) inputX = 1;
        else if (Input.GetKeyDown(leftKey)) inputX = -1;

        if (Input.GetKeyDown(upKey)) inputY = 1;
        else if (Input.GetKeyDown(downKey)) inputY = -1;

        if (inputX != 0)
        {
            GetComponent<Rigidbody2D>().velocity = inputX * Vector2.right * movementSpeed;
        }
        else if (inputY != 0)
        {
            GetComponent<Rigidbody2D>().velocity = inputY * Vector2.up * movementSpeed;
        }
        SpawnWall();
        FitWallCollider(currentWall, lastWallEndPoint, transform.position);
    }

    // Spawn a new Lightwall
    void SpawnWall()
    {
        // Define the point where the last wall ends
        lastWallEndPoint = transform.position;

        // Instantiate the new wall
        GameObject newWall = Instantiate(lightWallPrefab, transform.position, Quaternion.identity);
        currentWall = newWall.GetComponent<Collider2D>();
    }

    void FitWallCollider(Collider2D collider, Vector2 vec1, Vector2 vec2)
    {
        // Calculate the midpoint
        collider.transform.position = vec1 + (vec2 - vec1) * 0.5f;

        // Calculate distance between the two points
        float dist = Vector2.Distance(vec1, vec2);

        if (vec1.x != vec2.x)
        {
            // To scale horizontally
            collider.transform.localScale = new Vector2(dist + 1, 1);
        }
        else
        {
            // To scale vertically
            collider.transform.localScale = new Vector2(1, dist + 1);
        }
    }

    // void OnTriggerEnter2D(Collider2D collider)
    // {
    //     // Check if the collision was not between the player and the current wall
    //     if (collider != currentWall)
    //     {
    //         Destroy(gameObject);
    //     }
    // }
}
