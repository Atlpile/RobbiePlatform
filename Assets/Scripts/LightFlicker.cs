// This script causes the light component of a child object to flicker in a realistic
// fashion. This is used on the wall torches. Since this script is slightly expensive
// and purely cosmetic, it will only run on non-mobile platforms

//该脚本会使子对象的灯光组件以逼真的方式闪烁。这是用在墙上的手电筒。由于这个脚本稍微昂贵，而且纯粹是装饰性的，所以它只能在非移动平台上运行

using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public float amount;	//The amount of light flicker							设置灯光闪烁的次数
    public float speed;		//The speed of the flicker								设置灯光闪烁的速度
    
	//控制灯光变量
    Light localLight;		//Reference to the light component						定义用于获取Light组件的变量
    float intensity;        //The collective intensity of the light component		定义光的强度变量
	float offset;           //An offset so all flickers are different				定义光照偏移变量


	void Awake()
	{
		//If this is a mobile platform, remove this script							如果这是移动平台，请删除此脚本
		if (Application.isMobilePlatform)
			Destroy(this);
	}

	void Start()
    {
		//Get a reference to the Light component on the child game object			
		localLight = GetComponentInChildren<Light>();								//获取灯光组件

		//Record the intensity and pick a random seed number to start				
		intensity = localLight.intensity;											//设置光照强度，为光照强度的初始值
        offset = Random.Range(0, 10000);											//设置光照偏移的范围为0~10000
    }

	void Update ()
	{
		//Using perlin noise, determine a random intensity amount					使用perlin噪点函数，使其随机闪烁
		float amt = Mathf.PerlinNoise(Time.time * speed + offset, Time.time * speed + offset) * amount;
		localLight.intensity = intensity + amt;										//设置当前的光照强度，为光照强度的初始值+随机噪点值
	}
}
