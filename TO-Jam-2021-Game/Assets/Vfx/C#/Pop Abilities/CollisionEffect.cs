using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Emotion { Joy, Sadness, Anger }

public class CollisionEffect : MonoBehaviour
{
    [SerializeField] Shader effectMaterial;
    [SerializeField] Texture noiseTexture;
    Shader savedMaterial;

    MeshRenderer meshRenderer;
    float startTime;
    const float alphaDuration = 0.4f;

    [HideInInspector] public Emotion emotion;

    [Header("Effect Variables")]
    [SerializeField] float joyDuration;
    [SerializeField] Color joyColor;
    [SerializeField] float angerDuration;
    [SerializeField] Color angerColor;
    [SerializeField] float sadDuration;
    [SerializeField] Color sadColor;

    float duration;
    float maxAlpha;

    void Awake()
    {
        meshRenderer = transform.parent.GetComponentInChildren<MeshRenderer>();
        savedMaterial = meshRenderer.material.shader;
        meshRenderer.material.shader = effectMaterial;

        startTime = Time.time;
        StartCoroutine(SetupEmotion());

        CollisionEffect[] effects = transform.parent.GetComponentsInChildren<CollisionEffect>();
        foreach (CollisionEffect effect in effects) {
            if (effect != this) {
                Destroy(effect.gameObject);
            }
        }
    }

    IEnumerator SetupEmotion() {
        yield return new WaitForEndOfFrame();

        Color tempColor;
        if (emotion == Emotion.Joy) {
            duration = joyDuration;
            tempColor = joyColor;
        }
        else if (emotion == Emotion.Sadness) {
            duration = sadDuration;
            tempColor = sadColor;
        }
        else {
            duration = angerDuration;
            tempColor = angerColor;
        }
        maxAlpha = tempColor.a;
        tempColor.a = 0;
        meshRenderer.material.SetColor("_EffectColor", tempColor);
        meshRenderer.material.SetTexture("_Noise", noiseTexture);

        tempColor.a = 1;
        //reposition particle systems cause scaling screws them up
        for (int i = 0; i < transform.childCount; i++) {
            //transform.GetChild(i).position = transform.position + transform.up * 2;
            ParticleSystem ps = transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps) {
                ps.startColor = tempColor;
            }
        }
    }
    
    void Update()
    {
        float timer = Time.time - startTime;

        if (timer < alphaDuration) {
            Color color = meshRenderer.material.GetColor("_EffectColor");
            color.a = Mathf.Lerp(0, maxAlpha, timer / alphaDuration);
            meshRenderer.material.SetColor("_EffectColor", color);
        }
        else if (timer < duration && timer > duration - alphaDuration) {
            Color color = meshRenderer.material.GetColor("_EffectColor");
            color.a = Mathf.Lerp(0, maxAlpha, (duration - timer) / alphaDuration);
            meshRenderer.material.SetColor("_EffectColor", color);
        }
        else if (timer >= duration) {
            meshRenderer.material.shader = savedMaterial;
            Destroy(gameObject);
        }
    }


}