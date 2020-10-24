using UnityEngine;

// Copyright (C) 2018 Matthew K Wilson

namespace TerrainEngine2D.Lighting
{
    /// <summary>
    /// 2D Ambient Lighting
    /// </summary>
    [RequireComponent(typeof(BlockLighting))]
    public class AmbientLight : BlockLightMesh
    {
        private static AmbientLight instance;
        /// <summary>
        /// A singleton instance of the Ambient Lighting
        /// </summary>
        public static AmbientLight Instance
        {
            get { return instance; }
        }
        //Whether the light data was initialized 
        public bool Initialized { get; set; }
        [HideInInspector]
        /// <summary>
        /// The color of the ambient light in the daytime
        /// Changes the color of the Main Camera
        /// </summary>
        public Color DaylightColor = Color.white;
        [HideInInspector]
        /// <summary>
        /// The color of the ambient lighting in the nighttime
        /// Changes the color of the Main Camera
        /// </summary>
        public Color NightColor;
        //The colors used to set the ambient lighting
        [HideInInspector]
        /// <summary>
        /// The time the sun will rise, used for setting the color of the ambient lighting material (default 7)
        /// </summary>
        public float SunriseTime = 7;
        [HideInInspector]
        /// <summary>
        /// The time the sun will set, used for settinge the color of the ambient lighting material (default 19)
        /// </summary>
        public float SunsetTime = 19;

        private bool useHeightMap;
        /// <summary>
        /// Whether to use the heightmap to calulate an ambient light value
        /// </summary>
        public bool UseHeightMap
        {
            get { return useHeightMap; }
        }

        //Heightmap used for generating ambient light
        private int[] heightMap;

        private Color32 whiteColor32 = new Color32(255, 255, 255, 255);

#if UNITY_EDITOR
        public void EditorSetup()
        {
            blockLighting = GetComponent<BlockLighting>();
        }
#endif

        protected override void Awake()
        {
            base.Awake();

            if (instance == null)
                instance = this;
            else if (instance != this)
            {
                Debug.Log("Destroying extra instance of " + gameObject.name);
                Destroy(gameObject);
            }

            //Set Properties
            blockLighting.LightSpread = World.WorldData.AmbientLightSpread;
            blockLighting.LightTransmission = World.WorldData.AmbientLightTransmission;
            useHeightMap = World.WorldData.UseHeightMap;
        }

        protected override void Start()
        {
            base.Start();
            SetAmbientLightColor();
        }

        /// <summary>
        /// Initialize the ambient lighting
        /// </summary>
        public void Initialize()
        {
            if (Initialized)
                return;

            if (world == null)
                world = World.Instance;

            blockLighting.Initialize();

            //Calculate the Height Map data by finding the position of the surface blocks 
            if (useHeightMap)
            {
                heightMap = new int[world.WorldWidth];
                for (int x = 0; x < world.WorldWidth; x++)
                {
                    int y = world.WorldHeight - 1;
                    while (!world.GetBlockLayer(world.AmbientLightLayer).IsBlockAt(x, y) && y > 0)
                    {
                        //Place a light source at every position above the terrain surface to imitate sunlight
                        blockLighting.AddLightSourceBulk(new Vector2Int(x, y), whiteColor32);
                        y--;
                    }
                    heightMap[x] = y;
                }
            } else
            {
                for (int x = 0; x < world.WorldWidth; x++)
                {
                    for (int y = 0; y < world.WorldHeight; y++)
                    {
                        if (!world.GetBlockLayer(world.AmbientLightLayer).IsBlockAt(x, y))
                        {
                            //Place a light source wherever there isn't an ambient light block to imitate sunlight
                            blockLighting.AddLightSourceBulk(new Vector2Int(x, y), whiteColor32);
                        }
                    }
                }
            }
            blockLighting.ManualGenerateLighting();
            Initialized = true;
        }

        private void Update()
        {
            if (world.PauseTime)
                return;

            SetAmbientLightColor();
        }

        private void SetAmbientLightColor()
        {
            float time = world.TimeOfDay;
            //Set the ambient light color based on the time of day
            if (time < SunriseTime || time >= SunsetTime + 1)
                Camera.main.backgroundColor = NightColor;
            else if (time >= SunriseTime + 1 && time < SunsetTime)
                Camera.main.backgroundColor = DaylightColor;
            else if (time >= SunriseTime && time < SunriseTime + 1)
                Camera.main.backgroundColor = Color.Lerp(NightColor, DaylightColor, (time - SunriseTime) / 1f);
            else if (time >= SunsetTime && time < SunsetTime + 1)
                Camera.main.backgroundColor = Color.Lerp(DaylightColor, NightColor, (time - SunsetTime) / 1f);

            if (time < SunriseTime || time >= SunsetTime + 1)
                material.SetFloat(Shader.PropertyToID("_Alpha"), NightColor.a);
            else if (time >= SunriseTime + 1 && time < SunsetTime)
                material.SetFloat(Shader.PropertyToID("_Alpha"), DaylightColor.a);
            else if (time >= SunriseTime && time < SunriseTime + 1)
                material.SetFloat(Shader.PropertyToID("_Alpha"), Mathf.Lerp(NightColor.a, DaylightColor.a, (time - SunriseTime) / 1f));
            else if (time >= SunsetTime && time < SunsetTime + 1)
                material.SetFloat(Shader.PropertyToID("_Alpha"), Mathf.Lerp(DaylightColor.a, NightColor.a, (time - SunsetTime) / 1f));
        }

        /// <summary>
        /// Update the ambient lighting
        /// </summary>
        /// <param name="posX">The x position of the block change</param>
        /// <param name="posY">The y position of the block change</param>
        /// <param name="width">The width of the block (in block units)</param>
        /// <param name="height">The height of the block (in block units)</param>
        /// <param name="blockAdded">Whether a block was added or removed</param>
        public void UpdateAmbientLighting(int posX, int posY, int width, int height, bool blockAdded)
        {
            if (useHeightMap)
            {
                //Update the height map when terrain is modified
                for (int x = posX; x < posX + width; x++)
                {
                    if (blockAdded)
                    {
                        //If a block is added above the surface of the terrain then calculate the new height and remove any ambient light sources that are now below the terrains surface
                        int blockTop = posY + height - 1;
                        if (heightMap[x] < blockTop)
                        {
                            int y = heightMap[x] + 1;
                            blockLighting.RemoveLightSources(new Vector2Int(x, y), 1, posY - heightMap[x]);
                            heightMap[x] = blockTop;
                        }
                    }
                    else
                    {
                        //If a surface block was removed then calculate the new height of the surface block and 
                        //add light sources to all the blocks above the new surface block that were previously blocked off
                        if (heightMap[x] == posY + height - 1)
                        {
                            int newHeightMapY = heightMap[x];
                            while (!world.GetBlockLayer(world.AmbientLightLayer).IsBlockAt(x, newHeightMapY) && newHeightMapY > 0)
                            {
                                blockLighting.AddLightSource(new Vector2Int(x, newHeightMapY), whiteColor32);
                                newHeightMapY--;
                            }
                            heightMap[x] = newHeightMapY;
                        }
                    }
                }
            }
            else
            {
                if (blockAdded)
                    //If a block was added, then remove the light sources in the grid positions that the block took up
                    for (int x = posX; x < posX + width; x++)
                    {
                        for (int y = posY; y < posY + height; y++)
                        {
                            blockLighting.RemoveLightSource(new Vector2Int(x, y));
                        }
                    }
                else
                {
                    //If a block was removed, then add a light source in each grid position the block took up
                    for (int x = posX; x < posX + width; x++)
                    {
                        for (int y = posY; y < posY + height; y++)
                        {
                            blockLighting.AddLightSource(new Vector2Int(x, y), whiteColor32);
                        }
                    }
                }
            }
        }
    }
}