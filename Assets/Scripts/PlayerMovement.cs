using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //设置用于获取组件的变量
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [Header("移动参数")]
    public float speed = 8f;                //设置player左右移动速度，初始值为8
    public float crouchSpeedDivisir = 3f;   //设置player下蹲移动速度，初始值为3

    [Header("跳跃参数")]
    public float jumpForce = 6.3f;          //设置跳跃力，初始值为6.3
    public float jumpHoldForce = 1.9f;      //设置长按跳跃的跳跃力，初始值为1.9
    public float jumpHoldDuration = 0.1f;   //设置长按跳跃的持续时间，初始值为0.1
    public float crouchJumpBoost = 2.5f;    //设置下蹲跳跃的额外跳跃加成，初始值为2.5
    public float hangingJumpForce = 15f;    //设置悬挂跳跃的额外跳跃加成，初始值为15

    float jumpTime;                         //定义长按跳跃可持续的时间？？？（设置下次跳跃的时间间隔）

    [Header("状态")]
    public bool isCrouch;                   //判断player是否处于下蹲状态
    public bool isOnGround;                 //判断player是否处于地面状态（是否站在地面上）
    public bool isJump;                     //判断player是否处于跳跃状态（是否处于浮空状态）
    public bool isHeadBlocked;              //判断player是否处于遮挡头部状态（是否头顶撞墙）
    public bool isHanging;                  //判断player是否处于悬挂状态

    [Header("环境检测")]
    public float footOffset = 0.4f;         //设置射线左右两脚之间的位置
    public float headClearance = 0.5f;      //检测头顶距离
    public float groundDistance = 0.2f;     //设置射线地面检测深度

    float playerHeight;                     //设置角色头顶位置射线（角色高度射线）？？？
    public float eyeHeight = 1.5f;          //设置眼睛射线
    public float grabDistance = 0.4f;       //设置挂墙距离
    public float reachOffset = 0.7f;        //设置悬挂触发范围

    public LayerMask groundLayer;           //定义物体是否为groundLayer图层

    public float xVelocity;                 //定义x轴速度的方向

    //设置按键的状态
    bool jumpPressed;                       //瞬按跳跃状态
    bool jumpHeld;                          //长按跳跃状态
    bool crouchHeld;                        //长按下蹲状态
    bool crouchPressed;                     //瞬按下蹲状态

    //设置站立和下蹲时，碰撞体的大小和位置
    Vector2 colliderStandSize;
    Vector2 colliderStandOffset;
    Vector2 colliderCrouchSize;
    Vector2 colliderCrouchOffset;


    void Start()
    {
        //获取组件
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        playerHeight = coll.size.y;                                                 //设置player身高，为当前BoxCollider的y轴高度

        //获取站立时碰撞体的大小和位置
        colliderStandSize = coll.size;                                              //设置player站立时的Size，为当前BoxCollider的Size
        colliderStandOffset = coll.offset;                                          //设置player站立时的Offset，为当前BoxCollider的Offset
        //设置下蹲时碰撞体的大小和位置
        colliderCrouchSize = new Vector2(coll.size.x, coll.size.y / 2f);            //设置player下蹲时的Size，为BoxCollider中当前y轴Size的1/2
        colliderCrouchOffset = new Vector2(coll.offset.x, coll.offset.y / 2f);      //设置player下蹲时的Offset，为BoxCollider中当前y轴Offset的1/2

        transform.position = GameManager.instance.FirstPosition;                    //设置存档点位置？？？
    }
    void Update()
    {
        if (GameManager.GameOver())                     //若检测到游戏结束
            return;                                     //则不执行以下功能（人物不再移动）

        if (Input.GetButtonDown("Jump"))                //若按下跳跃按键
            jumpPressed = true;                         //触发瞬按跳跃状态

        jumpHeld = Input.GetButton("Jump");             //设置长按跳跃状态的实时检测，由Jump预设按键决定
        crouchHeld = Input.GetButton("Crouch");         //设置长按下蹲状态的实时检测，由Crouch预设按键决定

        if (Input.GetButtonDown("Crouch"))              //若按下下蹲按键
            crouchPressed = true;                       //触发瞬按下蹲状态
    }
    private void FixedUpdate()
    {
        if (GameManager.GameOver())                     //若检测到游戏结束
        {
            //额外拓展
            xVelocity = 0f;                             //将player的x轴移动速度设为0（使player不能移动）
            rb.bodyType = RigidbodyType2D.Static;       //将player的物理模式设为static（使player停止移动）
            return;                                     //则不执行以下功能（物理检测、人物移动和跳跃功能）
        }

        PhysicsCheck();                                 //物理检测
        GroundMovement();                               //player的移动功能
        MidAirMovement();                               //player的跳跃功能
    }


    //player的功能
    void GroundMovement()       //人物的移动
    {
        if (isHanging)                                                  //若player处于悬挂状态
            return;                                                     //不执行以下功能（player的移动和翻转）

        //下蹲与站立之间的状态切换
        if (crouchHeld && isOnGround && !isCrouch)
            Crouch();
        else if (isCrouch && !crouchHeld && !isHeadBlocked)
            StandUp();
        else if (isCrouch && !isOnGround)
            StandUp();

        xVelocity = Input.GetAxis("Horizontal");                        //设置player的x轴速度方向，由Horizontal的按键预设决定

        if (isCrouch)                                                   //若player处于下蹲状态
            xVelocity /= crouchSpeedDivisir;                            //改变player的x轴移动速度（由下蹲的移动速度决定）

        rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);    //实现人物移动

        FlipDirection();                                                //实现player的翻转功能
    }
    void Hanging()              //人物的悬挂
    {
        //悬挂时的跳跃状态
        if (isHanging)                                                          //若处于悬挂状态
        {
            if (jumpPressed)                                                    //若处于瞬按跳跃状态
            {
                rb.bodyType = RigidbodyType2D.Dynamic;                          //将player的物理模式还原为Dynamic
                rb.velocity = new Vector2(rb.velocity.x, hangingJumpForce);     //设置瞬按跳跃状态的跳跃速度
                isHanging = false;                                              //player处于悬挂状态关闭

                jumpPressed = false;                                            //（手感优化）瞬按跳跃状态关闭
            }
            if (crouchPressed)                                                  //若处于瞬按下蹲状态
            {
                rb.bodyType = RigidbodyType2D.Dynamic;                          //将player的物理模式还原为Dynamic
                isHanging = false;                                              //player处于悬挂状态关闭

                crouchPressed = false;                                          //（手感优化）瞬按下蹲状态关闭
            }

            //手感优化：悬挂状态的（额外）触发条件
            if (crouchPressed && crouchHeld)                                     //手感优化：若长按下蹲且瞬按下蹲（当添加crouchPressed条件时，按下下蹲键可以取消悬挂）
            {
                rb.bodyType = RigidbodyType2D.Static;                           //将player的物理模式设为Static（使人物悬挂在墙上）
                isHanging = true;                                               //player处于悬挂状态开启
            }

            if (crouchPressed && jumpPressed)                                    //手感优化：长按下蹲时且瞬按跳跃
            {
                rb.bodyType = RigidbodyType2D.Static;
                isHanging = true;
            }

            if (crouchHeld && jumpHeld)                                         //手感优化：长蹲下蹲时，且长按跳跃
            {
                rb.bodyType = RigidbodyType2D.Static;
                isHanging = true;
            }
        }
    }
    void MidAirMovement()       //人物的跳跃
    {
        Hanging();

        //瞬按跳跃实现的功能
        if (jumpPressed && isOnGround && !isJump && !isHeadBlocked)                 //若处于瞬按跳跃状态，且player处于地面状态，且player未处于跳跃状态，且player头顶未被遮挡
        {
            if (isCrouch)                                                            //若player处于下蹲状态（即：player处于下蹲状态的瞬按跳跃）
            {
                StandUp();                                                          //实现人物站起功能
                rb.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse); //为player添加一个附带下蹲加成的跳跃力（实现人物跳跃，并有下蹲状态加成)  
            }

            isOnGround = false;                                                     //player处于地面状态关闭
            isJump = true;                                                          //player处于跳跃状态开启

            jumpTime = Time.time + jumpHoldDuration;                                //下次跳跃的时间为：当前时间 + 长按跳跃按键的持续时间

            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);           //实现跳跃功能

            AudioManager.PlayJumpAudio();                                           //播放player跳跃音效

            jumpPressed = false;                                                    //（手感优化）瞬按跳跃状态关闭
        }
        else if (isJump)                                                            //若player处于跳跃状态
        {
            if (jumpHeld)                                                           //若为长按跳跃状态
                rb.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);   //实现长按跳跃功能
            if (jumpTime < Time.time)                                               //若下次跳跃时间 < 当前时间
                isJump = false;                                                     //player处于跳跃状态关闭

            jumpPressed = false;                                                    //（手感优化）瞬按跳跃状态关闭
        }
    }
    void FlipDirection()        //人物的翻转
    {
        if (xVelocity < 0)                                  //若x轴速度<0
            transform.localScale = new Vector3(-1, 1, 1);   //设置人物向左翻转
        if (xVelocity > 0)                                  //若x轴速度>0
            transform.localScale = new Vector3(1, 1, 1);    //设置人物向右翻转
    }
    void Crouch()               //人物的下蹲
    {
        isCrouch = true;                        //player的下蹲状态开启
        //使用下蹲时碰撞体的大小和位置
        coll.size = colliderCrouchSize;         //设置player下蹲时碰撞体的size，为colliderCrouchSize
        coll.offset = colliderCrouchOffset;     //设置player下蹲时碰撞体的offset，为colliderCrouchOffset
    }
    void StandUp()              //人物的站立
    {
        isCrouch = false;                       //player的下蹲状态关闭
        //使用站立时碰撞体的大小和位置
        coll.size = colliderStandSize;          //设置player站立时碰撞体的size，为colliderCrouchSize
        coll.offset = colliderStandOffset;      //设置player站立时碰撞体的offset，为colliderCrouchOffset
    }


    //设置射线检测的默认参数
    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDiraction, float length, LayerMask layer)       //射线检测：设置（射线起点，射线方向，射线长度，图层检测）
    {
        Vector2 pos = transform.position;                                                           //获取player的坐标（中心点），定义为pos
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, length, layer);            //定义射线检测信息hit：（人物中心点位置+射线起点，射线方向，长度，图层）
        Color color = hit ? Color.red : Color.green;                                                //设置射线颜色：触发射线检测则为红色，未触发射线检测则为绿色
        Debug.DrawRay(pos + offset, rayDiraction * length, color);                                  //显示射线（人物中心点位置+射线起点，射线方向，射线长度，射线颜色）

        return hit;                                                                                 //将射线检测的信息hit返回给Raycast方法
    }
    void PhysicsCheck()         //物理检测
    {
        //地面检测（射线的起点，射线的方向，射线的长度，射线检测的目标图层）
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance, groundLayer);

        if (leftCheck || rightCheck)    //若触发地面射线检测
            isOnGround = true;          //player处于地面状态开启
        else isOnGround = false;        //player处于地面状态关闭


        //头顶检测（射线的起点，射线的方向，射线的长度，射线检测的目标图层）
        RaycastHit2D headCheck = Raycast(new Vector2(0f, coll.size.y), Vector2.up, headClearance, groundLayer);

        if (headCheck)                  //若触发头顶射线检测
            isHeadBlocked = true;       //player处于遮挡头部状态开启
        else isHeadBlocked = false;     //player处于遮挡头部状态关闭


        //【未理解】
        //悬挂检测（射线的起点，射线的方向，射线的长度，射线检测的目标图层）
        float direction = transform.localScale.x;                                           //定义射线方向direction，为player当前的x轴方向
        Vector2 grabDir = new Vector2(direction, 0f);                                       //定义攀爬射线方向

        RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset * direction, playerHeight), grabDir, grabDistance, groundLayer);         //头顶射线检测（人物高度射线起点，射线方向，射线距离，射线图层判断）
        RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), grabDir, grabDistance, groundLayer);               //眼睛射线检测（眼睛高度射线起点......）
        RaycastHit2D ledgeCheck = Raycast(new Vector2(reachOffset * direction, playerHeight), Vector2.down, grabDistance, groundLayer);     //悬挂射线检测（悬挂高度射线起点，射线朝下......）

        /*
            若player不处于地面状态
            且player的y轴速度<0（处于浮空状态）
            且3种悬挂检测都触发
        */
        if (!isOnGround && rb.velocity.y < 0f && ledgeCheck && wallCheck && !blockedCheck)   //若触发悬挂状态
        {
            //设置人物悬挂的固定位置（刚好处于头顶）
            Vector3 pos = transform.position;                   //获取player坐标位置，定义为pos
            pos.x += (wallCheck.distance - 0.2f) * direction;   //设置player的x轴位置（设置player离墙面的距离）
            pos.y -= ledgeCheck.distance;                       //设置player的y轴位置（设置player离墙面的高度）
            transform.position = pos;                           //将预设的player位置坐标传回组件（设置悬挂的固定位置）

            rb.bodyType = RigidbodyType2D.Static;               //设置player的物理模式为static（使人物悬挂在墙上）

            isHanging = true;                                   //player处于悬挂状态开启
        }
    }
}
