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
        meshRenderer = transform.parent.GetComponent<MeshRenderer>();
        savedMaterial = meshRenderer.material.shader;
        meshRenderer.material.shader = effectMaterial;

        startTime = Time.time;
        StartCoroutine(SetupEmotion());

    }

    IEnumerator SetupEmotion() {
        yield return new WaitForEndOfFrame();

        if (emotion == Emotion.Joy) {
            duration = joyDuration;
            Color color = joyColor;
            color.a = 0;
            meshRenderer.material.SetColor("_EffectColor", color);
            maxAlpha = joyColor.a;
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (emotion == Emotion.Sadness) {
            duration = sadDuration;
            Color color = sadColor;
            color.a = 0;
            meshRenderer.material.SetColor("_EffectColor", color);
            maxAlpha = sadColor.a;
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (emotion == Emotion.Anger) {
            duration = angerDuration;
            Color color = angerColor;
            color.a = 0;
            meshRenderer.material.SetColor("_EffectColor", color);
            maxAlpha = angerColor.a;
            transform.GetChild(2).gameObject.SetActive(true);
        }
        meshRenderer.material.SetTexture("_Noise", noiseTexture);


        //reposition particle systems cause scaling screws them up
        for (int i = 0; i < 3; i++) {
            transform.GetChild(i).position = transform.position + transform.up * 2;
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