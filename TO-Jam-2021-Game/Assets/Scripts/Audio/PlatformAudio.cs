using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAudio : MonoBehaviour
{
    private FMOD.Studio.EventInstance platform;

    public void PlayRock(int actionType,float duration)
    {
        platform = FMODUnity.RuntimeManager.CreateInstance("event:/RockMovement");
        platform.setParameterByName("RockAction", actionType);
        platform.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        platform.start();
        platform.release();

        StartCoroutine(StopInstance(duration));
    }


    public IEnumerator StopInstance(float duration)
    {
        yield return new WaitForSeconds(duration);

        platform.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
