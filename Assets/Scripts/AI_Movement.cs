using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Movement : MonoBehaviour
{
    public GameManager gm;
    public float movementSpeed = 25f;
    public GameObject lightWallPrefab;
    Collider2D currentWall;
    Vector2 lastWallEndPoint;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GetInstance();
        // Randomly decide starting direction
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
        SpawnWall();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Implementar a Inteligência Artificial
        // Resize the collider between the current wall and last wall's end point
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

    // Fit a collider between the current wall and last wall's end point
    void FitWallCollider(Collider2D collider, Vector2 vec1, Vector2 vec2)
    {
        // Calculate the midpoint
        Vector2 midpoint = vec1 + ((vec2 - vec1) / 2f);
        collider.transform.position = midpoint;

        // Calculate distance between the two points
        float dist = Vector2.Distance(vec1, vec2);

        print(midpoint + " | " + dist);


        if (vec1.x != vec2.x)
        {
            // Scale horizontally
            collider.transform.localScale = new Vector2((dist / 2) + 1, 1);
        }
        else
        {
            // Scale vertically
            collider.transform.localScale = new Vector2(1, (dist / 2) + 1);
        }
    }

    //Define what happens in case of a collision
    void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the collision was not between the player and the current wall
        if (collider != currentWall)
        {
            Destroy(gameObject);
        }
    }
}
