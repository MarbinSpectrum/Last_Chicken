using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainEngine2D.Lighting;
using Custom;

public class DustScript : MonoBehaviour
{
    public Animator animator;
    public GameObject img;

    int delayTime;

    #region[OnEnable]
    void OnEnable()
    {
        StartCoroutine(AniCoroutine());
    }
    #endregion

    IEnumerator AniCoroutine()
    {
        delayTime = Random.Range(0, 10);
        float time = 0;
        while(time < delayTime)
        {
            time += 0.1f;
            LightCheck();
            yield return new WaitForSeconds(0.1f);
        }
        time = 0;

        animator.SetTrigger("Act");

        while (time < delayTime)
        {
            time += 0.1f;
            LightCheck();
            yield return new WaitForSeconds(0.1f);
        }

        StartCoroutine(AniCoroutine());
    }

    public void LightCheck()
    {
        if(CaveManager.inCave)
        {
            img.SetActive(false);
            return;
        }
        Vector2Int nowPos = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
        if (AdvancedLightSystem.Instance)
        {
            if (AdvancedLightSystem.Instance.BlockLighting.LightMap == null)
                return;

            if (!Exception.IndexOutRange(nowPos.x, nowPos.y, AdvancedLightSystem.Instance.BlockLighting.LightMap))
                return;

            Color32 lightColor = AdvancedLightSystem.Instance.BlockLighting.LightMap[nowPos.x, nowPos.y];

            if (lightColor.r > 3 || lightColor.g > 3 || lightColor.b > 3)
                img.SetActive(false);
            else
                img.SetActive(true);
        }
    }
}
