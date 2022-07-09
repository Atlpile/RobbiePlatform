using UnityEngine;

public class SwingAxe : MonoBehaviour
{
	public AnimationCurve swingPattern;         //定义摇摆模式，为动画曲线类型
	public float halfArcPerSecond = 25f;		//半弧秒
	public float angleForAudio = 2f;            //音频角度

	AudioSource audioSource;
	float timeModifier;                         //时间修饰符
	float elapsedTime;                          //经过的时间
	float swingSize;                            //摇摆大小
	int direction;								//方向

	void Start ()
	{
		audioSource = GetComponent<AudioSource>();						//获取音源组件

		float currentAngle = transform.rotation.eulerAngles.z;			//设置当前角度为：尤拉角的z轴

		timeModifier = Mathf.Abs(currentAngle) / halfArcPerSecond;		//设置时间修饰符为：尤拉角的绝对值 / 半弧秒
		swingSize = Mathf.Abs(currentAngle);							//设置摇摆范围为：尤拉角的绝对值

		if (currentAngle > 0f)											//若当前角度为0
			direction = 1;												//方向为1
		else
			direction = -1;												//方向为-1
	}

	void Update()
	{
		elapsedTime += Time.deltaTime / timeModifier;									//设置经过的时间

		if (elapsedTime >= 1f)															//若经过的时间>1f
			elapsedTime -= 1f;															//则经过的时间为-1f

		float angle = swingPattern.Evaluate(elapsedTime) * swingSize * direction;		//设置角度为

		if (angle < angleForAudio && angle > -angleForAudio)                            //若角度<音频角度 且 角度>负的音频角度
			audioSource.Play();															//播放声音

		Vector3 rot = transform.rotation.eulerAngles;									//设置rot为尤拉角，为三维向量类型
		rot.z = angle;																	//尤拉角的z轴数值 = angle
		transform.rotation = Quaternion.Euler(rot);										//旋转值为：rot尤拉角的值
	}
}
