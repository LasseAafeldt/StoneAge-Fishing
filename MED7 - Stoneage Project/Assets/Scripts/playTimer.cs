using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playTimer : MonoBehaviour {

    public PartnerSpeech partnerSpeech;

	public float startValue;
	public float endValue;

	public float totalPlayTimeSec =300;
	public static float timeLeft;
	float timeRemainingNormalized=0;

	public Material skyBox;
    [Range(345f,380f)]
	public float skyboxAngle;
    [Range(0.5f,1f)]
	public float skyboxExposure, minSkyboxExposure, maxSkyboxExposure;
    [SerializeField] private AnimationCurve skyboxExposureCurve;

    public Light lightSource;
    //[SerializeField] private float intensityOffset = 0.2f;
    [Tooltip("x=0 is corresponds to beginning of game while x=1 corresponds to end of game")]
    [SerializeField] private AnimationCurve intensityControl;

	Vector3 startAngle;
	Vector3 endAngle;

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

        timeLeft = totalPlayTimeSec;

		Vector3 startAngle = new Vector3(startValue,23,3);	
		Vector3 endAngle = new Vector3(endValue,0,0);

        guide = GameManager.singleton.guide.GetComponent<AudioSource>();
	}
	
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		timeRemainingNormalized = timeLeft/totalPlayTimeSec;

        UpdateSkybox();
        RotateSkybox();

        IfNotLinear();
        if(timeRemainingNormalized <= 0 && !endHasCome)
        {
            StartCoroutine(RunOutOfTime(partnerSpeech.GameTimerEnd));
            endHasCome = true;
        }
	}

    void UpdateSkybox()
    {
        //lerping
        if (timeRemainingNormalized > 0)
        {
            skyboxAngle = Mathf.Lerp(endValue, startValue, timeRemainingNormalized);
            //skyboxExposure = Mathf.Lerp(minSkyboxExposure, maxSkyboxExposure, timeRemainingNormalized);
            lightSource.intensity = intensityControl.Evaluate(1 - timeRemainingNormalized);
        }
    }

    void RotateSkybox()
    {
        skyBox.SetFloat("_RotationZ", -skyboxAngle);
        //skyBox.SetFloat("_Exposure", skyboxExposure);
        skyBox.SetFloat("_Exposure", skyboxExposureCurve.Evaluate(1 - timeRemainingNormalized));


        transform.eulerAngles = new Vector3(skyboxAngle, 23, 3);
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

    void IfNotLinear()
    {
        if (timeRemainingNormalized <= 0.5 && fourMinLeft)
        {
            fourMinLeft = false;
        }
        //when there is two minutes left
        if (timeRemainingNormalized <= 0.4 && twoMinLeft)
        {
            twoMinLeft = false;
        }
        //when there is one minutes left
        if (timeRemainingNormalized <= 0.2 && oneMinLeft)
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
