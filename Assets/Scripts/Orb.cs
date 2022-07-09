using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public GameObject explosionVFXPrefab;       //设置爆炸粒子效果预制体

    int player;                                 //定义用于获取图层的变量

    void Start()
    {
        player = LayerMask.NameToLayer("Player");                                       //获取Player图层
                                                                                        
        GameManager.RegisterOrb(this);                                                  //将Orb脚本参数，传递给RegisterOrb方法中（用于统计场景中Orb（脚本）的数量，并存放到列表中）
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == player)                                       //若碰撞体检测到player图层
        {
            Instantiate(explosionVFXPrefab, transform.position, transform.rotation);    //生成预制体（生成爆炸特效，在player的当前位置，player的当前角度生成）

            gameObject.SetActive(false);                                                //关闭Orb的游戏对象

            AudioManager.PlayOrbAudio();                                                //播放player获取宝珠音效

            GameManager.PlayerGrabbedOrb(this);                                         //将SceneFader脚本参数，传递给PlayerGrabbedOrb方法中（用于更新UI中，场景的Orb数量）
        }
    }
}
