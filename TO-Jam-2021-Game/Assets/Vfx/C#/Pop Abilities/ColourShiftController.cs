using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourShiftController : MonoBehaviour 
{
    [Range(0, 1)] public float alphaPercent = 0;

    Material mat;
    Color color = new Color();

    private void Start() {
        mat = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        mat.SetFloat("_Alpha", alphaPercent);
    }
}
