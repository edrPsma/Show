using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AreaDebug
{
    /// <summary>
    /// 绘制圆环
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="radius"></param>
    /// <param name="color"></param>
    public static void DrawWireRound(Vector3 pos, float radius, Color color)
    {
        Vector3 beginPoint = Vector3.zero;
        Vector3 firstPoint = Vector3.zero;
        for (float theta = 0; theta < 2 * Mathf.PI; theta += 0.1f)
        {
            float x = radius * Mathf.Cos(theta);
            float z = radius * Mathf.Sin(theta);
            Vector3 endPoint = new Vector3(x, 0, z) + pos;
            if (theta == 0)
            {
                firstPoint = endPoint;
            }
            else
            {
                Debug.DrawLine(beginPoint, endPoint, color);
            }
            beginPoint = endPoint;
        }

        // 绘制最后一条线段
        Debug.DrawLine(firstPoint, beginPoint, color);
    }

    /// <summary>
    /// 绘制矩形区域
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="quaternion"></param>
    /// <param name="width"></param>
    /// <param name="length"></param>
    /// <param name="color"></param>
    public static void DrawWireArea(Vector3 pos, Quaternion quaternion, float length, float width, Color color)
    {
        Vector3 point1 = pos + quaternion * new Vector3(-length * 0.5f, 0, -width * 0.5f);
        Vector3 point2 = pos + quaternion * new Vector3(-length * 0.5f, 0, width * 0.5f);
        Vector3 point3 = pos + quaternion * new Vector3(length * 0.5f, 0, width * 0.5f);
        Vector3 point4 = pos + quaternion * new Vector3(length * 0.5f, 0, -width * 0.5f);

        Debug.DrawLine(point1, point2, color);
        Debug.DrawLine(point2, point3, color);
        Debug.DrawLine(point3, point4, color);
        Debug.DrawLine(point4, point1, color);
    }

    /// <summary>q
    /// 绘制扇形区域
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="quaternion"></param>
    /// <param name="radius"></param>
    /// <param name="angle"></param>
    /// <param name="color"></param>
    public static void DrawWireSector(Vector3 pos, Quaternion quaternion, float radius, float angle, Color color)
    {
        Vector3 beginPoint = pos;
        Vector3 firstPoint = pos;
        for (float theta = -Mathf.Deg2Rad * angle * 0.5f; theta <= Mathf.Deg2Rad * angle * 0.5f; theta += 0.1f)
        {
            float x = radius * Mathf.Cos(theta);
            float z = radius * Mathf.Sin(theta);
            Vector3 endPoint = quaternion * new Vector3(x, 0, z) + pos;

            Debug.DrawLine(beginPoint, endPoint, color);
            beginPoint = endPoint;
        }

        float xEnd = radius * Mathf.Cos(Mathf.Deg2Rad * angle * 0.5f);
        float zEnd = radius * Mathf.Sin(Mathf.Deg2Rad * angle * 0.5f);
        Vector3 end = quaternion * new Vector3(xEnd, 0, zEnd) + pos;
        Debug.DrawLine(beginPoint, end, color);

        // 绘制最后一条线段
        Debug.DrawLine(firstPoint, end, color);
    }
}
