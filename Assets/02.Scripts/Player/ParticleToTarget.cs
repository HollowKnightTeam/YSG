using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleToTarget : MonoBehaviour
{
    Transform player;
    ParticleSystem particle;
    ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];
    int count;
    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        particle = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        count = particle.GetParticles(particles);
        for (int a = 0; a < count; a++)
        {
            ParticleSystem.Particle particle_ = particles[a];
        }
    }

}
