using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Transform cam;
    public float speed;
    private Vector3 lastCamPos;
    private float textureUnitSize;
    void Start()
    {
        cam = Camera.main.transform;

        lastCamPos = cam.position;

        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSize = texture.width / sprite.pixelsPerUnit;
    }

    void LateUpdate()
    {
        Vector3 movement = cam.position - lastCamPos;
        transform.position += new Vector3(movement.x*speed, movement.y, 0f) ;
        lastCamPos = cam.position;

        if(Mathf.Abs(cam.position.x - transform.position.x) >= textureUnitSize)
        {
            float offsetPosition = (cam.position.x - transform.position.x) % textureUnitSize;
            transform.position = new Vector3(cam.position.x+ offsetPosition, transform.position.y, transform.position.z);
        }
    }
}
