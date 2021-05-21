using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParticleClipSetup : MonoBehaviour
{
    public float maxRadius = 5f;
    [SerializeField][Range(0,1)] float radiusPercent = 0;
    private float radius { get { return maxRadius * radiusPercent; } }
    
    [SerializeField] Material[] mats;
    [SerializeField] Transform[] radiiObjects;
    [SerializeField] Transform[] maxRadiiObjects;

    private void Awake() {
        foreach (Transform obj in maxRadiiObjects) {
            ParticleSystem ps = obj.GetComponent<ParticleSystem>();
            if (ps) {
                var shape = ps.shape;
                shape.radius = maxRadius + 0.5f;
            }
            else {
                obj.localScale = new Vector3(maxRadius, maxRadius, maxRadius);
            }
        }
    }

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

    public void ResetMaterials() {
        for (int i = 0; i < mats.Length; i++) {
            mats[i].SetVector("_RenderSphere", new Vector4(0, 0, -1000, 0));
        }
    }
}
