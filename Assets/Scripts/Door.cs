using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator anim;                                  //定义用于获取Animator的变量

    int openID;                                     //定义用于获取Open动画参数的变量

    void Start()
    {
        anim = GetComponent<Animator>();            //获取Animator组件
        openID = Animator.StringToHash("Open");     //获取Animator中Open动画参数，并转换为整型

        GameManager.RegisterDoor(this);             //将Door脚本中的值传递给GameManager脚本的RegisterDoor方法中
    }

    public void Open()                              //开门功能
    {
        anim.SetTrigger(openID);                    //将openID动画参数，传入到Animator中（用于播放开门动画）

        AudioManager.PlayDoorOpenAudio();           //使用播放开门音效功能
    }

}
