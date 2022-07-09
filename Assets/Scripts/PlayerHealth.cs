using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;                                                  //用于重置场景

public class PlayerHealth : MonoBehaviour
{
    public GameObject deathVFXPrefab1;                                              //设置死亡特效1
    public GameObject deathVFXPrefab2;                                              //设置死亡特效2
 
    int trapsLayer;                                                                 //定义用于获取图层的变量

    void Start()
    {
        trapsLayer = LayerMask.NameToLayer("Traps");                                //获取Traps的图层
    }
    //碰撞检测
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == trapsLayer)                                //若碰撞体检测到Traps图层
        {
            Instantiate(deathVFXPrefab1, transform.position, transform.rotation);   //生成预制体（生成死亡特效1，在player的当前位置，player的当前角度生成）
            // Instantiate(deathVFXPrefab2, transform.position, Quaternion.Euler(0, 0, Random.Range(-45, 90)));      //Prefab随机旋转产生 

            gameObject.SetActive(false);                                            //关闭player物体

            AudioManager.PlayDeathAudio();                                          //播放死亡音效

            GameManager.PlayerDied();                                               //使用player死亡功能
        }
    }
}
