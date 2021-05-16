using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonAudio : MonoBehaviour
{
    private FMOD.Studio.EventInstance pop;
    private FMOD.Studio.EventInstance emote;
    private FMOD.Studio.EventInstance inflate;
    private FMOD.Studio.EventInstance UI;
    public void PlayPop()
    {
        pop = FMODUnity.RuntimeManager.CreateInstance("event:/BalloonPop");
        pop.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        pop.start();
        pop.release();
    }

    public void PlayEmote()
    {
        emote = FMODUnity.RuntimeManager.CreateInstance("event:/Emotion");
        emote.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        emote.start();
        emote.release();
    }
    public void PlayInflate()
    {
        inflate = FMODUnity.RuntimeManager.CreateInstance("event:/BalloonInflate");
        inflate.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        inflate.start();
        inflate.release();
    }

    public void PlayUI()
    {
        UI = FMODUnity.RuntimeManager.CreateInstance("event:/UIButton");
        UI.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        UI.start();
        UI.release();
    }

}
