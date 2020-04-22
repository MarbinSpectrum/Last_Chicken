using UnityEngine;

// Copyright (C) 2018 Matthew K Wilson

namespace TerrainEngine2D
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    /// <summary>
    /// A chunk of fluid blocks for rendering
    /// </summary>
    public class FluidChunk : MonoBehaviour
    {
        protected World world;
        [SerializeField]
        protected Chunk chunk;

        protected FluidDynamics fluidDynamics;
        protected AdvancedFluidDynamics advancedFluidDynamics;
        //Reference to the fluid block array
        protected FluidBlock[,] fluidBlocks;
        protected AdvancedFluidBlock[,] advancedFluidBlocks;

        //Holds mesh information for rendering the chunk
        protected BlockGridMesh blockGridMesh;
        //The secondary color for fluid 
        protected Color32 secondaryColor;
        //The primary color for fluid
        protected Color32 mainColor;

        protected bool update;
        /// <summary>
        /// Used to update the mesh when fluid blocks change
        /// </summary>
        public bool Update
        {
            set { update = value; }
        }

        public virtual void Awake()
        {
            gameObject.layer = LayerMask.NameToLayer("Terrain");
        }

        public virtual void Start()
        {
            world = World.Instance;
            fluidDynamics = FluidDynamics.Instance;
            advancedFluidDynamics = AdvancedFluidDynamics.Instance;
            if (world.BasicFluid)
            {
                fluidBlocks = fluidDynamics.FluidBlocks;
                //Get the fluid colors
                mainColor = fluidDynamics.MainColor;
                secondaryColor = fluidDynamics.SecondaryColor;
            } else
            {
                advancedFluidBlocks = advancedFluidDynamics.FluidBlocks;
            }
            //Initialize the block grid mesh
            blockGridMesh = new BlockGridMesh(GetComponent<MeshFilter>().mesh, chunk.ChunkSize, world.ZBlockDistance, true, 1, true);
            BuildChunk();
        }

        public virtual void LateUpdate()
        {
            //Rebuild the fluid chunk if it needs to be updated
            if (update)
            {
                BuildChunk();
                update = false;
            }
        }

        /// <summary>
        /// Build the chunk mesh
        /// </summary>
        public virtual void BuildChunk()
        {
            if (world.BasicFluid)
            {
                //Loop through the grid of chunks
                for (int x = 0; x < chunk.ChunkSize; x++)
                {
                    for (int y = 0; y < chunk.ChunkSize; y++)
                    {
                        //Get the current fluid block
                        FluidBlock fluidBlock = fluidBlocks[x + chunk.ChunkX, y + chunk.ChunkY];
                        float minWeight = fluidDynamics.MinWeight;
                        //Create a fluid block if its weight is above the minimum threshold
                        if (fluidBlock.Weight > minWeight)
                        {
                            //Calculate the z-order for the fluid (renders just behind the fluid layer)
                            float zOrder = world.GetBlockLayer(world.FluidLayer).ZLayerOrder + world.ZBlockDistance / 4f;
                            //Calculate the color of the mesh based on the fluid weight (higher weight means darker color)
                            Color32 color = Color32.Lerp(secondaryColor, mainColor, fluidBlock.Weight / 4f);
                            bool topDown = fluidDynamics.TopDown;
                            float height = !topDown ? fluidBlock.GetHeight() : 1;
                            //Add the fluid block to the mesh
                            blockGridMesh.CreateBlock(x, y, zOrder, new Vector2(0, 0), 0, 1, 1, 1, height, color);
                        }
                    }
                }
            } 
            else
            {
                //Loop through the grid of chunks
                for (int x = 0; x < chunk.ChunkSize; x++)
                {
                    for (int y = 0; y < chunk.ChunkSize; y++)
                    {
                        //Get the current fluid block
                        AdvancedFluidBlock fluidBlock = advancedFluidBlocks[x + chunk.ChunkX, y + chunk.ChunkY];
                        float minWeight = advancedFluidDynamics.MinWeight;
                        //Create a fluid block if its weight is above the minimum threshold
                        if (fluidBlock.Weight > minWeight)
                        {
                            //Calculate the z-order for the fluid (renders just behind the fluid layer)
                            float zOrder = world.GetBlockLayer(world.FluidLayer).ZLayerOrder + world.ZBlockDistance / 4f;
                            //Calculate the color of the mesh based on the fluid weight (higher weight means darker color)
                            Color color = fluidBlock.Color;
                            bool topDown = advancedFluidDynamics.TopDown;
                            float height = !topDown ? fluidBlock.GetHeight() : 1;
                            //Add the fluid block to the mesh
                            blockGridMesh.CreateBlock(x, y, zOrder, new Vector2(0, 0), 0, 1, 1, 1, height, color);
                        }
                    }
                }
            }
            //Update the mesh
            blockGridMesh.UpdateMesh();
        }
    }
}