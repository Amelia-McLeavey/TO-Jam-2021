using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBlit : MonoBehaviour
{
    private static Material mat;

    public static void QueueBlit(Material postProcessMat) {
        mat = postProcessMat;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (mat != null) {
            Graphics.Blit(source, destination, mat);
        }
        mat = null;
    }
}
