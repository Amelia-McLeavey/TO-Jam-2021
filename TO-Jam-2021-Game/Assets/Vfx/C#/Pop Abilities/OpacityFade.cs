using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpacityFade : MonoBehaviour
{
    [SerializeField] Material mat;
    [Range(0,1)][SerializeField] float alpha = 1;

    private void Update() {
        Color color = mat.GetColor("_Color");
        color.a = alpha;
        mat.SetColor("_Color", color);
    }
}
