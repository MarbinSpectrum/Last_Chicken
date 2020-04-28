using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainEngine2D;
using Custom;

public class Lantern : MonoBehaviour
{
    public GameObject lightMain;

    public bool blink;

    float time = 0;
    float delayTime = 0.1f;

    Vector3 keyPoint;
    Vector3 nonActPos = new Vector3(10000, 10000, 10000);

    void OnEnable()
    {
        keyPoint = transform.position;
        time = 0;
    }

    private void Update()
    {
        if (blink)
        {
            //return;
            if (delayTime < time && time <= delayTime + 0.1f && transform.position != nonActPos)
            {
                keyPoint = transform.position;
                transform.position = nonActPos;
            }
            else if (time > delayTime + 0.1f)
            {
                transform.position = keyPoint;
                time = 0;

                delayTime = Random.Range(3, 8);
            }
            time += Time.deltaTime;
        }
    }

    private IEnumerator Blink()
    {
        lightMain.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        lightMain.SetActive(true);

        if(blink)
        {
            yield return new WaitForSeconds(Random.Range(3, 8));
            StartCoroutine(Blink());
        }
    }
}
