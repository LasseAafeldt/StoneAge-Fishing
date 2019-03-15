using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalFlockPart : MonoBehaviour
{

    public ParticleSystem fishParticleEmitter;
    ParticleSystem.Particle[] fishParticles;
    public GameObject goalPrefab;
    public static int schoolSize = 5;

    static int numberofFish = 15;



    public static GameObject[] allFish = new GameObject[numberofFish];

    public static Vector3 goalPos = Vector3.zero;

    public float speed = 0.5f;
    float rotationSpeed = 4.0f;
    Vector3 averageHeading;
    Vector3 averagePosition;
    float neighbourDistance = 3.0f;


    bool turning = false;

    // Use this for initialization
    void Start()
    {
        fishParticleEmitter = GetComponent<ParticleSystem>();
        fishParticles = new ParticleSystem.Particle[fishParticleEmitter.main.maxParticles];
    }

    // Update is called once per frame
    void Update()
    {

        if (Random.Range(0, 10000) < 50)
        {
            goalPos = new Vector3(Random.Range(-schoolSize, schoolSize), Random.Range(-schoolSize, schoolSize), Random.Range(-schoolSize, schoolSize));
            goalPrefab.transform.position = goalPos;
        }
        for (int i = 0; i < fishParticleEmitter.GetParticles(fishParticles); i++)
        {
            if (Vector3.Distance(transform.position, Vector3.zero) >= globalFlock.schoolSize)
            {
                turning = true;
            }
            else
            {
                turning = false;
            }

            if (turning)
            {
                Vector3 direction = Vector3.zero - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
                
            }
            if (Random.Range(0, 5) < 1)
                ApplyRules();
        }

    }
    void ApplyRules()
    {
        ParticleSystem.Particle[] gos;
        gos = fishParticles;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.1f;

        Vector3 goalPos = globalFlock.goalPos;

        float dist;

        int groupSize = 0;
        foreach (ParticleSystem.Particle go in gos)
        {
            
                dist = Vector3.Distance(go.position, this.transform.position);
                if (dist <= neighbourDistance)
                {
                    vcentre += go.position;
                    groupSize++;

                    if (dist < 1.0f)
                    {
                        vavoid = vavoid + (this.transform.position - go.position);
                    }

                    //flock anotherFlock = go.GetComponent<flock>();
                    gSpeed = gSpeed; //+ anotherFlock.speed;
                }
            
        }

        if (groupSize > 0)
        {
            vcentre = vcentre / groupSize + (goalPos - this.transform.position);
            speed = gSpeed / groupSize;

            Vector3 direction = (vcentre + vavoid) - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            }
        }
    }
}
