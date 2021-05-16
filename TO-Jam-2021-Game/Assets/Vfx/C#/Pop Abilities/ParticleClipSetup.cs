using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParticleClipSetup : MonoBehaviour
{
    [SerializeField] float radius = 5f;
    
    [SerializeField] Material[] mats;
    [SerializeField] Transform[] radiiObjects;

    
    void Update()
    {
        for (int i=0; i<radiiObjects.Length; i++) {
            ParticleSystem ps = radiiObjects[i].GetComponent<ParticleSystem>();
            if (ps) {
                var shape = ps.shape;
                shape.radius = radius + 1;
            }
            else {
                radiiObjects[i].localScale = new Vector3(radius, radius, radius);
            }
        }

        for (int i = 0; i < mats.Length; i++) {
            mats[i].SetVector("_RenderSphere", new Vector4(transform.position.x, transform.position.y, transform.position.z, radius));
        }
    }
}
