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
    public Vector3 posicaojogador;
    public AudioClip shootSFX; 

    private float timetochange;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GetInstance();
        movementSpeed = gm.speed;
        timer = 0;
        timetochange = Random.Range(1.5f, 4f);
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
            foreach (GameObject w in instantiatedWalls)
            {
                Destroy(w);
            }
            Destroy(gameObject);
        }
        posicaojogador = GameObject.FindWithTag("Player").transform.position;
        timer += Time.deltaTime;
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up), 3f);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), 5f);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.left), 5f);        
        if (hitUp) AvoidCollision(hitRight, hitLeft);
        else if (timer >= timetochange){
            MudarDeDirecao(hitRight, hitLeft);
            timer = 0f;
            timetochange = Random.Range(1.5f,4f);
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


    // Avoid Direct Collision
    void AvoidCollision(RaycastHit2D hitRight, RaycastHit2D hitLeft)
    {
        float velocity_X = GetComponent<Rigidbody2D>().velocity.x;
        float velocity_Y = GetComponent<Rigidbody2D>().velocity.y;
        if (velocity_X != 0)
        {
            if ((hitLeft && velocity_X > 0) || (hitRight && velocity_X < 0))
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.down * movementSpeed;
                transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            }
            else if ((hitRight && velocity_X > 0) || (hitLeft && velocity_X < 0))
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.up * movementSpeed;
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            SpawnWall();
        }
        else if (velocity_Y != 0)
        {
            if ((hitLeft && velocity_Y > 0) || (hitRight && velocity_Y < 0))
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.right * movementSpeed;
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            }
            else if ((hitRight && velocity_Y > 0) || (hitLeft && velocity_Y < 0))
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.left * movementSpeed;
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            }
            SpawnWall();
        }
    }

    void MudarDeDirecao(RaycastHit2D hitRight, RaycastHit2D hitLeft){
        float velocity_X = GetComponent<Rigidbody2D>().velocity.x;
        float velocity_Y = GetComponent<Rigidbody2D>().velocity.y;
        if (velocity_X != 0)
        {
            if ((hitLeft && velocity_X > 0) || (hitRight && velocity_X < 0)) // --> e pega em cima
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.down * movementSpeed;
                transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                Debug.Log("Desceu");
            }
            else if ((hitRight && velocity_X > 0) || (hitLeft && velocity_X < 0))
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.up * movementSpeed;
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                Debug.Log("Subiu");
            } else {
                //movimento aleatorio de virada
                int vira = Random.Range(0,10);
                if (vira < 5){
                    GetComponent<Rigidbody2D>().velocity = Vector2.down * movementSpeed;
                    transform.rotation = Quaternion.Euler(0f, 0f, 180);
                    Debug.Log("Desceu aleatorio");
                } else {
                    GetComponent<Rigidbody2D>().velocity = Vector2.up * movementSpeed;
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    Debug.Log("Subiu aleatorio");
                }

            }
            SpawnWall();
        }
        else if (velocity_Y != 0)
        {
            if ((hitLeft && velocity_Y > 0) || (hitRight && velocity_Y < 0)) 
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.right * movementSpeed;
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                Debug.Log("Direta");
            }
            else if ((hitRight && velocity_Y > 0) || (hitLeft && velocity_Y < 0))
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.left * movementSpeed;
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                Debug.Log("Esquerda");
            }
            else{
                //movimento aleatorio de virada
                int vira = Random.Range(0,10);
                if (vira < 5){
                    GetComponent<Rigidbody2D>().velocity = Vector2.right * movementSpeed;
                    transform.rotation = Quaternion.Euler(0f, 0f, -90);
                    Debug.Log("Direita aleatorio");
                } else {
                    GetComponent<Rigidbody2D>().velocity = Vector2.left * movementSpeed;
                    transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                    Debug.Log("Esquerda aleatorio");
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
            foreach (GameObject w in instantiatedWalls)
            {
                Destroy(w);
            }
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
