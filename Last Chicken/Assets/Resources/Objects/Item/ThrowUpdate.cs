using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowUpdate : MonoBehaviour
{
    public string throwName;

    public ThrowObject throwObject;

    Animator animator;

    #region[Awake]
    public virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
    #endregion

    #region[Update]
    public virtual void Update()
    {
        if (animator)
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                gameObject.SetActive(false);
    }
    #endregion

    #region[OnEnable]
    public virtual void OnEnable()
    {
        if (ItemManager.FindData(throwName) != -1)
            throwObject.damage = Mathf.FloorToInt(ItemManager.instance.itemData[ItemManager.FindData(throwName)].value0);
    }
    #endregion
}
