using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
    public static Vector3[] GetPaths(Vector3 sou, Vector3 des, float delta = 3f)
    {
        List<Vector3> paths = new List<Vector3>();

        Vector3 direct = (des - sou).normalized;
        Vector3 directCross = new Vector3(direct.y, direct.x * -1f, 0);
        float distance = Vector3.Distance(sou, des);
        Vector3 between = sou + direct * distance * 0.5f;

        between += directCross * delta * Vector3.Distance(sou, des) * 0.5f;

        paths.Add(des);
        paths.Add(sou + (between - sou).normalized * Mathf.Abs(delta) * 0.5f);
        paths.Add(des + (between - des).normalized * Mathf.Abs(delta) * 0.5f);

        return paths.ToArray();
    }
}