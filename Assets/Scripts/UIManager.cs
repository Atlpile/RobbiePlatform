using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;                                                            //用于使用TextMeshPro的功能

public class UIManager : MonoBehaviour
{
    static UIManager instance;                                          //定义实现单例模式的变量

    public TextMeshProUGUI orbText,timeText,deathText,gameOverText;     //设置文本组件


    private void Awake()                                        
    {
        //实现单例模式
        if (instance != null)
        {
            Destroy(gameObject);                                //销毁挂载脚本上的物体
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }
    

    //实时更新的UI
    public static void UpdateOrbUI(int orbCount)                //更新Orb UI的文本（接收传入的orb数量参数）
    {
        instance.orbText.text = orbCount.ToString();            //将orb数量参数转化为String类型，并将参数同步到orbText的文本中
    }
    public static void UpdateDeathUI(int deathCount)            //更新死亡次数UI的文本（接收传入的死亡次数参数）
    {
        instance.deathText.text = deathCount.ToString();        //将死亡次数参数转化为String类型，并将参数同步到deathText的文本中
    }
    public static void UpdateTimeUI(float time)                 //更新游戏运行时间UI的文本（接收传入的时间参数）
    {
        //以“分:秒”形式显示时间
        int minutes = (int)(time / 60);                                                 //设置分钟计数方式
        float seconds = time % 60;                                                      //设置秒钟计数方式
        instance.timeText.text = minutes.ToString("00")+":"+seconds.ToString("00");     //将时间参数转化为String类型，并将参数同步到timeText的文本中
    }
    

    //需要条件触发的UI
    public static void DisplayGameOver()                        //显示GameOverUI
    {
        instance.gameOverText.enabled = true;                   //启用gameOverText的游戏对象
    }
}
