using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArrow : MonoBehaviour
{
    [SerializeField] private GameObject bodyPrefab;
    [SerializeField] private GameObject headPrefab;

    private const int AttackArrowPartsNumber = 17;
    private readonly List<GameObject> arrow = new List<GameObject>(AttackArrowPartsNumber);

    private Camera mainCamera;

    private bool isArrowEnabled;

    
    
    private void Start()
    {
        for (var i = 0; i < AttackArrowPartsNumber - 1; i++)
        {
            var body = Instantiate(bodyPrefab, gameObject.transform);
            arrow.Add(body);
        }

        var head = Instantiate(headPrefab, gameObject.transform);
        arrow.Add(head);

        foreach (var part in arrow)
        {
            part.SetActive(false);
        }

        mainCamera = Camera.main;
    }

    public void EnableArrow(bool arrowEnabled)
    {
        isArrowEnabled = arrowEnabled;
        foreach (var part in arrow)
        {
            part.SetActive(arrowEnabled);
        }
    }

    private void LateUpdate()
    {
        if (!isArrowEnabled)
        {
            return;
        }
        
        var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var mouseX = mousePos.x;
        var mouseY = mousePos.y;
        
        const float centerX = -15.0f;
        const float centerY = -1.0f;

        // for (var i = 0; i < arrow.Count; i++)
        // {
        //     var part = arrow[i];
        //     part.transform.position = new Vector3(centerX, centerY + 0.40f * i, 0.0f);
        // }
        
        // 这几个参数用于计算贝塞尔曲线的控制点
        var controlAx = centerX - (mouseX - centerX) * 0.3f;
        var controlAy = centerY + (mouseY - centerY) * 0.8f;
        var controlBx = centerX + (mouseX - centerX) * 0.1f;
        var controlBy = centerY + (mouseY - centerY) * 1.4f;

        for (var i = 0; i < arrow.Count; i++)
        {
            var part = arrow[i];
            
            
            // 计算箭身各部分在贝塞尔曲线中的位置
            // 不同箭身部分根据索引值，得到对应位置的t值
            // 越靠近箭尾部分，t值越小，
            // 越靠近箭头部分，t值越大
            var t = (i + 1) * 1.0f / arrow.Count;
            
            // t的平方
            var tt = t * t;
            
            // t的立方
            var ttt = tt * t;

            var u = 1.0f - t;
            
            // u的平方
            var uu = u * u;
            
            // u的立方
            var uuu = uu * u;
            
            // 计算位置
            // 这里使用贝塞尔曲线三阶立方公式
            // P0、P1、P2、P3四个点在平面或在三维空间中定义了三次方贝塞尔曲线。
            // 曲线起始于P0走向P1，并从P2的方向来到P3。一般不会经过P1或P2；
            // 这两个点只是在那里提供方向信息。P0和P1之间的间距，决定了曲线在转而趋进P2之前，走向P1方向的“长度有多长”。
            // 三阶曲线的参数形式为：
            // B(t) =  P0 * (1 - t) * (1 - t) * (1 - t) + 3 * P1 * t * (1 - t) * (1 - t) + 3 * P2 * t * t * (1 - t) + 
            //         P3 * t * t * t , 0 <= t <= 1
            // 这里设 u = (1- t), uu = (1 - t) * (1 - t), uuu =  (1 - t) * (1 - t) * (1 - t),
            // tt = t * t, ttt = t * t * t, 简化后得到:
            // B(t) = P0 * uuu + 3 * P1 * t * uu + 3 * P2 * tt * u + P3 * ttt , 0 <= t <= 1
            // 这里的P3就是centerX/centerY, P0是mouseX/mouseY, P1是controlAx/controlAy, P2是controlBx/controlBy
            // arrowX就是新的箭身组件X坐标值，arrowY就是新的箭身组件Y坐标值

            var arrowX = uuu * centerX +
                         3 * uu * t * controlAx +
                         3 * u * tt * controlBx +
                         ttt * mouseX;

            var arrowY = uuu * centerY +
                         3 * uu * t * controlAy +
                         3 * u * tt * controlBy +
                         ttt * mouseY;

            arrow[i].transform.position = new Vector3(arrowX, arrowY, 0.0f);
            
            // 计算箭身各部分精灵图片的朝向/角度
            float directionX;
            float directionY;

            if (i > 0)
            {
                // 除箭尾部分的方向计算
                directionX = arrow[i].transform.position.x - arrow[i - 1].transform.position.x;
                directionY = arrow[i].transform.position.y - arrow[i - 1].transform.position.y;
            }
            else
            {
                // 针对箭尾部分的方向计算
                directionX = arrow[i + 1].transform.position.x - arrow[i].transform.position.x;
                directionY = arrow[i + 1].transform.position.y - arrow[i].transform.position.y;
            }
            
            part.transform.rotation = Quaternion.Euler(0, 0, -Mathf.Atan2(directionX, directionY) * Mathf.Rad2Deg);

            part.transform.localScale = new Vector3(
                1.0f - 0.03f * (arrow.Count - 1 - i),
                1.0f - 0.03f * (arrow.Count - 1 - i),
                0);
            
        }
        
        
    }
}
