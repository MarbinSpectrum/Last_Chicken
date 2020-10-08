using UnityEngine;

// Copyright (C) 2018 Matthew K Wilson

namespace TerrainEngine2D.Lighting
{
    /// <summary>
    /// A textured mesh used to render block lighting
    /// </summary>
    [RequireComponent(typeof(BlockLighting))]
    public class BlockLightMesh : TexturedMesh
    {
        protected World world;
        protected ChunkLoader chunkLoader;
        [HideInInspector]
        [SerializeField]
        protected BlockLighting blockLighting;
        public BlockLighting BlockLighting
        {
            get { return blockLighting; }
        }

        protected virtual void OnEnable()
        {
            ChunkLoader.OnChunksLoaded += UpdateTexture;
            if (blockLighting != null)
                blockLighting.OnLightGenerated += UpdateTexture;
        }

        protected virtual void OnDisable()
        {
            ChunkLoader.OnChunksLoaded -= UpdateTexture;
            if (blockLighting != null)
                blockLighting.OnLightGenerated -= UpdateTexture;
        }

        protected override void Awake()
        {
            base.Awake();
            blockLighting = GetComponent<BlockLighting>();
            gameObject.layer = LayerMask.NameToLayer("Lighting");
        }

        protected override void Start()
        {
            world = World.Instance;
            chunkLoader = ChunkLoader.Instance;
            //Set the size of the texture
            width = chunkLoader.LoadedWorldWidth;
            height = chunkLoader.LoadedWorldHeight;

            base.Start();
            //Set the z position/render order for the light system
            transform.position = new Vector3(transform.position.x, transform.position.y,transform.position.z + world.EndZPoint - world.ZBlockDistance * world.ZLayerFactor * World.LIGHT_SYSTEM_Z_ORDER);
        }

        protected override void LateUpdate()
        {
            if (update && !world.LightingDisabled)
            {
                transform.position = new Vector3(chunkLoader.OriginLoadedChunks.x, chunkLoader.OriginLoadedChunks.y, transform.position.z);
                base.LateUpdate();
            }
        }

        protected override Color32[] GetPixelData()
        {
            Color32[,] pixels = blockLighting.LightMap;
            //Current position of the texture
            int posX = (int)transform.position.x;
            int posY = (int)transform.position.y;

            int index = 0;
            //Grab the a section of the pixel data from the current position of the loaded world
            for (int y = posY; y < posY + height; y++)
            {
                for (int x = posX; x < posX + width; x++)
                {
                    //Set the pixel data to the array
                    tempPixelData[index] = pixels[x, y];
                    index++;
                }
            }
            return tempPixelData;
        }
    }
}