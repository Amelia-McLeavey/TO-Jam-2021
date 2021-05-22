using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAudio : MonoBehaviour
{
    private FMOD.Studio.EventInstance platform1;
    private FMOD.Studio.EventInstance platform2;
    private FMOD.Studio.EventInstance platform3;
    private FMOD.Studio.EventInstance platform4;
    private FMOD.Studio.EventInstance platform5;

    public void PlayRock(int actionType,float duration)
    {
        switch (actionType)
        {
            case 0:
                platform1 = FMODUnity.RuntimeManager.CreateInstance("event:/RockMovement");
                platform1.setParameterByName("RockAction", actionType);
                platform1.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                platform1.start();
                platform1.release();
                break;
            case 1:
                platform2 = FMODUnity.RuntimeManager.CreateInstance("event:/RockMovement");
                platform2.setParameterByName("RockAction", actionType);
                platform2.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                platform2.start();
                platform2.release();
                break;
            case 2:
                platform3 = FMODUnity.RuntimeManager.CreateInstance("event:/RockMovement");
                platform3.setParameterByName("RockAction", actionType);
                platform3.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                platform3.start();
                platform3.release();
                break;
            case 3:
                platform4 = FMODUnity.RuntimeManager.CreateInstance("event:/RockMovement");
                platform4.setParameterByName("RockAction", actionType);
                platform4.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                platform4.start();
                platform4.release();
                break;
            case 4:
                platform5 = FMODUnity.RuntimeManager.CreateInstance("event:/RockMovement");
                platform5.setParameterByName("RockAction", actionType);
                platform5.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                platform5.start();
                platform5.release();
                break;
        }


        Invoke("StopRockingOut", duration);

        //StartCoroutine(StopInstance(duration));
        //Debug.Log(duration);
    }


    //public IEnumerator StopInstance(float duration)
    //{
    //    yield return new WaitForSeconds(duration);

    //    platform.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    //}

    public void StopRockingOut()
    {

        platform1.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        platform2.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        platform3.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        platform4.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        platform5.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
