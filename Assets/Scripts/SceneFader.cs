using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    Animator anim;                                  //定义用于获取动画控制器的变量

    int faderID;                                    //定义用于获取动画参数编号的变量

    private void Start()
    {
        anim = GetComponent<Animator>();            //获取组件

        faderID = Animator.StringToHash("Fade");    //获取动画参数

        
        GameManager.RegisterSceneFader(this);       //将SceneFader脚本参数，传递给RegisterSceneFader方法中（即：在GameManager中使用SceneFader脚本中的功能）
    }

    public void FadeOut()                           //褪色效果
    {
        anim.SetTrigger(faderID);                   //播放褪色动画
    }
}
