using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXAudio : MonoBehaviour
{
    private FMOD.Studio.EventInstance emote;

    [SerializeField]
    int emotion;

    public void PlayEmotion()
    {
        emote = FMODUnity.RuntimeManager.CreateInstance("event:/Emotion");
        emote.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        emote.setParameterByName("Emotion", emotion);
        emote.start();
        emote.release();
    }

    public void StopEmotion()
    {

        emote.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
