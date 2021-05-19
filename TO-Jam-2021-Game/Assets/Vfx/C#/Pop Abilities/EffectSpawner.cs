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

    private void Awake() {
        col = GetComponent<CircleCollider2D>();
        maxRadius = maxRadius == 0 ? transform.parent.parent.GetComponent<ParticleClipSetup>().maxRadius : maxRadius;
    }

    void LateUpdate() {
        if (transform.parent.localScale.x > maxRadius) {
            Destroy(gameObject);
        }
        
        Collider2D[] hitCols = Physics2D.OverlapCircleAll((Vector2)transform.position + col.offset, col.radius * transform.lossyScale.x, ~(1<<0));
        foreach (Collider2D hitCol in hitCols) {

            //check if platform is interactable with this ability
            DynamicPlatformController platform = hitCol.GetComponent<DynamicPlatformController>();

            if (platform == null) {
                continue;
            }

            bool isInteractable = false;
            isInteractable = emotion == Emotion.Anger && (platform._type == "Shove" || platform._type == "Universal") ? true : isInteractable;
            isInteractable = emotion == Emotion.Joy && (platform._type == "Float" || platform._type == "Universal") ? true : isInteractable;
            isInteractable = emotion == Emotion.Sadness && platform._affectedBySlow ? true : isInteractable;

            //apply effect
            if (!hitColliders.Contains(hitCol) && isInteractable) {
                hitColliders.Add(hitCol);

                Vector3 point = hitCol.ClosestPoint(transform.position);
                GameObject effect = Instantiate(effectPrefab, point, Quaternion.identity, hitCol.transform);
                effect.GetComponent<CollisionEffect>().emotion = emotion;
                effect.transform.up = (transform.position - point).normalized;
            }
        }

        if (transform.parent.localScale.x == maxRadius) {
            Destroy(gameObject);
        }
    }
}
