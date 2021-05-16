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
        if (transform.parent.localScale.x * 2 > maxRadius) {
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

    //private void OnTCOllisionEnter2D(Collider2D collision) {
    //    if (collision.gameObject.layer == gameObject.layer) {
    //        if (Vector2.Distance(collision.cGetContact(0).point, transform.position) > maxRadius) {
    //            Destroy(gameObject);
    //            return;
    //        }

    //        GameObject effect = Instantiate(effectPrefab, collision.GetContact(0).point, Quaternion.Euler(collision.GetContact(0).normal), collision.transform);
    //        effect.GetComponent<CollisionEffect>().emotion = emotion;
    //    }
    //}
}
