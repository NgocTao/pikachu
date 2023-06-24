using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public static class GameUtils
{
    public static void RemoveAllChildren(Transform parent)
    {
        int childs = parent.childCount;
        for (int i = childs - 1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(parent.GetChild(i).gameObject);
        }
    }

    public static T Clone<T>(T data)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(data));
    }

    public static void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }

    public static Vector3 GetPositionByHeightWithCamera(Camera cam, float height, Vector3 worldPoint, LayerMask maskLayers)
    {
        var direct = (worldPoint - cam.transform.position).normalized;
        var distance = Vector3.Distance(worldPoint, cam.transform.position);
        var ray = new Ray(cam.transform.position, direct);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, maskLayers))
        {
            return ray.GetPoint(distance - height);
        }
        else
        {
            return Vector3.zero;
        }
    }

    public static Vector3 GetPositionByScreenPointWithCamera(Camera cam, Vector3 screenPoint, LayerMask maskLayers)
    {
        var ray = cam.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, maskLayers))
        {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        var rng = new System.Random((int)System.DateTime.Now.Ticks);
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static System.DateTime UnixSecondToDateTime(double unixSecondsTimeStamp)
    {
        System.DateTime dtDateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixSecondsTimeStamp).ToUniversalTime();
        return dtDateTime;
    }

}