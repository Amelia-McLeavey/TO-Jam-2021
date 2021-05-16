using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColourShifterLookAt : MonoBehaviour
{
    void Update()
    {
        //transform.up = -Camera.main.transform.forward;
        transform.up = Camera.main.transform.position - transform.position;
    }
}
