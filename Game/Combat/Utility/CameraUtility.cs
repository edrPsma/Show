using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraUtility
{
    public static Vector3 ScenePos2UIPos(Camera sceneCamera, RectTransform uiTrans, Vector3 pos)
    {
        Vector3 screenePos = sceneCamera.WorldToScreenPoint(pos);
        Vector3 worldPos;

        RectTransformUtility.ScreenPointToWorldPointInRectangle(uiTrans, screenePos, GameEntry.UI.UICamera, out worldPos);

        return worldPos;
    }

    public static Vector3 ViewPort2GamePos(Camera gameCamera, Vector3 pos)
    {
        return gameCamera.ViewportToWorldPoint(pos);
    }
}
