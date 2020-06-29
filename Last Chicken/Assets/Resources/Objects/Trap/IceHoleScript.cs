using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceHoleScript : CustomCollider
{
    public BoxCollider2D iceCollider;

    public float time = 0;

    public ParticleSystem iceParticle;

    void Update()
    {
        if (time > 0)
        {
            if (time >= 0 && time - Time.deltaTime <= 0)
                iceParticle.Play();
            time -= Time.deltaTime;
        }
        else if (time > -1)
        {
            time -= Time.deltaTime;
            if (IsAtPlayer(iceCollider) && Player.instance && Player.instance.playerHotTime <= 0)
                EffectManager.instance.IceEffect(0.1f);
        }
        else
        {
            Random.InitState(Random.Range(-100, 100) * (int)(10000 * Time.deltaTime));
            time = Random.Range(10, 20);
        }
    }
}
