using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainEngine2D;

public class BubbleScript : MonoBehaviour
{
    public float speed;
    float time = 0;

    #region[Update]
    void Update()
    {
        BubbleRemove();
        BubbleMove();
    }
    #endregion

    #region[OnEnable]
    private void OnEnable()
    {
        time = 0;
    }
    #endregion

    #region[방울 이동]
    public void BubbleMove()
    {
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);
    }
    #endregion

    #region[방울 사라짐]
    public void BubbleRemove()
    {
        if (time >= 10)
            gameObject.SetActive(false);

        if (!CheckInFluid())
            time = 100;
    }
    #endregion

    #region[물속인지검사]
    bool CheckInFluid()
    {
        if (AdvancedFluidDynamics.Instance && AdvancedFluidDynamics.Instance.GetFluidBlock((int)transform.position.x, (int)transform.position.y + 1) != null)
        {
            bool fluidCheck = AdvancedFluidDynamics.Instance.GetFluidBlock((int)transform.position.x, (int)transform.position.y + 1).Weight > 0.1f;

            return fluidCheck;
        }
        return false;
    }
    #endregion
}
