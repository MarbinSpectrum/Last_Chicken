using System.Collections.Generic;
using UnityEngine;

// Copyright (C) 2018 Matthew K Wilson

namespace TerrainEngine2D.Lighting
{
    /// <summary>
    /// A 2d block lighting system
    /// This lighting system features light propogation, multi-colored lights, and a highly optimized light calculation algorithm
    /// </summary>
    public class BlockLighting : MonoBehaviour
    {
        private World world;
        [SerializeField]
        [Tooltip("The amount of blocks a light of full intensity will transmit through")]
        private int lightTransmission = 5;
        /// <summary>
        /// The amount of blocks a light of full intensity will transmit through
        /// </summary>
        public int LightTransmission
        {
            get { return lightTransmission; }
            set { lightTransmission = value; }
        }
        [SerializeField]
        [Tooltip("The number of air blocks a light of full intensity will spread over")]
        private int lightSpread = 18;
        /// <summary>
        /// The number of air blocks a light of full intensity will spread over
        /// </summary>
        public int LightSpread
        {
            get { return lightSpread; }
            set { lightSpread = value; }
        }

        private byte spreadIntensityDrop;
        /// <summary>
        /// The amount light intensity will drop as light spreads from block to block
        /// </summary>
        public byte SpreadIntensityDrop
        {
            get { return spreadIntensityDrop; }
        }
        
        private byte transmissionIntensityDrop;
        /// <summary>
        /// The amount light intensity will drop as light is transmitted through a terrain block
        /// </summary>
        public byte TransmissionIntensityDrop
        {
            get { return transmissionIntensityDrop; }
        }

        private Color32[,] lightMap;
        /// <summary>
        /// Light data for the whole world
        /// </summary>
        public Color32[,] LightMap
        {
            get { return lightMap; }
        }

        //Collection of light sources
        private Dictionary<Vector2Int, Color32> lightSources;
        private bool[,] lightSourceMap;

        //Collections of light positions and the light color at those positions used for calculating the light map
        //Light nodes used for adding light to the map
        private Queue<BlockLightNode> lightNodes;
        //Light nodes used for removing light from the map
        private Queue<BlockLightNode> clearNodes;
        //Light nodes used to update light on the map
        private HashSet<BlockLightNode> updateNodes;

        //Completely transparent Color rgba(0,0,0,0)
        private Color32 ClearColor32 = Color.clear;

        //Whether the block lighting has been initialized yet
        private bool initialized;


        /// <summary>
        /// Actions invoked by the Block Lighting System
        /// </summary>
        public delegate void BlockLightingEvent();
        /// <summary>
        /// Event called when lighting has been generated
        /// </summary>
        public event BlockLightingEvent OnLightGenerated;

        private void OnEnable()
        {
            World.OnBlockPlaced += BlockPlaced;
            World.OnBlockRemoved += BlockRemoved;
        }

        private void OnDisable()
        {
            World.OnBlockPlaced -= BlockPlaced;
            World.OnBlockRemoved -= BlockRemoved;
        }

        private void Start()
        {
            //Ensure the block lighting has been initialized
            if (!initialized)
                Initialize();
        }

        /// <summary>
        /// Initialize the block lighting
        /// </summary>
        public void Initialize()
        {
            if (world == null)
                world = World.Instance;

            lightSources = new Dictionary<Vector2Int, Color32>();
            lightSourceMap = new bool[world.WorldWidth, world.WorldHeight];

            lightNodes = new Queue<BlockLightNode>();
            clearNodes = new Queue<BlockLightNode>();
            updateNodes = new HashSet<BlockLightNode>();

            //Initialize the light map for the world
            lightMap = new Color32[world.WorldWidth, world.WorldHeight];

            //Calculate the color intensity drop values
            spreadIntensityDrop = (byte)(byte.MaxValue / (lightSpread + 1));
            transmissionIntensityDrop = (byte)(byte.MaxValue / (lightTransmission + 1));
            transmissionIntensityDrop = (byte)Mathf.Max(0, transmissionIntensityDrop - spreadIntensityDrop);

            initialized = true;
        }

        /// <summary>
        /// Generate the block lighting
        /// </summary>
        private void GenerateLighting()
        {
            //Loop through all the clear nodes to clear any lighting as a result of any removed light sources or added/removed terrain blocks
            while (clearNodes.Count > 0)
                ClearLight(clearNodes.Dequeue());

            //Update any light nodes added from the clearing
            //This is to ensure any lighting that was previously cleared will be recalculated by the nearby light sources and nodes
            foreach (BlockLightNode node in updateNodes)
            {
                //Drop the lights intensity and spread the light to all the neighbouring nodes
                Color32 color = node.Color;
                byte intensityDrop = GetIntensityDrop(node.x, node.y);
                color.r = (byte)Mathf.Clamp(node.Color.r - intensityDrop, 0, 255);
                color.g = (byte)Mathf.Clamp(node.Color.g - intensityDrop, 0, 255);
                color.b = (byte)Mathf.Clamp(node.Color.b - intensityDrop, 0, 255);
                color.a = (byte)Mathf.Clamp(node.Color.a - intensityDrop, 0, 255);

                BlockLightNode lightNode = new BlockLightNode(color, node.x, node.y, node.OriginX, node.OriginY);
                SpreadLight(lightNode, lightNodes);
            }
            updateNodes.Clear();

            //Loop through all the light nodes to propagate any lighting as a result of added light sources, or nodes that may need to be updated
            while (lightNodes.Count > 0)
                PropagateLight(lightNodes.Dequeue());

            //Run the light generated event
            if(OnLightGenerated != null)
                OnLightGenerated();
        }

        /// <summary>
        /// Propagate the lighting from a light node
        /// Each color channel is handled seperately
        /// Light is only transferred if the value of the node is greater than the current value at that position
        /// </summary>
        /// <param name="lightNode">The light node to propagate</param>
        private void PropagateLight(BlockLightNode lightNode)
        {
            Color32 nodeColor = lightNode.Color;
            Color32 currentColor = lightMap[lightNode.x, lightNode.y];
            bool lightTransferred = false;
            byte intensityDrop = GetIntensityDrop(lightNode.x, lightNode.y);

            //Red Channel
            if (nodeColor.r > currentColor.r)
            {
                lightMap[lightNode.x, lightNode.y].r = nodeColor.r;
                nodeColor.r = (byte)Mathf.Clamp(nodeColor.r - intensityDrop, 0, 255);
                lightTransferred = true;
            }
            //Green Channel
            if (nodeColor.g > currentColor.g)
            {
                lightMap[lightNode.x, lightNode.y].g = nodeColor.g;
                nodeColor.g = (byte)Mathf.Clamp(nodeColor.g - intensityDrop, 0, 255);
                lightTransferred = true;
            }
            //Blue Channel
            if (nodeColor.b > currentColor.b)
            {
                lightMap[lightNode.x, lightNode.y].b = nodeColor.b;
                nodeColor.b = (byte)Mathf.Clamp(nodeColor.b - intensityDrop, 0, 255);
                lightTransferred = true;
            }
            //Alpha Channel
            if(nodeColor.a > currentColor.a)
            {
                lightMap[lightNode.x, lightNode.y].a = nodeColor.a;
                nodeColor.a = (byte)Mathf.Clamp(nodeColor.a - intensityDrop, 0, 255);
            }

            //If light has been transferred across any channel then spread this light to its neighbours
            if (lightTransferred)
            {
                lightNode.Color = nodeColor;
                SpreadLight(lightNode, lightNodes);
            }
        }

        /// <summary>
        /// Clear any light using the given node
        /// All lighting will be cleared outwardly from the clearNode as long as the lighting is of equal or lesser intensity 
        /// than the clearNode. Other light sources will be skipped over. After all has cleared
        /// </summary>
        /// <param name="clearNode">The node used to clear the lighting</param>
        private void ClearLight(BlockLightNode clearNode)
        {
            Color32 nodeColor = clearNode.Color;
            Color32 currentColor = lightMap[clearNode.x, clearNode.y];
            bool clearLight = false;

            //Don't clear any light sources unless the light at the given node is being removed
            if (!clearNode.LightSource && lightSourceMap[clearNode.x, clearNode.y])
            {
                //Add this light source as a node to be updated so that it will spread its light back over any previously
                //cleared block positions
                BlockLightNode lightSourceNode = new BlockLightNode(currentColor, clearNode.x, clearNode.y, clearNode.OriginX, clearNode.OriginY);
                updateNodes.Add(lightSourceNode);
                return;
            }

            //Finish clearing the lighting if the color of the current node is already clear or the color of the node is clear
            //If the nodeColor is clear this means that the lighting has been cleared
            if (currentColor.Equals(ClearColor32) || nodeColor.Equals(ClearColor32))
                return;

            //Red Channel
            if (nodeColor.r >= currentColor.r && nodeColor.r != 0)
            {
                nodeColor.r = currentColor.r;
                clearLight = true;
            }
            //Green Channel
            if (nodeColor.g >= currentColor.g && nodeColor.g != 0)
            {
                nodeColor.g = currentColor.g;
                clearLight = true;
            }
            //Blue Channel
            if (nodeColor.b >= currentColor.b && nodeColor.b != 0)
            {
                nodeColor.b = currentColor.b;
                clearLight = true;
            }

            if (clearLight)
            {
                //A block may be cleared that was previously added to the list to be updated. If this is the case it needs to be removed from the update list since 
                //it no longer has any light to spread.
                BlockLightNode currentNode = new BlockLightNode(currentColor, clearNode.x, clearNode.y, clearNode.OriginX, clearNode.OriginY);
                if (updateNodes.Contains(currentNode)) {
                    updateNodes.Remove(currentNode);
                }

                //Clear the light at the current position
                lightMap[clearNode.x, clearNode.y] = ClearColor32;

                //Spread the clearNode to continue clearing all connected lighting
                clearNode.Color = nodeColor;
                ClearLightSpread(clearNode, clearNodes);
                
            }
            else
            {
                //If no light was cleared this means that the current light has at least one channel of greater intensity than the clearNode. Therefore,
                //there must be another light source nearby, so the current light is added as a node to spread its light back over the previously cleared
                //blocks.
                BlockLightNode currentLightNode = new BlockLightNode(currentColor, clearNode.x, clearNode.y, clearNode.OriginX, clearNode.OriginY);
                if (!updateNodes.Contains(currentLightNode))
                    updateNodes.Add(currentLightNode);
            }
        }

        /// <summary>
        /// Clear the lighting from neighbouring nodes
        /// </summary>
        /// <param name="clearNode">The clear node</param>
        /// <param name="blockLightNodes">The light node queue to spread the lighting across</param>
        private void ClearLightSpread(BlockLightNode clearNode, Queue<BlockLightNode> blockLightNodes)
        {
            if (clearNode.x < world.WorldWidth - 1 && clearNode.x - clearNode.OriginX < lightSpread)
                blockLightNodes.Enqueue(new BlockLightNode(clearNode.Color, clearNode.x + 1, clearNode.y, clearNode.OriginX, clearNode.OriginY));
            if (clearNode.x > 0 && clearNode.OriginX - clearNode.x < lightSpread)
                blockLightNodes.Enqueue(new BlockLightNode(clearNode.Color, clearNode.x - 1, clearNode.y, clearNode.OriginX, clearNode.OriginY));
            if (clearNode.y < world.WorldHeight - 1 && clearNode.y - clearNode.OriginY < lightSpread)
                blockLightNodes.Enqueue(new BlockLightNode(clearNode.Color, clearNode.x, clearNode.y + 1, clearNode.OriginX, clearNode.OriginY));
            if (clearNode.y > 0 && clearNode.OriginY - clearNode.y < lightSpread)
                blockLightNodes.Enqueue(new BlockLightNode(clearNode.Color, clearNode.x, clearNode.y - 1, clearNode.OriginX, clearNode.OriginY));
        }


        /// <summary>
        /// Spread the lighting from a light node to all the neighbouring nodes
        /// </summary>
        /// <param name="lightNode">The light node</param>
        /// <param name="blockLightNodes">The light node queue to spread the lighting across</param>
        private void SpreadLight(BlockLightNode lightNode, Queue<BlockLightNode> blockLightNodes)
        {
            if (lightNode.x < world.WorldWidth - 1)
                blockLightNodes.Enqueue(new BlockLightNode(lightNode.Color, lightNode.x + 1, lightNode.y, lightNode.OriginX, lightNode.OriginY));
            if (lightNode.x > 0)
                blockLightNodes.Enqueue(new BlockLightNode(lightNode.Color, lightNode.x - 1, lightNode.y, lightNode.OriginX, lightNode.OriginY));
            if (lightNode.y < world.WorldHeight - 1)
                blockLightNodes.Enqueue(new BlockLightNode(lightNode.Color, lightNode.x, lightNode.y + 1, lightNode.OriginX, lightNode.OriginY));
            if (lightNode.y > 0)
                blockLightNodes.Enqueue(new BlockLightNode(lightNode.Color, lightNode.x, lightNode.y - 1, lightNode.OriginX, lightNode.OriginY));
        }

        /// <summary>
        /// Get the amount of light intensity to drop when transferring light from one block to another
        /// Light intensity will drop by a greater amount if it is transferring through a block as determined by the light layer
        /// </summary>
        /// <param name="x">The x coordinate of the position to get the intensity drop</param>
        /// <param name="y">The y coordinate of the position to get the intensity drop</param>
        /// <returns>Returns the amount of light intensity to drop in bytes</returns>
        private byte GetIntensityDrop(int x, int y)
        {
            //If there is a block in the light layer at the given coordinates than add the transmissionIntensityDrop to the total
            //This transmissionIntensityDrop value represents the amount of light blocked/absorbed by the terrain
            if (world.GetBlockLayer(world.LightLayer).IsBlockAt(x, y))
                return (byte)(spreadIntensityDrop + transmissionIntensityDrop);
            return spreadIntensityDrop;
        }

        /// <summary>
        /// Update the lighting at a certain position
        /// </summary>
        /// <param name="x">The x coordinate of the position</param>
        /// <param name="y">The y coordinate of the position</param>
        /// <param name="width">The width of the area to be updated</param>
        /// <param name="height">The height of the area to be updated</param>
        /// <param name="lightSource">Whether there should be a light source in this position</param>
        private void UpdateLight(int x, int y, int width = 1, int height = 1, bool lightSource = false)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    BlockLightNode lightNode = new BlockLightNode(lightMap[x + i, y + j], x + i, y + j, lightSource);
                    clearNodes.Enqueue(lightNode);
                }
            }
            GenerateLighting();
        }

        /// <summary>
        /// Manually Generate Lighting
        /// Use when lights are added in bulk
        /// </summary>
        public void ManualGenerateLighting()
        {
            GenerateLighting();
        }

        /// <summary>
        /// Add a light source
        /// </summary>
        ///<param name="color">The color of the light source</param>
        ///<param name="position">The position of the light source</param>
        /// <returns>Returns false if there is already a light in that key position</returns>
        public bool AddLightSource(Vector2Int position, Color32 color)
        {
            if (lightSourceMap == null || position.x < 0 || lightSourceMap.GetLength(0) <= position.x || position.y < 0 || lightSourceMap.GetLength(1) <= position.y)
                return false;
            if (lightSources.ContainsKey(position))
                return false;
            lightSources.Add(position, color);
            lightSourceMap[position.x, position.y] = true;
            BlockLightNode lightNode = new BlockLightNode(color, position);
            lightNodes.Enqueue(lightNode);
            GenerateLighting();
            return true;
        }

        /// <summary>
        /// Add a light source
        /// Make sure to call ManualLightGeneration after all light sources are added
        /// </summary>
        ///<param name="color">The color of the light source</param>
        ///<param name="position">The position of the light source</param>
        /// <returns>Returns false if there is already a light in that key position</returns>
        public bool AddLightSourceBulk(Vector2Int position, Color32 color)
        {
            if (lightSources.ContainsKey(position))
                return false;
            lightSources.Add(position, color);
            lightSourceMap[position.x, position.y] = true;
            BlockLightNode lightNode = new BlockLightNode(color, position);
            lightNodes.Enqueue(lightNode);
            return true;
        }

        /// <summary>
        /// Remove a light source from a given position
        /// </summary>
        /// <param name="position">The position to remove a light source</param>
        /// <returns>Returns true if a light was removed from that position</returns>
        public bool RemoveLightSource(Vector2Int position)
        {
            if (lightSources != null && lightSources.Remove(position))
            {
                lightSourceMap[position.x, position.y] = false;
                UpdateLight(position.x, position.y, 1, 1, true);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove an area of light sources
        /// </summary>
        /// <param name="position">The starting position to remove the light sources</param>
        /// <param name="width">The width of the area</param>
        /// <param name="height">The height of the area</param>
        /// <returns>Returns false if even one position does not have a light source</returns>
        public bool RemoveLightSources(Vector2Int position, int width, int height)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (lightSources.Remove(new Vector2Int(position.x + i, position.y + j)))
                        lightSourceMap[(int)position.x + i, (int)position.y + j] = false;
                    else
                    {
                        throw new System.Exception("No light source at position: " + (int)(position.x + i) + " " + (int)(position.y + j));
                    }
                }
            }
            UpdateLight((int)position.x, (int)position.y, width, height, true);
            return true;
        }

        /// <summary>
        /// Get the light source from a position
        /// </summary>
        /// <param name="position">The position to try and get a light source from</param>
        /// <param name="lightSource">The reference to a light source for output</param>
        /// <returns>Returns true if a light source is found at that position</returns>
        public bool GetLightSource(Vector2Int position, out Color32 lightSource)
        {
            if (lightSources.TryGetValue(position, out lightSource))
                return true;
            return false;
        }

        /// <summary>
        /// Check if there is a light source at the position
        /// </summary>
        /// <param name="position">The position to check for a light source</param>
        /// <returns>Returns true if a light source is found at that position</returns>
        public bool IsLightSource(Vector2Int position)
        {
            return lightSourceMap[position.x, position.y];
        }

        /// <summary>
        /// Clear the map of all lights
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < lightMap.GetLength(0); i++)
            {
                for (int j = 0; j < lightMap.GetLength(1); j++)
                {
                    lightMap[i, j] = ClearColor32;
                    lightSourceMap[i, j] = false;
                }
            }
            lightSources.Clear();
        }

        /// <summary>
        /// Update the lighting if a block is placed
        /// If there is a light source where the block is placed then remove it
        /// </summary>
        /// <param name="x">The x coordinate where the block was placed</param>
        /// <param name="y">The y coordinate where the block was placed</param>
        /// <param name="layer">The layer the block was placed in</param>
        /// <param name="blockType">The type of block placed</param>
        private void BlockPlaced(int x, int y, byte layer, byte blockType)
        {
            if (layer == world.LightLayer)
            {
                BlockInfo blockInfo = world.GetBlockLayer(layer).GetBlockInfo(blockType);
                if (blockInfo.MultiBlock)
                {
                    for (int i = 0; i < blockInfo.TextureWidth; i++)
                    {
                        for (int j = 0; j < blockInfo.TextureHeight; j++)
                        {
                            //If there is no block in the AmbientLightLayer there must be an ambient light source there (which should not be removed)
                            if (world.GetBlockLayer(world.AmbientLightLayer).IsBlockAt(x + i, y + j))
                            {
                                if (!RemoveLightSource(new Vector2Int(x + i, y + j)))
                                    UpdateLight(x + i, y + j);
                            } else
                                UpdateLight(x + i, y + j);
                        }
                    }
                }
                else
                {
                    if (world.GetBlockLayer(world.AmbientLightLayer).IsBlockAt(x, y))
                    {
                        if (!RemoveLightSource(new Vector2Int(x, y)))
                            UpdateLight(x, y);
                    } else
                        UpdateLight(x, y);
                }
            }
        }

        /// <summary>
        /// Update the block lighting if a block was removed 
        /// </summary>
        /// <param name="x">The x coordinate where the block was removed</param>
        /// <param name="y">The y coordinate where the block was removed</param>
        /// <param name="layer">The layer the block was removed from</param>
        /// <param name="blockType">The type of block removed</param>
        private void BlockRemoved(int x, int y, byte layer, byte blockType)
        {
            if (layer == world.LightLayer)
            {
                BlockInfo blockInfo = world.GetBlockLayer(layer).GetBlockInfo(blockType);
                if (blockInfo.MultiBlock)
                {
                    for (int i = 0; i < blockInfo.TextureWidth; i++)
                    {
                        for (int j = 0; j < blockInfo.TextureHeight; j++)
                        {
                            UpdateLight(x + i, y + j);
                        }
                    }
                }
                else
                    UpdateLight(x, y);
            }
        }
    }
}