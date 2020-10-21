using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playTimer : MonoBehaviour {

    public PartnerSpeech partnerSpeech;

	public float startValue;
	Vector3 startAngle;
	public float endValue;
	Vector3 endAngle;

	Vector3 currentAngle = new Vector3(0,0,0);

	public float totalPlayTime =300;
	public static float timeLeft;
	float timeSpent=0;

	public Material skyBox;
    [Range(345f,380f)]
	public float skyboxAngle;
    [Range(0.5f,1f)]
	public float skyboxExposure;
    public Light lightSource;

    AudioSource guide;

	// 360 -> 345

	//talking booleans
	bool oneMinLeft=true;
	bool twoMinLeft=true;

	bool sixMinLeft=true;
	bool fourMinLeft=true;

    bool endHasCome = false;


	// Use this for initialization
	void Start () {
        oneMinLeft = true;
        twoMinLeft = true;

        sixMinLeft = true;
        fourMinLeft = true;

        endHasCome = false;

        timeLeft = totalPlayTime;

		Vector3 startAngle = new Vector3(startValue,23,3);	
		Vector3 endAngle = new Vector3(endValue,0,0);

        guide = GameManager.singleton.guide.GetComponent<AudioSource>();
	}
	
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		timeSpent = timeLeft/totalPlayTime;

        UpdateSkybox();
        RotateSkybox();

        IfNotLinear();
        if(timeSpent <= 0 && !endHasCome)
        {
            StartCoroutine(RunOutOfTime(partnerSpeech.GameTimerEnd));
            endHasCome = true;
        }
	}

    void UpdateSkybox()
    {
        if (timeSpent > 0)
        {
            skyboxAngle = (timeSpent) * 380 + (1 - timeSpent) * 345;
            skyboxExposure = (timeSpent) * (float)1 + (1 - timeSpent) * (float)0.5;
            currentAngle.x = (timeSpent) * startValue + (1 - timeSpent) * endValue;
            lightSource.intensity = (timeSpent) * (float)1.2;
        }
        else
        {
            currentAngle.x = endValue;
            lightSource.intensity = 0;
        }
    }

    void RotateSkybox()
    {
        skyBox.SetFloat("_RotationZ", skyboxAngle);
        skyBox.SetFloat("_Exposure", skyboxExposure);

        transform.eulerAngles = new Vector3(currentAngle.x, 23, 3);
    }

    IEnumerator RunOutOfTime(AudioClip clip)
    {
        if (guide.isPlaying)
        {
            StartCoroutine(waitForSoundToEnd(guide.clip));
            yield break;
        }
        partnerSpeech.PartnerSaysSomething(partnerSpeech.GameTimerEnd);
        Object.FindObjectOfType<SceneLoadManager>().ChangeScene(clip.length);
        GameManager.singleton.boat.GetComponent<EventCatcher>().CheckForEnding();
    }

	public float GetTimeSpent()
	{
		return timeSpent;
	}

    void IfNotLinear()
    {
        if (timeSpent <= 0.5 && fourMinLeft)
        {
            fourMinLeft = false;
        }
        //when there is two minutes left
        if (timeSpent <= 0.4 && twoMinLeft)
        {
            twoMinLeft = false;
        }
        //when there is one minutes left
        if (timeSpent <= 0.2 && oneMinLeft)
        {
            oneMinLeft = false;
            partnerSpeech.PartnerSaysSomething(partnerSpeech.GameTimerLow);
        }
        
    }
    IEnumerator waitForSoundToEnd(AudioClip clip)
    {
        yield return new WaitForSeconds(clip.length);
        StartCoroutine(RunOutOfTime(partnerSpeech.GameTimerEnd));
    }
}
