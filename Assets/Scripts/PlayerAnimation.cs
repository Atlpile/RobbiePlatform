using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    //定义用于获取组件的变量
    Animator anim;
    PlayerMovement movement;
    Rigidbody2D rb;

    //定义用于获取动画参数的变量
    int groundID;
    int hangingID;
    int crouchID;
    int speedID;
    int fallID;             //Blend Tree中跳跃速度参数


    void Start()
    {
        //获取组件
        anim = GetComponent<Animator>();
        movement = GetComponentInParent<PlayerMovement>();  //获取父级中的PlayerMovement脚本
        rb = GetComponentInParent<Rigidbody2D>();           //获取父级中的Rigidbody2D组件

        //获取动画参数
        groundID = Animator.StringToHash("isOnGround");     //StringToHash函数可以将字符型转化为数值型，使其为变量形式
        hangingID = Animator.StringToHash("isHanging");
        crouchID = Animator.StringToHash("isCrouching");
        speedID = Animator.StringToHash("speed");
        fallID = Animator.StringToHash("verticalVelocity"); //Blend Tree中影响跳跃的参数

    }
    void Update()
    {
        //用字符形式播放动画
        //anim.SetFloat("speed", Mathf.Abs(movement.xVelocity));    //调用speed动画参数
        //anim.SetBool("isOnGround", movement.isOnGround);          //调用isOnGround动画参数

        //用编号形式调用动画参数（播放动画）
        anim.SetFloat(speedID, Mathf.Abs(movement.xVelocity));      //动画中的速度参数，只需要绝对值即可
        anim.SetBool(groundID, movement.isOnGround);                
        anim.SetBool(hangingID, movement.isHanging);
        anim.SetBool(crouchID, movement.isCrouch);
        anim.SetFloat(fallID, rb.velocity.y);                       //使用父级Rigidbody 2D的参数，来播放跳跃动画
    }


    //Animation Event：动画事件
    public void StepAudio()                         //走路时的声音
    {
        AudioManager.PlayFootstepAudio();           //播放动画时，播放走路声音
    }
    public void CrouchStepStudio()                  //下蹲走路时的声音
    {
        AudioManager.PlayerCrouchFootstepAudio();
    }
}
