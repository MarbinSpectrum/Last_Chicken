using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMapSnow : MonoBehaviour
{
    ParticleSystem particleSystem;

    bool playerIce = false;

    float iceState = 0;
    float time = 0;

    void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag.Equals("Player"))
            iceState += 2*Time.deltaTime;
        
    }

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        time += Time.deltaTime;
        time %= 60;

        if (time < 40 || CaveManager.inCave)
            particleSystem.Stop();
        else
            particleSystem.Play();

        iceState -= Time.deltaTime;
        if (iceState < 0)
            iceState = 0;
        if (iceState > 2.1f)
            iceState = 2.1f;

        if (iceState > 2)
            EffectManager.instance.IceEffect(0);


    }
}
