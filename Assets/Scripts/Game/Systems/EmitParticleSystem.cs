using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum ParticleType
{
    PickObject,
    PutDownObject
}

[Serializable]
public struct Particle
{
    public ParticleType particleType;
    public List<ParticleSystem> particles;

    public delegate void Callback(Particle particle);

    public Particle(ParticleType particleType, List<ParticleSystem> particles)
    {
        this.particleType = particleType;
        this.particles = particles;
    }

    public async void Play(Vector3 position, Callback callback)
    {
        float maxDuration = 0;

        foreach (ParticleSystem p in particles)
        {
            p.gameObject.transform.position = position;
            p.Play();

            maxDuration = MathF.Max(maxDuration, p.main.duration);
        }

        await Task.Delay((int)(maxDuration * 1000));
        callback(this);
    }
}

public class EmitParticleSystem : MonoBehaviour
{
    [SerializeField] List<Particle> particles;

    Dictionary<ParticleType, ObjectPool<Particle>> objectPool = new();
    Dictionary<ParticleType, Particle> particlesDictionary = new();

    public static EmitParticleSystem instance;

    private ParticleType currentParticleType;

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < particles.Count; i++) {

            ObjectPool<Particle> pool = new ObjectPool<Particle>();
            pool.OnPoolIsEmpty += AddParticle;

            objectPool.Add(particles[i].particleType, pool);
            particlesDictionary.Add(particles[i].particleType, particles[i]);
        }
    }

    public void Play(ParticleType particleType, Vector3 position)
    {
        if (objectPool.ContainsKey(currentParticleType) == false)
        {
            Debug.LogWarning($"There is no particle with type {particleType}");
            return;
        }

        currentParticleType = particleType;
        Particle particle = objectPool[currentParticleType].GetObject();

        particle.Play(position, BackParticle);
    }

    private void BackParticle(Particle particle) {
        objectPool[particle.particleType].AddObject(particle);
    }

    private void AddParticle()
    {
        List<ParticleSystem> list = new List<ParticleSystem>();
        for (int i = 0; i < particlesDictionary[currentParticleType].particles.Count; i++) {
            ParticleSystem p = Instantiate(particlesDictionary[currentParticleType].particles[i]);
            list.Add(p);
        }
        Particle particle = new Particle(currentParticleType, list);
        objectPool[currentParticleType].AddObject(particle);
    }

    private void OnDestroy()
    {
        foreach (var p in objectPool) { 
            p.Value.OnPoolIsEmpty -= AddParticle;
        }
    }
}