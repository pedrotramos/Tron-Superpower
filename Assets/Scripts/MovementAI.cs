using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAI : MonoBehaviour
{
    public GameManager gm;
    float movementSpeed;
    public GameObject lightWallPrefab;
    Collider2D currentWall;
    Vector2 lastWallEndPoint;
    List<GameObject> instantiatedWalls = new List<GameObject>();
    float timer;
    float timeToChangeDirection;
    float timeToMove;
    float timerMove;
    public Vector3 posicaojogador;
    public AudioClip shootSFX;
    public Transform rcOriginL, rcOriginM, rcOriginR;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GetInstance();
        movementSpeed = gm.speed;
        timer = 0;
        timeToChangeDirection = Random.Range(1f, 4f);
        timerMove = 0f;
        timeToMove = gm.difficulty == 1 ? 0.15f : 0.225f / gm.difficulty;
        // Randomly decide starting direction
        float movementDirection = Random.Range(0, 4);
        if (movementDirection == 0)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.right * movementSpeed;
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }
        else if (movementDirection == 1)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.down * movementSpeed;
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        else if (movementDirection == 2)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.left * movementSpeed;
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.up * movementSpeed;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        SpawnWall();
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.gameState != GameManager.GameState.SINGLE)
        {
            Destroy(gameObject);
        }
        posicaojogador = GameObject.FindWithTag("Player").transform.position;
        timer += Time.deltaTime;
        timerMove += Time.deltaTime;

        RaycastHit2D hitUp = Physics2D.Raycast(rcOriginM.position, transform.TransformDirection(Vector2.up), 2f);
        RaycastHit2D hitUpR = Physics2D.Raycast(rcOriginR.position, transform.TransformDirection(Vector2.up), 2f);
        RaycastHit2D hitUpL = Physics2D.Raycast(rcOriginL.position, transform.TransformDirection(Vector2.up), 2f);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), Mathf.Infinity);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.left), Mathf.Infinity);
        if ((hitUp || hitUpR || hitUpL) && timerMove > timeToMove)
        {
            timerMove = 0f;
            AvoidCollision(hitRight, hitLeft);
        }
        else if (timer >= timeToChangeDirection && timerMove > timeToMove)
        {
            ChangeDirection(hitRight, hitLeft);
            timer = 0f;
            timerMove = 0f;
            timeToChangeDirection = Random.Range(1f, 4f);
        }
        else
        {
            if (gm.difficulty == 2) // Normal difficulty
            {
                // Increase difficulty


            }
            else if (gm.difficulty == 3) // Hard difficulty
            {
                // Increase difficulty even more
            }
        }
        // Resize the collider between the current wall and last wall's end point
        FitWallCollider(currentWall, lastWallEndPoint, transform.position);
    }

    void ChangeDirection(RaycastHit2D hitRight, RaycastHit2D hitLeft)
    {
        float velocity_X = GetComponent<Rigidbody2D>().velocity.x;
        float velocity_Y = GetComponent<Rigidbody2D>().velocity.y;
        float distConst = 3.5f;
        if (hitRight && hitLeft) return;
        else if (velocity_X != 0)
        {
            if ((hitLeft.distance <= distConst && velocity_X > 0) || (hitRight.distance <= distConst && velocity_X < 0)) // Obstacle on top
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.down * movementSpeed;
                transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            }
            else if ((hitRight.distance <= distConst && velocity_X > 0) || (hitLeft.distance <= distConst && velocity_X < 0)) // Obstacle below
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.up * movementSpeed;
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                // Random turn
                if ((hitLeft.distance <= hitRight.distance && velocity_X > 0) || (hitRight.distance <= hitLeft.distance && velocity_X < 0))
                {
                    GetComponent<Rigidbody2D>().velocity = Vector2.down * movementSpeed;
                    transform.rotation = Quaternion.Euler(0f, 0f, 180);
                }
                else if ((hitRight.distance < hitLeft.distance && velocity_X > 0) || (hitLeft.distance < hitRight.distance && velocity_X < 0))
                {
                    GetComponent<Rigidbody2D>().velocity = Vector2.up * movementSpeed;
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }

            }
            SpawnWall();
        }
        else if (velocity_Y != 0)
        {
            if ((hitLeft.distance <= distConst && velocity_Y > 0) || (hitRight.distance <= distConst && velocity_Y < 0))
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.right * movementSpeed;
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            }
            else if ((hitRight.distance <= distConst && velocity_Y > 0) || (hitLeft.distance <= distConst && velocity_Y < 0))
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.left * movementSpeed;
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            }
            else
            {
                //movimento aleatorio de virada
                if ((hitLeft.distance <= hitRight.distance && velocity_Y > 0) || (hitRight.distance <= hitLeft.distance && velocity_Y < 0))
                {
                    GetComponent<Rigidbody2D>().velocity = Vector2.right * movementSpeed;
                    transform.rotation = Quaternion.Euler(0f, 0f, -90);
                }
                else if ((hitRight.distance < hitLeft.distance && velocity_Y > 0) || (hitLeft.distance < hitRight.distance && velocity_Y < 0))
                {
                    GetComponent<Rigidbody2D>().velocity = Vector2.left * movementSpeed;
                    transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                }

            }
            SpawnWall();
        }
    }

    // Avoid Direct Collision
    void AvoidCollision(RaycastHit2D hitRight, RaycastHit2D hitLeft)
    {
        float velocity_X = GetComponent<Rigidbody2D>().velocity.x;
        float velocity_Y = GetComponent<Rigidbody2D>().velocity.y;
        float distConst = 3.5f;
        if (velocity_X != 0)
        {
            if ((hitLeft.distance <= distConst && velocity_X > 0) || (hitRight.distance <= distConst && velocity_X < 0))
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.down * movementSpeed;
                transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            }
            else if ((hitRight.distance <= distConst && velocity_X > 0) || (hitLeft.distance <= distConst && velocity_X < 0))
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.up * movementSpeed;
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                if ((hitLeft.distance <= hitRight.distance && velocity_X > 0) || (hitRight.distance <= hitLeft.distance && velocity_X < 0))
                {
                    GetComponent<Rigidbody2D>().velocity = Vector2.down * movementSpeed;
                    transform.rotation = Quaternion.Euler(0f, 0f, 180);
                }
                else if ((hitRight.distance < hitLeft.distance && velocity_X > 0) || (hitLeft.distance < hitRight.distance && velocity_X < 0))
                {
                    GetComponent<Rigidbody2D>().velocity = Vector2.up * movementSpeed;
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
            }
            SpawnWall();
        }
        else if (velocity_Y != 0)
        {
            if ((hitLeft.distance <= distConst && velocity_Y > 0) || (hitRight.distance <= distConst && velocity_Y < 0))
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.right * movementSpeed;
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            }
            else if ((hitRight.distance <= distConst && velocity_Y > 0) || (hitLeft.distance <= distConst && velocity_Y < 0))
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.left * movementSpeed;
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            }
            else
            {
                if ((hitLeft.distance <= hitRight.distance && velocity_Y > 0) || (hitRight.distance <= hitLeft.distance && velocity_Y < 0))
                {
                    GetComponent<Rigidbody2D>().velocity = Vector2.right * movementSpeed;
                    transform.rotation = Quaternion.Euler(0f, 0f, -90);
                }
                else if ((hitRight.distance < hitLeft.distance && velocity_Y > 0) || (hitLeft.distance < hitRight.distance && velocity_Y < 0))
                {
                    GetComponent<Rigidbody2D>().velocity = Vector2.left * movementSpeed;
                    transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                }
            }
            SpawnWall();
        }
    }

    // Spawn a new Lightwall
    void SpawnWall()
    {
        // Define the point where the last wall ends
        lastWallEndPoint = transform.position;

        // Instantiate the new wall
        GameObject newWall = Instantiate(lightWallPrefab, transform.position, Quaternion.identity);
        instantiatedWalls.Add(newWall);
        currentWall = newWall.GetComponent<Collider2D>();
    }

    void TryToCatch(RaycastHit2D hitRight, RaycastHit2D hitLeft)
    {
        posicaojogador = GameObject.FindWithTag("Goal").transform.position;

        float difx, dify = 0;
        float velocity_X = GetComponent<Rigidbody2D>().velocity.x;
        float velocity_Y = GetComponent<Rigidbody2D>().velocity.y;
        difx = transform.position.x - posicaojogador.x;
        dify = transform.position.y - posicaojogador.y;
        float distConst = 3.5f;
        if (velocity_Y != 0)
        {
            if (difx > 0)
            {
                if (!(hitLeft.distance <= distConst && velocity_Y > 0) && !(hitRight.distance <= distConst && velocity_Y < 0))
                {
                    //virar para esquerda
                    GetComponent<Rigidbody2D>().velocity = Vector2.left * movementSpeed;
                    transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                }

            }
            else if (difx < 0)
            {
                if (!(hitRight.distance <= distConst && velocity_Y > 0) && !(hitLeft.distance <= distConst && velocity_Y < 0))
                {
                    //virar para direita
                    GetComponent<Rigidbody2D>().velocity = Vector2.right * movementSpeed;
                    transform.rotation = Quaternion.Euler(0f, 0f, -90);
                }

            }
            SpawnWall();
        }
        else if (velocity_X != 0)
        {
            if (dify < 0)
            {
                //virar para cima
                if (!(hitLeft.distance <= distConst && velocity_X > 0) && !(hitRight.distance <= distConst && velocity_X < 0))
                {
                    GetComponent<Rigidbody2D>().velocity = Vector2.up * movementSpeed;
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
            }
            else if (dify > 0)
            {
                if (!(hitRight.distance <= distConst && velocity_X > 0) && !(hitLeft.distance <= distConst && velocity_X < 0))
                {
                    //virar para baixo
                    GetComponent<Rigidbody2D>().velocity = Vector2.down * movementSpeed;
                    transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                }
            }
            SpawnWall();
        }
    }

    // Fit a collider between the current wall and last wall's end point
    void FitWallCollider(Collider2D collider, Vector2 vec1, Vector2 vec2)
    {
        // Calculate the midpoint
        Vector2 midpoint = vec1 + ((vec2 - vec1) / 2f);
        collider.transform.position = midpoint;

        // Calculate distance between the two points
        float dist = Vector2.Distance(vec1, vec2);

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
            AudioManager.PlaySFX(shootSFX);

            if (gm.gameState == GameManager.GameState.SINGLE)
            {
                gm.destroyedNPCs++;
            }
            if (gm.destroyedNPCs == 3)
            {
                gm.singleWin = true;
                gm.ChangeState(GameManager.GameState.END_SINGLE);
            }
        }
    }
}
