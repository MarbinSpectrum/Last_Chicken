using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [Header("생성시만 이동")]
    public bool initMove;

    [Header("따라갈 오브젝트")]
    public GameObject followUI;

    [Header("오브젝트 크기 초기화")]
    public bool initSize;

    [Header("따라가는 오브젝트가 Transform 좌표계를 사용하는가")]
    public bool isTransfrom = true;

    [Header("따라갈 오브젝트가 Transform 좌표계를 사용하는가")]
    public bool atTransfrom = false;

    [Header("속도를 가지는가")]
    public bool hasSpeed = false;
    public float speedValue = 10;


    #region[Start]
    private void Start()
    {
        if(initSize)
            transform.localScale = new Vector3(1, 1, 1);
    }
    #endregion

    #region[Update]
    void Update()
    {
        if (initMove)
            return;
        if (isTransfrom)
        {
            if (hasSpeed)
            {
                if (atTransfrom)
                {
                    if (Vector2.Distance(transform.position, followUI.transform.position) < speedValue * Time.deltaTime)
                        transform.position = followUI.transform.position;
                    else
                    {
                        Vector2 dic = followUI.transform.position - transform.position;
                        dic = dic.normalized;

                        transform.position += (Vector3)dic * speedValue * Time.deltaTime;
                    }
                }
                else
                {
                    if (Vector2.Distance(transform.position, Camera.main.ScreenToWorldPoint(followUI.transform.position)) < speedValue * Time.deltaTime)
                        transform.position = Camera.main.ScreenToWorldPoint(followUI.transform.position);
                    else
                    {
                        Vector2 dic = Camera.main.ScreenToWorldPoint(followUI.transform.position) - transform.position;
                        dic = dic.normalized;

                        transform.position += (Vector3)dic * speedValue * Time.deltaTime;


                    }
                }
            }
            else
            {
                if(atTransfrom)
                {
                    float z = transform.position.z;
                    transform.position = new Vector3(followUI.transform.position.x, followUI.transform.position.y, z);
                }
                else
                {
                    float z = transform.position.z;
                    transform.position = Camera.main.ScreenToWorldPoint(followUI.transform.position);
                    transform.position = new Vector3(transform.position.x, transform.position.y, z);
                }
            }
        }
        else
        {
            if (hasSpeed)
            {
                if (atTransfrom)
                {
                    if (Vector2.Distance(transform.position, Camera.main.WorldToScreenPoint(followUI.transform.position)) < speedValue * Time.deltaTime)
                        transform.position = Camera.main.WorldToScreenPoint(followUI.transform.position);
                    else
                    {
                        Vector2 dic = Camera.main.WorldToScreenPoint(followUI.transform.position) - transform.position;
                        dic = dic.normalized;
                        transform.position += (Vector3)dic * speedValue * Time.deltaTime;
                    }
                }
                else
                {

                    if (Vector2.Distance(transform.position, followUI.transform.position) < speedValue * Time.deltaTime)
                        transform.position = followUI.transform.position;
                    else
                    {
                        Vector2 dic = followUI.transform.position - transform.position;
                        dic = dic.normalized;

                        transform.position += (Vector3)dic * speedValue * Time.deltaTime;
                    }
                }
            }
            else
            {
                if (atTransfrom)
                    transform.position = Camera.main.WorldToScreenPoint(followUI.transform.position);
                else
                    transform.position = followUI.transform.position;
            }
        }
    }
    #endregion

    #region[OnEnable]
    private void OnEnable()
    {
        if (initMove)
        {
            if (isTransfrom)
            {
                if (atTransfrom)
                {
                    float z = transform.position.z;
                    transform.position = new Vector3(followUI.transform.position.x, followUI.transform.position.y, z);
                }
                else
                {
                    float z = transform.position.z;
                    transform.position = Camera.main.ScreenToWorldPoint(followUI.transform.position);
                    transform.position = new Vector3(transform.position.x, transform.position.y, z);
                }
            }
            else
            {
                if (atTransfrom)
                    transform.position = Camera.main.WorldToScreenPoint(followUI.transform.position);
                else
                    transform.position = followUI.transform.position;
            }
        }
    }
    #endregion
}
