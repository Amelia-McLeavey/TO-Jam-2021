using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableScript : MonoBehaviour
{
    private float startYpos;
    public float bounceHeight, speed;
    [Header("yes if white, no if black")]
    public bool whiteCollectable;
    private Counter counter;
    // Start is called before the first frame update
    void Start()
    {
        startYpos = this.transform.position.y;
        counter = this.transform.GetComponentInParent<Counter>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x, startYpos + Mathf.Sin(speed*Time.time)*bounceHeight);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            counter.addCount(whiteCollectable);
            Destroy(this.gameObject);
        }
    }
}
