using UnityEngine;

//Copyright (C) 2018 Matthew K Wilson
//This file is licensed under the MIT open source license (https://opensource.org/licenses/MIT)
//Modified, optimized and implemented in Terrain Engine 2D
//References (Refer to license.txt for all Third Party Licenses):
// https://w-shadow.com/blog/2009/09/01/simple-fluid-simulation/
// http://www.jgallant.com/2d-liquid-simulator-with-cellular-automaton-in-unity/

namespace TerrainEngine2D
{
    [DisallowMultipleComponent]
    /// <summary>
    /// Fluid physics system
    /// </summary>
    public class FluidDynamics : MonoBehaviourSingleton<FluidDynamics>
    {
        private ChunkLoader chunkLoader;
        private FluidRenderer fluidRenderer;
        private WorldData worldData;
        private bool topDown;
        /// <summary>
        /// Whether the game has a top-down camera style for controlling fluid flow
        /// </summary>
        public bool TopDown
        {
            get { return topDown; }
        }
        //Whether to update the fluid simulatiom
        private bool runSimulation;
        //The rate at which the fluid simulation will update (in seconds)
        private float fluidUpdateRate;
  
        private float maxWeight = 1.0f;
        /// <summary>
        /// Maximum amount of liquid a fluid block can hold
        /// </summary>
        public float MaxWeight
        {
            get { return maxWeight; }
        }
        private float minWeight = 0.005f;
        /// <summary>
        /// Minimum amount of liquid a fluid block can hold
        /// </summary>
        public float MinWeight
        {
            get { return minWeight; }
        }
        private float stableAmount = 0.0001f;
        public float StableAmount
        {
            get { return stableAmount; }
        }
        private float pressureWeight = 0.2f;
        /// <summary>
        /// Fluid weight pressure factor (each fluid block can hold pressureWeight more liquid than the block above it)
        /// </summary>
        public float PressureWeight
        {
            get { return pressureWeight; }
        }
        private float fluidDropAmount = 0.2f;
        /// <summary>
        /// Amount of fluid added on drop
        /// </summary>
        public float FluidDropAmount
        {
            get { return fluidDropAmount; }
        }
        private Color32 mainColor = new Color(0.176f, 0.431f, 0.557f, 0.8f);
        /// <summary>
        /// Main color of the fluid
        /// </summary>
        public Color32 MainColor
        {
            get { return mainColor; }
        }
        private Color32 secondaryColor = new Color(0.275f, 0.686f, 0.894f, 0.8f);
        /// <summary>
        /// Secondary color of the fluid (used for lower pressure blocks)
        /// </summary>
        public Color32 SecondaryColor
        {
            get { return secondaryColor; }
        }

        //Update the fluid blocks
        private bool updateFluid;

        private FluidBlock[,] fluidBlocks;
        /// <summary>
        /// The array of fluid blocks for the whole world
        /// </summary>
        public FluidBlock[,] FluidBlocks
        {
            get { return fluidBlocks; }
            set { fluidBlocks = value; }
        }
        //The difference in fluid weight in the current iteration
        private float[,] fluidDifference;
        //Timer for fluid update
        private float updateTimer;
        //Whether fluid is being rendered as a texture
        private bool renderFluidTexture;

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        /// <summary>
        /// Initialize the fluid data
        /// </summary>
        /// <param name="world">Reference to the world</param>
        public void Initialize()
        {
            Instance = this;

            worldData = World.WorldData;
            //Allocates memory for the fluid blocks
            fluidBlocks = new FluidBlock[worldData.WorldWidth, worldData.WorldHeight];
            for (int x = 0; x < worldData.WorldWidth; x++)
            {
                for (int y = 0; y < worldData.WorldHeight; y++)
                {
                    fluidBlocks[x, y] = new FluidBlock();
                }
            }
            fluidDifference = new float[worldData.WorldWidth, worldData.WorldHeight];
            //Sets the adjacent blocks for each fluid block
            for (int x = 0; x < worldData.WorldWidth; x++)
            {
                for (int y = 0; y < worldData.WorldHeight; y++)
                {
                    //Sets adjacent blocks that are within the world bounds
                    fluidBlocks[x, y].TopBlock = GetFluidBlock(x, y + 1);
                    fluidBlocks[x, y].BottomBlock = GetFluidBlock(x, y - 1);
                    fluidBlocks[x, y].LeftBlock = GetFluidBlock(x - 1, y);
                    fluidBlocks[x, y].RightBlock = GetFluidBlock(x + 1, y);
                }
            }
        }

        private void OnEnable()
        {
            //Update the fluid simulation when chunks are loaded
            ChunkLoader.OnChunksLoaded += UpdateFluid;
        }

        private void OnDisable()
        {
            ChunkLoader.OnChunksLoaded -= UpdateFluid;
        }

        private void Start()
        {
            fluidRenderer = FluidRenderer.Instance;
            chunkLoader = ChunkLoader.Instance;
            renderFluidTexture = worldData.RenderFluidAsTexture;

            //Set Properties
            UpdateProperties(World.WorldData);
        }

        public void UpdateProperties(WorldData worldData)
        {
            topDown = worldData.TopDown;
            runSimulation = worldData.RunFluidSimulation;
            fluidUpdateRate = worldData.FluidUpdateRate;
            maxWeight = worldData.MaxFluidWeight;
            minWeight = worldData.MinFluidWeight;
            stableAmount = worldData.StableFluidAmount;
            pressureWeight = worldData.FluidPressureWeight;
            fluidDropAmount = worldData.FluidDropAmount;

            mainColor = worldData.MainFluidColor;
            secondaryColor = worldData.SecondaryFluidColor;
        }

        /// <summary>
        /// Sets the fluid for updating
        /// </summary>
        public void UpdateFluid()
        {
            updateFluid = true;
        }

        private void FixedUpdate()
        {
            //Checks if the simulation is running
            if (runSimulation) {
                //Checks if it is time to update the fluid
                if (updateTimer >= fluidUpdateRate)
                {
                    //Updates the fluid if it needs updating
                    if (updateFluid)
                    {
                        SimulateFluid(chunkLoader.OriginLoadedChunks, chunkLoader.EndPointLoadedChunks + new Vector2Int(chunkLoader.ChunkSize, chunkLoader.ChunkSize));
                        updateTimer = 0;
                    }
                } else
                    updateTimer += Time.deltaTime;
            }
        }

        /// <summary>
        /// Updates the fluid blocks and runs the fluid simulation
        /// </summary>
        /// <param name="startPosition">The starting position of the loaded world</param>
        /// <param name="endPosition">The end position of the loaded world</param>
        void SimulateFluid(Vector2Int startPosition, Vector2Int endPosition)
        {
            updateFluid = false;
            //Sets the coordinates for looping through the fluid blocks
            //Loops through all the loaded fluid blocks
            for (int x = startPosition.x; x < endPosition.x; x++)
            {
                for (int y = startPosition.y; y < endPosition.y; y++)
                {
                    //Gets the current fluid block
                    FluidBlock fluidBlock = fluidBlocks[x, y];
                    //Skips the block if it has settled or it has less liquid than the minimum
                    if (fluidBlock.Weight > minWeight && !fluidBlock.Stable)
                    {
                        //Calculate the liquid flow
                        if (topDown)
                            TopDownFlow(x, y, fluidBlock);
                        else
                            DownFlow(x, y, fluidBlock);
                    }
                }
            }
            //Second loop for setting values
            for (int x = startPosition.x; x < endPosition.x; x++)
            {
                for (int y = startPosition.y; y < endPosition.y; y++)
                {
                    FluidBlock fluidBlock = fluidBlocks[x, y];
                    //Skips the current block if it is solid
                    if (fluidBlock.Weight == FluidBlock.SolidWeight)
                        continue;
                    //Applies the change to the fluid weight
                    fluidBlock.Weight += fluidDifference[x, y];
                    //Updates the chunk if there is substancial difference in the block's fluid
                    if (fluidDifference[x, y] > stableAmount || fluidDifference[x, y] < -stableAmount)
                    {
                        if(renderFluidTexture)
                            fluidRenderer.UpdateTexture();
                        else
                            chunkLoader.UpdateChunk(x, y, true);
                    }

                    float weight = fluidBlock.Weight;
                    //Empty the block if it has lower than the minimum amount of fluid and it is not already empty or solid
                    if (weight < minWeight && weight > 0)
                    {
                        if (!topDown)
                            fluidBlock.SetEmpty();
  
                        //Unsettle the block and adjacent blocks if it was just emptied
                    }
                    else if (weight == 0 && fluidDifference[x, y] != 0)
                    {
                        fluidBlock.UnsettleNeighbours();
                        fluidBlock.Stable = false;
                    }
                    //Resets the fluid difference
                    fluidDifference[x, y] = 0;
                    //If the fluid block is not stable and still has liquid continue to run the simulation
                    if (!fluidBlock.Stable && weight > minWeight)
                        updateFluid = true;
                }
            }
            
        }

        /// <summary>
        /// Calculates the movement of fluid in a fluid block for a top-down simulation
        /// </summary>
        /// <param name="x">X coordinate of the fluid block</param>
        /// <param name="y">Y coordinate of the fluid block</param>
        /// <param name="fluidBlock">Reference to the fluid block</param>
        void TopDownFlow(int x, int y, FluidBlock fluidBlock)
        {
            //Reset starting values
            float flowAmount = 0;
            float startAmount = fluidBlock.Weight;
            float remainingAmount = startAmount;

            //If there is a block below it that is not solid flow down
            if (fluidBlock.BottomBlock != null && fluidBlock.BottomBlock.Weight != FluidBlock.SolidWeight)
            {
                //------Calculate the amount of fluid to flow down-----
                //Move one quarter of the difference between the two blocks to the block with less fluid
                flowAmount = (remainingAmount - fluidBlock.BottomBlock.Weight) / 4f;

                //Ensure there isn't more fluid flowing than the block started with
                if (flowAmount < 0)
                    flowAmount = 0;
                if (flowAmount > remainingAmount)
                    flowAmount = remainingAmount;
                //If fluid is flowing down set the temporary values
                if (flowAmount > 0)
                {
                    //Remove fluid from the fluid block
                    remainingAmount -= flowAmount;
                    fluidDifference[x, y] -= flowAmount;
                    //Add fluid to the block below it
                    fluidDifference[x, y - 1] += flowAmount;
                    fluidBlock.BottomBlock.Stable = false;

                    //Stop flowing if there is not enough fluid remaining
                    if (remainingAmount < minWeight)
                        return;
                }
            }

            //If there is a block to the right that is not solid flow right
            if (fluidBlock.RightBlock != null && fluidBlock.RightBlock.Weight != FluidBlock.SolidWeight)
            {
                //Calculate the amount of fluid to flow horizontally between the blocks
                //Move one quarter of the difference between the two blocks to the block with less fluid
                flowAmount = (remainingAmount - fluidBlock.RightBlock.Weight) / 4f;
                //Ensure there isn't more fluid flowing than the block started with
                if (flowAmount < 0)
                    flowAmount = 0;
                if (flowAmount > remainingAmount)
                    flowAmount = remainingAmount;
                //If fluid is flowing down set the temporary values
                if (flowAmount > 0)
                {
                    //Remove fluid from the fluid block
                    remainingAmount -= flowAmount;
                    fluidDifference[x, y] -= flowAmount;
                    //Add fluid to the block to the right
                    fluidDifference[x + 1, y] += flowAmount;
                    if (flowAmount > stableAmount)
                        fluidBlock.RightBlock.Stable = false;

                    //Stop flowing if there is not enough fluid remaining
                    if (remainingAmount < minWeight)
                        return;
                }
            }

            //Flow left
            if (fluidBlock.LeftBlock != null && fluidBlock.LeftBlock.Weight != FluidBlock.SolidWeight)
            {
                flowAmount = (remainingAmount - fluidBlock.LeftBlock.Weight) / 4f;

                if (flowAmount < 0)
                    flowAmount = 0;
                if (flowAmount > remainingAmount)
                    flowAmount = remainingAmount;

                if (flowAmount > 0)
                {
                    remainingAmount -= flowAmount;
                    fluidDifference[x, y] -= flowAmount;
                    fluidDifference[x - 1, y] += flowAmount;
                    if (flowAmount > stableAmount)
                        fluidBlock.LeftBlock.Stable = false;
                    if (remainingAmount < minWeight)
                        return;
                }
            }

            //Flow up
            if (fluidBlock.TopBlock != null && fluidBlock.TopBlock.Weight != FluidBlock.SolidWeight)
            {
                flowAmount = (remainingAmount - fluidBlock.TopBlock.Weight) / 4f;

                if (flowAmount < 0)
                    flowAmount = 0;
                if (flowAmount > remainingAmount)
                    flowAmount = remainingAmount;

                if (flowAmount > 0)
                {
                    remainingAmount -= flowAmount;
                    fluidDifference[x, y] -= flowAmount;
                    fluidDifference[x, y + 1] += flowAmount;
                    fluidBlock.TopBlock.Stable = false;
                    if (remainingAmount < minWeight)
                        return;
                }
            }
            //Calculate the difference in the liquid amount after flowing
            float difference = startAmount - remainingAmount;
            //If the difference is negligible, set the block to stable
            if (difference < stableAmount && difference > -stableAmount)
                fluidBlock.Stable = true;
            //If there is a large difference unsettle the adjacent blocks
            else
                fluidBlock.UnsettleNeighbours();
        }

        /// <summary>
        /// Calculates the movement of fluid in a fluid block for a down simulation
        /// </summary>
        /// <param name="x">X coordinate of the fluid block</param>
        /// <param name="y">Y coordinate of the fluid block</param>
        /// <param name="fluidBlock">Reference to the fluid block</param>
        void DownFlow(int x, int y, FluidBlock fluidBlock)
        {
            //Reset starting values
            float flowAmount = 0;
            float startAmount = fluidBlock.Weight;
            float remainingAmount = startAmount;
            
            //If there is a block below it that is not solid flow down
            if (fluidBlock.BottomBlock != null && fluidBlock.BottomBlock.Weight != FluidBlock.SolidWeight)
            {
                //------Calculate the amount of fluid to flow down-----
                //Get the total amount of fluid
                float combinedAmount = startAmount + fluidBlock.BottomBlock.Weight;
                //The total amount is less than the max amount of fluid of a single block
                if (combinedAmount <= maxWeight)
                    //The lower block gets all the fluid
                    flowAmount = startAmount;
                //Both blocks are not fully pressurized with fluid
                else if (combinedAmount < 2 * maxWeight + pressureWeight)
                    //The lower block is filled and compressed by a factor of fluid in the top block
                    flowAmount = (combinedAmount * pressureWeight + maxWeight * maxWeight) / (maxWeight + pressureWeight) - fluidBlock.BottomBlock.Weight;
                //Both blocks are full and pressurized
                else
                    //Lower block is filled with max pressure
                    flowAmount = (combinedAmount + pressureWeight) / 2f - fluidBlock.BottomBlock.Weight;
                //Value equal to or between maxWeight and maxWeight + pressureWeight

                //Ensure there isn't more fluid flowing than the block started with
                if (flowAmount < 0)
                    flowAmount = 0;
                if (flowAmount > startAmount)
                    flowAmount = startAmount;

                //If fluid is flowing down set the temporary values
                if (flowAmount > 0)
                {
                    //Remove fluid from the fluid block
                    remainingAmount -= flowAmount;
                    fluidDifference[x, y] -= flowAmount;
                    //Add fluid to the block below it
                    fluidDifference[x, y - 1] += flowAmount;
                    fluidBlock.BottomBlock.Stable = false;

                    //Stop flowing if there is not enough fluid remaining
                    if (remainingAmount < minWeight)
                        return;
                }
            }

            //If there is a block to the right that is not solid flow right
            if (fluidBlock.RightBlock != null && fluidBlock.RightBlock.Weight != FluidBlock.SolidWeight)
            {
                //Calculate the amount of fluid to flow horizontally between the blocks
                //Move one quarter of the difference between the two blocks to the block with less fluid
                flowAmount = (remainingAmount - fluidBlock.RightBlock.Weight) / 4f;
                //Ensure there isn't more fluid flowing than the block started with
                if (flowAmount < 0)
                    flowAmount = 0;
                if (flowAmount > remainingAmount)
                    flowAmount = remainingAmount;
                //If fluid is flowing down set the temporary values
                if (flowAmount > 0)
                {
                    //Remove fluid from the fluid block
                    remainingAmount -= flowAmount;
                    fluidDifference[x, y] -= flowAmount;
                    //Add fluid to the block to the right
                    fluidDifference[x + 1, y] += flowAmount;
                    if (flowAmount > stableAmount)
                        fluidBlock.RightBlock.Stable = false;

                    //Stop flowing if there is not enough fluid remaining
                    if (remainingAmount < minWeight)
                        return;
                }
            }

            //Flow left
            if (fluidBlock.LeftBlock != null && fluidBlock.LeftBlock.Weight != FluidBlock.SolidWeight)
            {
                flowAmount = (remainingAmount - fluidBlock.LeftBlock.Weight) / 4f;
                if (flowAmount < 0)
                    flowAmount = 0;
                if (flowAmount > remainingAmount)
                    flowAmount = remainingAmount;

                if (flowAmount > 0)
                {
                    remainingAmount -= flowAmount;
                    fluidDifference[x, y] -= flowAmount;
                    fluidDifference[x - 1, y] += flowAmount;
                    if (flowAmount > stableAmount)
                        fluidBlock.LeftBlock.Stable = false;
                    if (remainingAmount < minWeight)
                        return;
                }
            }

            //Flow up
            if (fluidBlock.TopBlock != null && fluidBlock.TopBlock.Weight != FluidBlock.SolidWeight)
            {
                //Get the total amount of fluid
                float combinedAmount = remainingAmount + fluidBlock.TopBlock.Weight;
                //The total amount is less than the max amount of fluid of a single block
                if (combinedAmount <= maxWeight)
                    //The lower block gets all the fluid
                    flowAmount = 0;
                //Both blocks are not fully pressurized with fluid
                else if (combinedAmount < 2 * maxWeight + pressureWeight)
                {
                    //The lower block is filled and compressed by a factor of fluid in the top block
                    flowAmount = remainingAmount - (combinedAmount * pressureWeight + maxWeight * maxWeight) / (maxWeight + pressureWeight);
                    //Both blocks are full and pressurized
                }
                else
                    //Lower block is filled with max pressure, the rest flows up
                    flowAmount = remainingAmount - (combinedAmount + pressureWeight) / 2f;
                //Returns a value equal to or between maxWeight and maxWeight + pressureWeight

                if (flowAmount < 0)
                    flowAmount = 0;
                if (flowAmount > remainingAmount)
                    flowAmount = remainingAmount;

                if (flowAmount > 0)
                {
                    remainingAmount -= flowAmount;
                    fluidDifference[x, y] -= flowAmount;
                    fluidDifference[x, y + 1] += flowAmount;
                    fluidBlock.TopBlock.Stable = false;
                    if (remainingAmount < minWeight)
                        return;
                }
            }
            //Calculate the difference in the liquid amount after flowing
            float difference = startAmount - remainingAmount;
            //If the difference is negligible, set the block to stable
            if (difference < stableAmount && difference > -stableAmount)
                fluidBlock.Stable = true;
            //If there is a large difference unsettle the adjacent blocks
            else
                fluidBlock.UnsettleNeighbours();
        }

        /// <summary>
        /// Get the fluid block at a specific coordinate
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Returns the fluid block</returns>
        public FluidBlock GetFluidBlock(int x, int y)
        {
            if (x < 0 || x >= worldData.WorldWidth || y < 0 || y >= worldData.WorldHeight)
                return null;
            return fluidBlocks[x, y];
        }
    }
}
