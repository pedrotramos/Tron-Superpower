using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBehavior : MonoBehaviour
{

    public GameObject ObjetoColetavel;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.tag == "Obtaculo"){
                Destroy(gameObject);
                SpawnColetavel();

            }
        else if (collider.gameObject.tag == "Wall"){
                Destroy(gameObject);
                SpawnColetavel();
            }
            
    }

    void SpawnColetavel(){
        float xrand = Random.Range(-100f, 100f);
        float yrand = Random.Range(-50f, 50f);
        Instantiate(ObjetoColetavel, new Vector3(xrand,yrand,0f),Quaternion.identity);
    } 
}
