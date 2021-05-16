using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{

    private FMOD.Studio.EventInstance footstep;
    private FMOD.Studio.EventInstance land;

    public void PlayFootstep()
    {
        footstep = FMODUnity.RuntimeManager.CreateInstance("event:/Footstep");
        footstep.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        footstep.start();
        footstep.release();
    }

    public void PlayCharacterLand()
    {
        land = FMODUnity.RuntimeManager.CreateInstance("event:/CharacterLand");
        land.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        land.start();
        land.release();
    }
}
