using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceAspectRatio : MonoBehaviour
{
    [SerializeField] float width = 16f;
    [SerializeField] float height = 9f;

    void Start()
    {
        float targetAspectRatio = width / height;

        float windowAspectRatio = (float)Screen.width / (float)Screen.height;

        // The amount to scale for the camera's viewport
        float scaleHeight = windowAspectRatio / targetAspectRatio;

        Camera cameraComp = GetComponent<Camera>();

        // If the game window height is smaller than the target aspect ratio height, add letterbox
        if (scaleHeight < 1f)
        {
            Rect rect = cameraComp.rect;

            rect.width = 1f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1f - scaleHeight) / 2f;

            cameraComp.rect = rect;
        }
        else if (scaleHeight > 1f) // add pillarbox
        {
            float scaleWidth = 1f / scaleHeight;

            Rect rect = cameraComp.rect;

            rect.width = scaleWidth;
            rect.height = 1f;
            rect.x = (1f - scaleWidth) / 2f;
            rect.y = 0;

            cameraComp.rect = rect;
        }
    }
}
