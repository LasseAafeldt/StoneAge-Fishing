using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelicanEvent : MonoBehaviour
{
    bool hasHappening;

    public GameObject start, middle, end;

    public float t = 0.0f, velocitySpeed = 0.2f;

    Vector3 newPos = new Vector3(0, 0, 0);
    Vector3 forwardRotation = new Vector3(0, 0, 0);

    bool orcaMoving = true;

    float velocity = 0;
    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        //if(orcaMoving)
        {

            t += velocity * Time.deltaTime;
            if (t > 2.5)
            {
                t = 5.0f;
                velocity = 0;
                transform.parent.gameObject.SetActive(false);
                hasHappening = true;
                //GameManager.singleton.PelicanEvent.SetActive(true);
            }
            else
            {
                newPos =
                    Mathf.Pow(1 - t, 2) * start.transform.position
                    + 2 * (1 - t) * t * middle.transform.position
                    + Mathf.Pow(t, 2) * end.transform.position;

                //Debug.Log(transform.position);
                //Debug.Log(newPos);
                //Debug.DrawRay(transform.position, (newPos-transform.position)*100, Color.red, 1);
                forwardRotation = newPos - transform.position;
                transform.forward = -forwardRotation;
                
                transform.position = newPos;

            }

            if (t < 0.0) { t = 0.0f; }
        }

    }

    public void StartPelicanEvent()
    {

    }
    //old script that that is taken from the previous groups project.... don't fix if it isn't broken...
    public void startOrcaEvent()
    {
        if (!hasHappening)
        {
            playTimer.timeLeft += GameManager.singleton.partner.GetComponent<PartnerSpeech>().PelicanAppears.length;
            orcaMoving = true;
            velocity = velocitySpeed;

            GameManager.singleton.
                partner.GetComponent<PartnerSpeech>().PartnerSaysSomething(
                GameManager.singleton.partner.GetComponent<PartnerSpeech>().PelicanAppears);
                    
        }
    }
}

