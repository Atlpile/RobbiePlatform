using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    //定义用于获取图层的变量
    int playerLayer;                                        


    void Update()
    {
        playerLayer = LayerMask.NameToLayer("Player");      //获取Player图层
    }


    //碰撞检测
    private void OnTriggerEnter2D(Collider2D collision)     
    {
        if (collision.gameObject.layer == playerLayer)      //若碰撞到Player图层
            Debug.Log("Player Won!");

        GameManager.PlayerWon();                            //使用Player胜利功能
    }
}
