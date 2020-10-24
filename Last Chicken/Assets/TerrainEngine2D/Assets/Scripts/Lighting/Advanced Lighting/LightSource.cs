using UnityEngine;
using Custom;
// Copyright (C) 2018 Matthew K Wilson

namespace TerrainEngine2D.Lighting
{
    /// <summary>
    /// A source of light
    /// </summary>
    public abstract class LightSource : MonoBehaviour
    {
        /// <summary>
        /// Color of the light source
        /// </summary>
        [Tooltip("Color of the light source")]
        public Color32 LightColor = new Color32(255, 255, 255, 255);



        protected Vector2Int keyPosition;
        /// <summary>
        /// The key position of the light source
        /// This is used as the dictionary key value for referencing light sources in the AdvancedLightSystem script
        /// </summary>
        public Vector2Int KeyPosition
        {
            get { return keyPosition; }
        }

        protected virtual void Awake()
        {
            keyPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        }

        protected virtual void Start()
        {
            // AdvancedLightSystem.Instance.SendMessage("LightAdded", this, SendMessageOptions.RequireReceiver);
        }

        protected virtual void Update()
        {
            if(new Vector2Int((int)transform.position.x, (int)transform.position.y) != keyPosition)
            {
                LightOff();
                keyPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);
                if(World.Instance && World.Instance.GetBlockLayer(1).GetBlockInfo(keyPosition.x, keyPosition.y) == null)
                 LightOn();
            }
            // AdvancedLightSystem.Instance.SendMessage("LightAdded", this, SendMessageOptions.RequireReceiver);
        }

        //protected virtual void OnDestroy()
        //{
        //    if (AdvancedLightSystem.Instance != null)
        //        AdvancedLightSystem.Instance.SendMessage("LightRemoved", this, SendMessageOptions.DontRequireReceiver);
        //}

        protected virtual void OnEnable()
        {
            keyPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);
            LightOn();
        }

        protected virtual void OnDisable()
        {
            LightOff();
        }
        protected virtual void LightOn()
        {
            if (AdvancedLightSystem.Instance)
                AdvancedLightSystem.Instance.SendMessage("LightAdded", this, SendMessageOptions.RequireReceiver);
        }


        protected virtual void LightOff()
        {
            if (AdvancedLightSystem.Instance != null)
                AdvancedLightSystem.Instance.SendMessage("LightRemoved", this, SendMessageOptions.DontRequireReceiver);
        }
    }
}