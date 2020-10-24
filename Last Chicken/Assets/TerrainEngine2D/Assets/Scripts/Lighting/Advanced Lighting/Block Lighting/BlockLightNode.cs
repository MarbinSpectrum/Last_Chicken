using UnityEngine;

// Copyright (C) 2018 Matthew K Wilson

namespace TerrainEngine2D.Lighting
{
    /// <summary>
    /// A node of light
    /// </summary>
    public struct BlockLightNode
    {
        /// <summary>
        /// Whether this node has a recognized light source
        /// Used for removing light sources
        /// </summary>
        public bool LightSource;
        /// <summary>
        /// The color at the light node
        /// </summary>
        public Color32 Color;
        /// <summary>
        /// The x coordinate of the node
        /// </summary>
        public int x;
        /// <summary>
        /// The y coordinate of the node
        /// </summary>
        public int y;
        /// <summary>
        /// The original x position of the block light node (for iight spreading)
        /// </summary>
        public int OriginX;
        /// <summary>
        /// The original y position of the block light node (for iight spreading)
        /// </summary>
        public int OriginY;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="color">The color of the light node</param>
        /// <param name="position">The position of the node</param>
        /// <param name="lightSource">Whether there is a light source at this position</param>
        public BlockLightNode(Color32 color, Vector2Int position, bool lightSource = false)
        {
            Color = color;
            x = position.x;
            y = position.y;
            OriginX = x;
            OriginY = y;
            LightSource = lightSource;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="color">The color of the light node</param>
        /// <param name="x">The x coordinate position of the node</param>
        /// <param name="y">The y coordinate position of the node</param>
        /// <param name="lightSource">Whether there is a light source at this position</param>
        public BlockLightNode(Color32 color, int x, int y, bool lightSource = false)
        {
            Color = color;
            this.x = x;
            this.y = y;
            OriginX = x;
            OriginY = y;
            LightSource = lightSource;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="color">The color of the light node</param>
        /// <param name="x">The x coordinate position of the node</param>
        /// <param name="y">The y coordinate position of the node</param>
        /// <param name="OriginX">The x coordinate origin position of the node</param>
        /// <param name="OriginY">The y coordinate origin position of the node</param>
        /// <param name="lightSource">Whether there is a light source at this position</param>
        public BlockLightNode(Color32 color, int x, int y, int OriginX, int OriginY, bool lightSource = false)
        {
            Color = color;
            this.x = x;
            this.y = y;
            this.OriginX = OriginX;
            this.OriginY = OriginY;
            LightSource = lightSource;
        }

        #region Override Equals
        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(null, obj))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            return IsEqual((BlockLightNode)obj);
        }

        public bool Equals(BlockLightNode blockLightNode)
        {
            if (Object.ReferenceEquals(null, blockLightNode))
                return false;

            if (Object.ReferenceEquals(this, blockLightNode))
                return true;

            return IsEqual(blockLightNode);
        }

        private bool IsEqual(BlockLightNode blockLightNode)
        {
            return x == blockLightNode.x && y == blockLightNode.y && Color.Equals(blockLightNode.Color);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int HashingBase = (int)8032442819;
                const int HashingMultiplier = 5146919;

                int hash = HashingBase;
                hash = (hash * HashingMultiplier) ^ x.GetHashCode();
                hash = (hash * HashingMultiplier) ^ y.GetHashCode();
                hash = (hash * HashingMultiplier) ^ Color.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(BlockLightNode nodeA, BlockLightNode nodeB)
        {
            if (Object.ReferenceEquals(nodeA, nodeB))
                return true;
            
            if (Object.ReferenceEquals(null, nodeA))
                return false;

            return nodeA.Equals(nodeB);
        }

        public static bool operator !=(BlockLightNode nodeA, BlockLightNode nodeB)
        {
            return !(nodeA == nodeB);
        }
        #endregion
    }
}
