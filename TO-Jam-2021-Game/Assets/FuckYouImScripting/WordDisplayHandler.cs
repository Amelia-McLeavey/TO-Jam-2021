using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordDisplayHandler : MonoBehaviour
{
    private FMOD.Studio.EventInstance emote;

    [SerializeField]
    int emotion;

    private void Start()
    {
        StartCoroutine(delayTime());
        PlayEmotion();
    }
    private IEnumerator delayTime()
    {
        yield return new WaitForSeconds(4f);
        this.gameObject.SetActive(false);
    }

    public void PlayEmotion()
    {
        emote = FMODUnity.RuntimeManager.CreateInstance("event:/EmotionPickUp");
        emote.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        emote.setParameterByName("Emotion", emotion);
        emote.start();
        emote.release();
    }
}
