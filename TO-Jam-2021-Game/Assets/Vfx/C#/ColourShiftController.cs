using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourShiftController : MonoBehaviour 
{
    [SerializeField] float maxAlpha = 1;
    [Range(0, 1)] public float alphaPercent = 0;

    Material mat;
    Color color = new Color();

    private void Start() {
        mat = GetComponent<MeshRenderer>().material;
        color = mat.GetColor("_Color");
    }

    void Update()
    {
        color.a = maxAlpha * alphaPercent;
        mat.SetColor("_Color", color);
    }
}
