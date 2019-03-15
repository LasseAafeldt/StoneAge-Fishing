using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleFlock : MonoBehaviour {

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
    public float neighbourDistance = 6.0f;


    bool turning = false;



    private void FixedUpdate()
    {
        InitializeIfNeeded();
        
        // GetParticles is allocation free because we reuse the m_Particles buffer between updates
        int numParticlesAlive = fishParticleEmitter.GetParticles(fishParticles);

        // Change only the particles that are alive
        for (int i = 0; i < numParticlesAlive; i++)
        {
            if (Vector3.Distance(fishParticles[i].position, fishParticleEmitter.transform.position) > neighbourDistance)
            {
               fishParticles[i].velocity = -(fishParticles[i].velocity+fishParticles[i].animatedVelocity);

                Debug.Log(fishParticles[i].velocity);
            }
        }

        // Apply the particle changes to the Particle System
        fishParticleEmitter.SetParticles(fishParticles, numParticlesAlive);
    }

    void InitializeIfNeeded()
    {
        if (fishParticleEmitter == null)
            fishParticleEmitter = GetComponent<ParticleSystem>();

        if (fishParticles == null || fishParticles.Length < fishParticleEmitter.main.maxParticles)
            fishParticles = new ParticleSystem.Particle[fishParticleEmitter.main.maxParticles];
    }
}
