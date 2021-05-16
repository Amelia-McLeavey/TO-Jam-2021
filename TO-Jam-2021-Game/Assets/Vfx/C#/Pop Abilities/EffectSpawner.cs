using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    [SerializeField] float maxRadius;
    [SerializeField] GameObject effectPrefab;
    [SerializeField] Emotion emotion;
    CircleCollider2D col;
    List<Collider2D> hitColliders = new List<Collider2D>();

    private void Start() {
        col = GetComponent<CircleCollider2D>();
    }

    void LateUpdate() {
        if (transform.parent.localScale.x > maxRadius) {
            Destroy(gameObject);
        }
        
        Collider2D[] hitCols = Physics2D.OverlapCircleAll((Vector2)transform.position + col.offset, col.radius * transform.lossyScale.x, ~(1<<0));
        foreach (Collider2D hitCol in hitCols) {
            if (!hitColliders.Contains(hitCol) && hitCol.gameObject.layer == gameObject.layer) {
                hitColliders.Add(hitCol);

                Vector3 point = hitCol.ClosestPoint(transform.position);
                GameObject effect = Instantiate(effectPrefab, point, Quaternion.identity, hitCol.transform);
                effect.GetComponent<CollisionEffect>().emotion = emotion;
                effect.transform.up = (transform.position - point).normalized;
            }
        }
    }
}
