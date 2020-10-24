using UnityEngine;

// Copyright (C) 2018 Matthew K Wilson

namespace TerrainEngine2D
{
    [DisallowMultipleComponent]
    /// <summary>
    /// Base class for procedurally generating world block data
    /// </summary>
    public abstract class TerrainGenerator : MonoBehaviour
    {
        protected World world;
        protected WorldData worldData;
        protected FluidDynamics fluidDynamics;
        protected AdvancedFluidDynamics advancedFluidDynamics;

        private bool initialized;

        //Temporarily holding key sequence values for generating fluid data
        protected Vector2Int[,] fluidKey;
        //Value used for procedurally generating the block data
        protected int seed;
        //Random for randomly setting blocks
        protected System.Random random;

        /// <summary>
        /// Setup the variables used in generating the world
        /// </summary>
        /// <param name="world">Reference to the World</param>
        /// <param name="worldData">Reference to the World Data</param>
        /// <param name="fluidDynamics">Reference to the Fluid Dynamics system</param>
        /// <param name="advancedFluidDynamics">Reference to the Advanced Fluid Dynamics system</param>
        public void Initialize(World world, WorldData worldData, FluidDynamics fluidDynamics, AdvancedFluidDynamics advancedFluidDynamics)
        {
            this.world = world;
            this.worldData = worldData;
            this.fluidDynamics = fluidDynamics;
            this.advancedFluidDynamics = advancedFluidDynamics;
            fluidKey = new Vector2Int[world.WorldWidth, world.WorldHeight];
            seed = world.Seed;
            random = new System.Random(seed);
            initialized = true;
        }

        /// <summary>
        /// Procedurally generates world block data using random and pseudo-random functions
        /// </summary>
        /// <param name="world">Reference to the world to access block arrays</param>
        public virtual void GenerateData()
        {
            if (!initialized)
                Initialize(World.Instance, World.WorldData, FluidDynamics.Instance, AdvancedFluidDynamics.Instance);
        }

        /// <summary>
        /// Perlin Noise Function for generating block height data
        /// </summary>
        /// <param name="x">Block x coordinate</param>
        /// <param name="y">Block y coordinate</param>
        /// <param name="scale">Scaling; lower values gives more varying height output</param>
        /// <param name="mag">Magnitdue; higher values produce a larger height output</param>
        /// <param name="exp">Exponential; greatly increases height output</param>
        /// <returns>Returns the noise value</returns>
        protected int PerlinNoise(int x, int y, float scale, float mag, float exp)
        {
            return (int)(Mathf.Pow((Mathf.PerlinNoise((x + seed) / scale, (y + seed) / scale) * mag), (exp)));
        }

        /// <summary>
        /// Sets the world block data of a specified layer to a specific block
        /// </summary>
        /// <param name="x">Block x coordinate</param>
        /// <param name="y">Block y coordinate</param>
        /// <param name="layer">Layer to set the block data</param>
        /// <param name="blockType">The block type which is being set</param>
        /// <param name="probability">Percent chance of setting block (0 - 100)</param>
        protected void AddBlock(int x, int y, byte layer, byte blockType, float probability = 100f)
        {
            //Determine if the block should be added using the set probability
            if (!DoAddBlock(probability))
                return;

            world.GetBlockLayer(layer).AddBlock(x, y, (byte)(blockType + BlockLayer.BLOCK_INDEX_OFFSET));
            //Set the fluid blocks to solid if working with the fluid layer
            if (layer == world.FluidLayer)
            {
                //Get the block information
                BlockInfo blockInfo = world.GetBlockLayer(layer).GetBlockInfo(x, y);
                if (blockInfo != null && blockInfo.MultiBlock)
                {
                    //Loop through the size of the block
                    for (int posX = x; posX < x + blockInfo.TextureWidth; posX++)
                    {
                        for (int posY = y; posY < y + blockInfo.TextureHeight; posY++)
                        {
                            //If the block is in bounds set the fluid block to solid
                            if (InBounds(posX, posY))
                            {
                                if (world.BasicFluid)
                                    fluidDynamics.GetFluidBlock(posX, posY).SetSolid();
                                else
                                    advancedFluidDynamics.GetFluidBlock(posX, posY).SetSolid();
                            }
                        }
                    }
                }
                else
                {
                    if(world.BasicFluid)
                        fluidDynamics.GetFluidBlock(x, y).SetSolid();
                    else
                        advancedFluidDynamics.GetFluidBlock(x, y).SetSolid();
                }
            }
        }

        /// <summary>
        /// Checks if block should be set based on probability
        /// </summary>
        /// <param name="probability">The odds of setting the block</param>
        /// <returns>Returns true if block should be set</returns>
        protected bool DoAddBlock(double probability)
        {
            return (random.Next(100) + random.NextDouble()) <= probability;
        }

        /// <summary>
        /// Removes blocks from a specified layer and coordinate
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="layer">Layer to remove the block from</param>
        protected void RemoveBlock(int x, int y, byte layer)
        {
            //If there is a block at the coordinates
            if (world.GetBlockLayer(layer).IsBlockAt(x, y))
            {
                //Get the block information to get the size of the block
                BlockInfo blockInfo = world.GetBlockLayer(layer).GetBlockInfo(x, y);
                //Remove the block and get its position
                Vector2Int blockPos = world.GetBlockLayer(layer).RemoveBlock(x, y);
                //If working with the fluid layer, set all the blocks to empty
                if (layer == world.FluidLayer)
                {
                    if (blockInfo.MultiBlock)
                        RemoveFluid(blockPos.x, blockPos.y, blockInfo.TextureWidth, blockInfo.TextureHeight);
                    else
                    {
                        if(world.BasicFluid)
                            fluidDynamics.GetFluidBlock(x, y).SetEmpty();
                        else
                            advancedFluidDynamics.GetFluidBlock(x, y).SetEmpty();
                    }
                }
            }
        }

        /// <summary>
        /// Removes blocks from all layers at a coordinate
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        protected void RemoveAllBlocks(int x, int y)
        {
            //Loop through all the block layers and remove the blocks
            for(int i = 0; i < world.NumBlockLayers; i++)
            {
                RemoveBlock(x, y, (byte)i);
            }
        }


        /// <summary>
        /// Checks to see if a coordinate is within world bounds
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Returns true if the coordinate is within bounds</returns>
        bool InBounds(int x, int y)
        {
            if (x < 0 || x >= world.WorldWidth || y < 0 || y >= world.WorldHeight)
                return false;
            return true;
        }

        /// <summary>
        /// Checks if there is a block at a specific coordinate or area in the layer
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="layer">Layer to be checked</param>
        /// <param name="width">The width of the area (in blocks)</param>
        /// <param name="height">The height of the area (in blocks)</param>
        /// <returns>Returns true if there is a block</returns>
        protected bool IsBlockAt(int x, int y, byte layer, int width = 1, int height = 1)
        {
            bool isBlockAt = false;
            for(int i = x; i < x + width; i++)
            {
                for(int j = y; j < y + height; j++)
                {
                    if (world.GetBlockLayer(layer).IsBlockAt(i, j))
                    {
                        isBlockAt = true;
                        break;
                    }
                }
            }
            return isBlockAt;
        }

        /// <summary>
        /// Add fluid to a block at a coordinate (Basic)
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="weight">Amount of fluid to add</param>
        protected void AddFluid(int x, int y, float weight)
        {
            //Don't add any fluid if fluid is disabled
            if (world.FluidDisabled)
                return;
            if (!world.BasicFluid)
            {
                Debug.LogError("Using the wrong function, fluid is being added to the Basic Fluid System instead of the Advanced");
                return;
            }
            //Adds fluid to the block
            fluidDynamics.GetFluidBlock(x, y).AddWeight(weight);
        }

        /// <summary>
        /// Add fluid to a block at a coordinate (Advanced)
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="weight">Amount of fluid to add</param>
        /// <param name="density">The density of the fluid (or type)</param>
        protected void AddFluid(int x, int y, float weight, byte density)
        {
            //Don't add any fluid if fluid is disabled
            if (world.FluidDisabled)
                return;
            if (world.BasicFluid)
                throw new System.Exception("Using the wrong function, fluid is being added to the Advanced Fluid System instead of the Basic");
            //Adds fluid to the block
            advancedFluidDynamics.GetFluidBlock(x, y).AddWeight(density, weight, advancedFluidDynamics.GetFluidType(density).DefaultColor);
        }

        /// <summary>
        /// Add fluid to a block at a coordinate (Advanced)
        /// Custom Color
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="weight">Amount of fluid to add</param>
        /// <param name="density">The density of the fluid (or type)</param>
        /// <param name="customColor">Use a custom color instead of the fluid type default</param>
        protected void AddFluid(int x, int y, float weight, byte density, Color32 customColor)
        {
            //Don't add any fluid if fluid is disabled
            if (world.FluidDisabled)
                return;
            if (world.BasicFluid)
            {
                Debug.LogError("Using the wrong function, fluid is being added to the Advanced Fluid System instead of the Basic");
                return;
            }
            //Adds fluid to the block
            advancedFluidDynamics.GetFluidBlock(x, y).AddWeight(density, weight, customColor);
        }

        /// <summary>
        /// Remove fluid from an area
        /// </summary>
        /// <param name="x">X coordinate of area</param>
        /// <param name="y">Y coordinate of area</param>
        /// <param name="width">Width of area</param>
        /// <param name="height">Height of area</param>
        protected void RemoveFluid(int x, int y, int width = 1, int height = 1)
        {
            //Loop through the area
            for (int posX = x; posX < x + width; posX++)
            {
                for (int posY = y; posY < y + height; posY++)
                {
                    //If the block is in bounds set the fluid position to empty
                    if (InBounds(posX, posY))
                    {
                        if(world.BasicFluid)
                            fluidDynamics.GetFluidBlock(posX, posY).SetEmpty();
                        else
                            advancedFluidDynamics.GetFluidBlock(posX, posY).SetEmpty();
                    }
                }
            }
        }

        /// <summary>
        /// Recursive function for generating a pool of fluid below a threshold, previously set fluid blocks will be overwritten (Basic)
        /// </summary>
        /// <param name="x">Fluid block x coordinate</param>
        /// <param name="y">Fluid block y coordinate</param>
        /// <param name="weight">Amount of fluid added to the block</param>
        /// <param name="maxY">Heighest fluid point</param>
        /// <param name="key">The starting coordinate</param>
        /// <param name="newPool">Whether this is the </param>
        protected void GeneratePool(int x, int y, float weight, int maxY, Vector2Int key, bool newPool = true)
        {
            //Don't generate any pools if fluid is disabled
            if (world.FluidDisabled)
                return;

            if (!world.BasicFluid)
            {
                Debug.LogError("Using the wrong function, fluid is being added to the Basic Fluid System instead of the Advanced");
                return;
            }

            //Don't generate a new pool if there is already fluid in the current position
            if (newPool && fluidDynamics.GetFluidBlock(x, y).Weight > 0)
                return;
            //Return if the current position is outside world bounds
            if (x < 0 || x >= world.WorldWidth || y < 0 || y >= world.WorldHeight)
                return;
            //Empty block if it is not a part of this sequence
            if (fluidKey[x, y] != key && fluidDynamics.GetFluidBlock(x, y).Weight > 0)
                fluidDynamics.GetFluidBlock(x, y).Weight = 0;
            //Add fluid if block is empty
            if (fluidDynamics.GetFluidBlock(x, y).Weight == 0)
            {
                //Adds fluid to the block
                fluidDynamics.GetFluidBlock(x, y).Weight = weight;
                fluidDynamics.GetFluidBlock(x, y).Stable = true;
                //Set the key for the current block
                fluidKey[x, y] = key;
                //Add fluid to adjacent blocks if below the threshold
                GeneratePool(x - 1, y, weight, maxY, key, false);
                GeneratePool(x + 1, y, weight, maxY, key, false);
                GeneratePool(x, y - 1, weight + fluidDynamics.PressureWeight, maxY, key, false);
                if (y < maxY)
                    GeneratePool(x, y + 1, weight, maxY, key, false);
            }
        }

        /// <summary>
        /// Recursive function for generating a pool of fluid below a threshold, previously set fluid blocks will be overwritten (Advanced)
        /// </summary>
        /// <param name="x">Fluid block x coordinate</param>
        /// <param name="y">Fluid block y coordinate</param>
        /// <param name="weight">Amount of fluid added to the block</param>
        /// <param name="density">The density of the fluid (or type) - used by the Advanced Fluid Dynamics</param>
        /// <param name="maxY">Heighest fluid point</param>
        /// <param name="key">The starting coordinate</param>
        /// <param name="newPool">Whether this is the </param>
        protected void GeneratePool(int x, int y, float weight, byte density, int maxY, Vector2Int key, bool newPool = true)
        {
            //Don't generate any pools if fluid is disabled
            if (world.FluidDisabled)
                return;

            if (world.BasicFluid)
            {
                Debug.LogError("Using the wrong function, fluid is being added to the Advanced Fluid System instead of the Basic");
                return;
            }

            //Don't generate a new pool if there is already fluid in the current position
            if (newPool && advancedFluidDynamics.GetFluidBlock(x, y).Weight > 0)
                return;
            //Return if the current position is outside world bounds
            if (x < 0 || x >= world.WorldWidth || y < 0 || y >= world.WorldHeight)
                return;
            //Empty block if it is not a part of this sequence
            if (fluidKey[x, y] != key && advancedFluidDynamics.GetFluidBlock(x, y).Weight > 0)
                advancedFluidDynamics.GetFluidBlock(x, y).Weight = 0;
            //Add fluid if block is empty
            if (advancedFluidDynamics.GetFluidBlock(x, y).Weight == 0)
            {
                //Adds fluid to the block
                advancedFluidDynamics.GetFluidBlock(x, y).Weight = weight;
                advancedFluidDynamics.GetFluidBlock(x, y).Stable = true;
                advancedFluidDynamics.GetFluidBlock(x, y).Density = density;
                advancedFluidDynamics.GetFluidBlock(x, y).Color = advancedFluidDynamics.GetFluidType(density).DefaultColor;
                //Set the key for the current block
                fluidKey[x, y] = key;
                //Add fluid to adjacent blocks if below the threshold
                GeneratePool(x - 1, y, weight, density, maxY, key, false);
                GeneratePool(x + 1, y, weight, density, maxY, key, false);
                GeneratePool(x, y - 1, weight + advancedFluidDynamics.PressureWeight, density, maxY, key, false);
                if (y < maxY)
                    GeneratePool(x, y + 1, weight, density, maxY, key, false);
            }
        }

        /// <summary>
        /// Recursive function for generating a pool of fluid below a threshold, previously set fluid blocks will be overwritten (Advanced)
        /// Custom color
        /// </summary>
        /// <param name="x">Fluid block x coordinate</param>
        /// <param name="y">Fluid block y coordinate</param>
        /// <param name="weight">Amount of fluid added to the block</param>
        /// <param name="density">The density of the fluid (or type) - used by the Advanced Fluid Dynamics</param>
        /// <param name="customColor">Use a custom color instead of the fluid type default</param>
        /// <param name="maxY">Heighest fluid point</param>
        /// <param name="key">The starting coordinate</param>
        /// <param name="newPool">Whether this is the </param>
        protected void GeneratePool(int x, int y, float weight, byte density, Color32 customColor, int maxY, Vector2Int key, bool newPool = true)
        {
            //Don't generate any pools if fluid is disabled
            if (world.FluidDisabled)
                return;

            if (world.BasicFluid)
            {
                Debug.LogError("Using the wrong function, fluid is being added to the Advanced Fluid System instead of the Basic");
                return;
            }

            //Don't generate a new pool if there is already fluid in the current position
            if (newPool && advancedFluidDynamics.GetFluidBlock(x, y).Weight > 0)
                return;
            //Return if the current position is outside world bounds
            if (x < 0 || x >= world.WorldWidth || y < 0 || y >= world.WorldHeight)
                return;
            //Empty block if it is not a part of this sequence
            if (fluidKey[x, y] != key && advancedFluidDynamics.GetFluidBlock(x, y).Weight > 0)
                advancedFluidDynamics.GetFluidBlock(x, y).Weight = 0;
            //Add fluid if block is empty
            if (advancedFluidDynamics.GetFluidBlock(x, y).Weight == 0)
            {
                //Adds fluid to the block
                advancedFluidDynamics.GetFluidBlock(x, y).Weight = weight;
                advancedFluidDynamics.GetFluidBlock(x, y).Stable = true;
                advancedFluidDynamics.GetFluidBlock(x, y).Density = density;
                advancedFluidDynamics.GetFluidBlock(x, y).Color = customColor;
                //Set the key for the current block
                fluidKey[x, y] = key;
                //Add fluid to adjacent blocks if below the threshold
                GeneratePool(x - 1, y, weight, density, customColor, maxY, key, false);
                GeneratePool(x + 1, y, weight, density, customColor, maxY, key, false);
                GeneratePool(x, y - 1, weight + advancedFluidDynamics.PressureWeight, density, customColor, maxY, key, false);
                if (y < maxY)
                    GeneratePool(x, y + 1, weight, density, customColor, maxY, key, false);
            }
        }

        /// <summary>
        /// Recursive function for removing fluid below a threshold
        /// </summary>
        /// <param name="x">Fluid block x coordinate</param>
        /// <param name="y">Fluid block y coordinate</param>
        /// <param name="maxY">Heighest fluid point</param>
        protected void ClearFluid(int x, int y, int maxY)
        {
            //Don't clear if fluid is disabled
            if (world.FluidDisabled)
                return;
            //Return if the current position is outside world bounds
            if (x < 0 || x >= world.WorldWidth || y < 0 || y >= world.WorldHeight)
                return;

            if (world.BasicFluid)
            {
                if (fluidDynamics.GetFluidBlock(x, y).Weight <= 0)
                    return;
                fluidDynamics.GetFluidBlock(x, y).Weight = 0;
            } else
            {
                if (advancedFluidDynamics.GetFluidBlock(x, y).Weight <= 0)
                    return;
                advancedFluidDynamics.GetFluidBlock(x, y).Weight = 0;
            }

            //Remove fluid to adjacent blocks if below the threshold
            ClearFluid(x - 1, y, maxY);
            ClearFluid(x + 1, y, maxY);
            ClearFluid(x, y - 1, maxY);
            if (y < maxY)
                ClearFluid(x, y + 1, maxY);
            
        }
    }
}
