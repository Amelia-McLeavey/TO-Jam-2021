using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallonSpawnScript : MonoBehaviour
{
    public GameObject balloonToSpawn;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            GameObject newBalloon = Instantiate(balloonToSpawn, collision.transform.position, Quaternion.identity);
            newBalloon.name = newBalloon.name.Substring(0, 9);
            BallonMovement bm = newBalloon.GetComponent<BallonMovement>();
            bm.holder = GameObject.Find("Holder").transform;
            bm.holderRb = collision.GetComponent<Rigidbody2D>();

            Destroy(this.gameObject);
        }
    }
}
