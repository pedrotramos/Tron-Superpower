using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlayer : MonoBehaviour
{
    GameManager gm;
    float movementSpeed;
    public GameObject lightWallPrefab,ObjetoColetavel;
    Collider2D currentWall;
    Vector2 lastWallEndPoint;
    List<GameObject> instantiatedWalls = new List<GameObject>();
    List<GameObject> instantiatedCollectibles = new List<GameObject>();
    float timer;
    float timetospawn;
    float timeToScore;
    float timeToMove;
    float timerMove;
    
    public AudioClip shootSFX;

    private GameObject[] ListadeObstaculos;
    private bool isSurvival = false;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.GetInstance();
        movementSpeed = gm.speed;
        timer = 0;
        timerMove = 0f;
        timeToScore = 1f;
        timetospawn = 0f;
        timeToMove = gm.difficulty == 1 ? 0.15f : 0.3f / gm.difficulty;
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
        if (gm.gameState == GameManager.GameState.SURVIVAL){
            isSurvival = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.gameState != GameManager.GameState.SINGLE && gm.gameState != GameManager.GameState.SURVIVAL)
        {
            foreach (GameObject w in instantiatedWalls)
            {
                Destroy(w);
            }
            Destroy(gameObject);
            AudioManager.PlaySFX(shootSFX);
        }
        timer += Time.deltaTime;
        timerMove += Time.deltaTime;
        if (timer > timeToScore)
        {
            if (gm.gameState == GameManager.GameState.SINGLE)
            {
                gm.score += (gm.difficulty * 10) + (gm.destroyedNPCs * 10);
            }
            else if (gm.gameState == GameManager.GameState.SURVIVAL)
            {
                gm.score += 10 * gm.difficulty;
            }
            timer = 0;
        }
        // Change movement direction
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        if (inputX != 0 && GetComponent<Rigidbody2D>().velocity.x == 0 && timerMove > timeToMove)
        {
            timerMove = 0f;
            GetComponent<Rigidbody2D>().velocity = inputX * Vector2.right * movementSpeed;
            transform.rotation = Quaternion.Euler(0f, 0f, inputX * -90f);
            SpawnWall();
        }
        else if (inputY != 0 && GetComponent<Rigidbody2D>().velocity.y == 0 && timerMove > timeToMove)
        {
            timerMove = 0f;
            GetComponent<Rigidbody2D>().velocity = inputY * Vector2.up * movementSpeed;
            if (inputY > 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            }
            SpawnWall();
        }
        // Resize the collider between the current wall and last wall's end point
        FitWallCollider(currentWall, lastWallEndPoint, transform.position);

        //Spawn coletavel
        if (isSurvival){
            timetospawn += Time.deltaTime;
            if ( timetospawn > 2){
                SpawnColetavel();
                timetospawn = 0;
            }
        }
    }

    // Spawn um coletavel
    void SpawnColetavel(){
        float xrand = Random.Range(-100f, 100f);
        float yrand = Random.Range(-50f, 50f);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector3(xrand,yrand,0f), 3);
        while (colliders.Length > 0){
            xrand = Random.Range(-100f, 100f);
            yrand = Random.Range(-50f, 50f);
            colliders = Physics2D.OverlapCircleAll(new Vector3(xrand,yrand,0f), 3);
        }

        GameObject objcoletavel = Instantiate(ObjetoColetavel, new Vector3(xrand,yrand,0f),Quaternion.identity);
        instantiatedCollectibles.Add(objcoletavel);

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
        if (collider.gameObject.tag == "Coletavel"){
                gm.score += 20;
                Destroy(collider.gameObject);
            }
        else if (collider != currentWall)
        {
            foreach (GameObject w in instantiatedWalls)
            {
                Destroy(w);
            }
            foreach (GameObject c in instantiatedCollectibles)
            {
                Destroy(c);
            }
            Destroy(gameObject);
            AudioManager.PlaySFX(shootSFX);
            if (gm.gameState == GameManager.GameState.SURVIVAL)
            {
                gm.ChangeState(GameManager.GameState.END_SURVIVAL);
                ListadeObstaculos = GameObject.FindGameObjectsWithTag("Obstaculo");
                for (int i = 0; i < ListadeObstaculos.Length; i++)
                {
                    Destroy(ListadeObstaculos[i]);
                }
            }
            else if (gm.gameState == GameManager.GameState.SINGLE)
            {
                ListadeObstaculos = GameObject.FindGameObjectsWithTag("Obstaculo");
                for (int i = 0; i < ListadeObstaculos.Length; i++)
                {
                    Destroy(ListadeObstaculos[i]);
                }
                gm.ChangeState(GameManager.GameState.END_SINGLE);
            }
        }
    }
}