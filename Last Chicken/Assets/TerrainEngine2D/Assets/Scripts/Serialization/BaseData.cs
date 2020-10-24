using System;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainEngine2D
{
    [Serializable]
    /// <summary>
    /// Block data to be saved to file
    /// </summary>
    public class BaseData
    {
        /// <summary>
        /// The name of the world
        /// </summary>
        public string Name;
        /// <summary>
        /// The width of the world in blocks
        /// </summary>
        public int Width;
        /// <summary>
        /// The height of the world in blocks
        /// </summary>
        public int Height;
        /// <summary>
        /// The seed used to generate the world
        /// </summary>
        public int Seed;

        /// <summary>
        /// Default constructor for BlockData
        /// Holds all the base data for saving
        /// </summary>
        public BaseData()
        {
            Name = World.WorldData.Name;
            Width = World.WorldData.WorldWidth;
            Height = World.WorldData.WorldHeight;
            Seed = World.WorldData.Seed;
        }
    }
}