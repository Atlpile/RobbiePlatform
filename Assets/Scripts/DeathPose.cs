using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPose : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);    //设置死亡效果，在场景中不被销毁
    }
}
