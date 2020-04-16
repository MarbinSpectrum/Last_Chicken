using UnityEngine;

public class Effect : MonoBehaviour
{
    Animator animator;

    public Vector2 Pivot;

    bool flag = false;

    private void OnEnable()
    {
        flag = false;
    }

    private void Awake()
    {
        try { animator = GetComponent<Animator>(); } catch { }
    }

    void Update()
    {
        if(!flag)
        {
            flag = true;
            transform.position += new Vector3(Pivot.x * (transform.localScale.x > 0 ? +1 : -1), Pivot.y, 0);
        }
        if (animator)
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                gameObject.SetActive(false);
    }
}