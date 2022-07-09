using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;                        //用于控制音效组件

public class AudioManager : MonoBehaviour       
{
    static AudioManager current;                //定义实现单例模式所需的变量

    [Header("环境声音")]
    public AudioClip ambientClip;               //设置环境音效
    public AudioClip musicClip;                 //设置背景音乐

    [Header("音效")]
    public AudioClip deathFXClip;               //设置player死亡后，Orb归还音效
    public AudioClip orbFXClip;                 //设置player获取Orb音效
    public AudioClip doorFXClip;                //设置开门音效
    public AudioClip startLevelClip;            //设置场景开始音效
    public AudioClip winClip;                   //设置player通关音效

    [Header("Robbie音效")]
    public AudioClip[] walkStepClips;           //设置一组走路音效
    public AudioClip[] crouchStepClips;         //设置一组下蹲音效
    public AudioClip jumpClip;                  //设置player跳跃音效
    public AudioClip deathClip;                 //设置player死亡音效

    [Header("Robbie人声")]
    public AudioClip jumpVoiceClip;             //设置player跳跃时人声音效
    public AudioClip deathVoiceClip;            //设置player死亡时人声音效
    public AudioClip orbVoiceClip;              //设置player获取Orb时人声音效

    //添加AudioSource（音源）组件
    AudioSource ambientSource;                  //定义ambientSource   为AudioSource组件类型（音源类型/播放器组件类型）
    AudioSource musicSource;                    //定义用于获取背景音乐音源的变量
    AudioSource fxSource;                       //定义用于获取环境音效音源的变量
    AudioSource playerSource;                   //定义用于获取player音效音源的变量
    AudioSource voiceSource;                    //定义用于获取player人声音源的变量

    public AudioMixerGroup ambientGroup, musicGroup, FXGroup, playerGroup, voiceGroup;

    private void Awake()                                                    
    {
        //实现单例模式
        if(current != null)
        {
            Destroy(gameObject);
            return;
        }
        current = this;                                                     //将当前（AudioManager）类的值，赋值给实例current

        DontDestroyOnLoad(gameObject);                                      //使AudioManager一直存在（场景重新加载也保留）

        //为AudioManager游戏对象添加AudioSource组件
        ambientSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        fxSource = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();
        voiceSource = gameObject.AddComponent<AudioSource>();

        //将AudioSource组件，用AudioMixer功能控制
        ambientSource.outputAudioMixerGroup = ambientGroup;
        playerSource.outputAudioMixerGroup = playerGroup;
        musicSource.outputAudioMixerGroup = ambientGroup;
        fxSource.outputAudioMixerGroup = ambientGroup;
        voiceSource.outputAudioMixerGroup = voiceGroup;

        StartLevelAudio();                                                  //播放游戏开始时的音频
    }


    void StartLevelAudio()                                                  //游戏开始时播放的音频
    {
        current.ambientSource.clip = current.ambientClip;                   //将ambientSource组件的当前音效，切换为ambientClip的音效
        current.ambientSource.loop = true;                                  //启用ambientSource组件的循环播放功能
        current.ambientSource.Play();                                       //播放ambientSource组件中的音效

        current.musicSource.clip = current.musicClip;                       //将musicSource组件的当前音效，切换为musicClip的音效
        current.musicSource.loop = true;                                    //启用musicSource组件的循环播放功能
        current.musicSource.Play();                                         //播放musicSource组件中的音效

        current.fxSource.clip = current.startLevelClip;                     //将fxSource组件的当前音效，切换为startLevelClip的音效
        current.fxSource.Play();                                            //播放fxSource组件中的音效
    }

    public static void PlayerWonAudio()                                     //播放Player胜利音效
    {
        current.fxSource.clip = current.winClip;                            
        current.fxSource.Play();                                            
        current.playerSource.Stop();                                        //停止播放playerSource中的音效
    }
    public static void PlayDoorOpenAudio()                                  //播放开门音效
    {
        current.fxSource.clip = current.doorFXClip;                         
        current.fxSource.PlayDelayed(1f);                                   //在fxSource中延迟播放1s音效
    }
    public static void PlayFootstepAudio()                                  //播放player行走音效
    {
        int index = Random.Range(0, current.walkStepClips.Length);          //设置player行走音效的随机播放范围，由行走音效的长度（数量）决定，定义为index

        current.playerSource.clip = current.walkStepClips[index];           //将fxSource组件的当前音效，切换为walkStepClips的音效
        current.playerSource.Play();                                        //在playerSource中播放音效

    }
    public static void PlayerCrouchFootstepAudio()                          //播放player蹲走音效
    {
        int index = Random.Range(0, current.crouchStepClips.Length);

        current.playerSource.clip = current.crouchStepClips[index];
        current.playerSource.Play();
    }
    public static void PlayJumpAudio()                                      //播放player跳跃音效和人声
    {
        current.playerSource.clip = current.jumpClip;
        current.playerSource.Play();                                        

        current.voiceSource.clip = current.jumpVoiceClip;                   
        current.voiceSource.Play();                                         
    }
    public static void PlayDeathAudio()                                     //播放player死亡音效和人声，以及Orb重置音效
    {
        current.playerSource.clip = current.deathClip;                      
        current.playerSource.Play();

        current.voiceSource.clip = current.deathVoiceClip;                  
        current.voiceSource.Play();

        current.fxSource.clip = current.deathFXClip;                        
        current.fxSource.Play();
    }
    public static void PlayOrbAudio()                                       //播放player获得Orb的音效和人声
    {
        current.fxSource.clip = current.orbFXClip;                          
        current.fxSource.Play();                                            

        current.voiceSource.clip = current.orbVoiceClip;                    
        current.voiceSource.Play();                                         
    }
}
