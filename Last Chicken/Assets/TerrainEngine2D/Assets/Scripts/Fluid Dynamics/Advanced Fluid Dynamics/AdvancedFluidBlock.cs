using UnityEngine;
using System;

// Copyright (C) 2018 Matthew K Wilson

namespace TerrainEngine2D
{
    [Serializable]
    /// <summary>
    /// A block of fluid
    /// </summary>
    public class AdvancedFluidBlock
    {
        /// <summary>
        /// Weight used for representing solid blocks
        /// </summary>
        public const int SolidWeight = -100;
        /// <summary>
        /// The Density used for representing air blocks
        /// </summary>
        public const byte AirDensity = 0;

        //**Optimized for use in the editor**
        /// <summary>
        /// Total amount of liquid in the block
        /// </summary>
        public float Weight;
        /// <summary>
        /// The color of the liquid 
        /// </summary>
        public Color32 Color;
        /// <summary>
        /// The density of the liquid
        /// </summary>
        public byte Density = 0;
        /// <summary>
        /// If the fluid has settled
        /// </summary>
        public bool Stable;
        /// <summary>
        /// Adjacent top FluidBlock
        /// </summary>
        public AdvancedFluidBlock TopBlock;
        /// <summary>
        /// Adjacent bottom FluidBlock
        /// </summary>
        public AdvancedFluidBlock BottomBlock;
        /// <summary>
        /// Adjacent left FluidBlock
        /// </summary>
        public AdvancedFluidBlock LeftBlock;
        /// <summary>
        /// Adjacent right FluidBlock
        /// </summary>
        public AdvancedFluidBlock RightBlock;

        /// <summary>
        /// Check if fluid block is solid
        /// </summary>
        /// <returns>Returns true if the block is solid</returns>
        public bool IsSolid()
        {
            return Weight == SolidWeight;
        }
        /// <summary>
        /// Sets fluid block to solid
        /// </summary>
        public void SetSolid()
        {
            Weight = SolidWeight;
            Density = AirDensity;
            UnsettleNeighbours();
        }
        /// <summary>
        /// Empties fluid block
        /// </summary>
        public void SetEmpty()
        {
            Weight = 0;
            Stable = false;
            Density = AirDensity;
            UnsettleNeighbours();
        }
        /// <summary>
        /// Adds liquid to the fluid block
        /// </summary>
        /// <param name="density">The density (type) of the fluid</param>
        /// <param name="amount">Amount of fluid to add</param>
        /// <param name="color">The color of the fluid</param>
        public void AddWeight(byte density, float amount, Color32 color)
        {
            Weight += amount;
            Stable = false;
            Density = density;
            if (Color.Equals(AdvancedFluidDynamics.ClearColor))
                Color = color;
            else
                Color = Color32.Lerp(Color, color, amount / Weight);
        }
        /// <summary>
        /// Set all adjacent blocks to unstable
        /// </summary>
        public void UnsettleNeighbours()
        {
            if (TopBlock != null)
                TopBlock.Stable = false;
            if (BottomBlock != null)
                BottomBlock.Stable = false;
            if (LeftBlock != null)
                LeftBlock.Stable = false;
            if (RightBlock != null)
                RightBlock.Stable = false;
        }

        /// <summary>
        /// Get the height of the fluid block (for mesh generation)
        /// </summary>
        /// <returns></returns>
        public float GetHeight()
        {
            //Height is set based on the amount of fluid
            float height = Mathf.Min(1, Weight);
            //Set falling blocks as full
            if (TopBlock != null && TopBlock.Weight > 0 && TopBlock.Density == Density)
                height = 1;
            return height;
        }
    }

}