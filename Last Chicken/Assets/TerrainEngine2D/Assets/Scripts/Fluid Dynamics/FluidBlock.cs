using UnityEngine;
using System;

// Copyright (C) 2018 Matthew K Wilson

namespace TerrainEngine2D
{
    [Serializable]
    /// <summary>
    /// A block of fluid
    /// </summary>
    public class FluidBlock
    {
        /// <summary>
        /// Weight used for representing solid blocks
        /// </summary>
        public const int SolidWeight = -100;

        //**Optimized for use in the editor**
        /// <summary>
        /// Amount of liquid in the block
        /// </summary>
        public float Weight;
        /// <summary>
        /// If the fluid has settled
        /// </summary>
        public bool Stable;
        /// <summary>
        /// Adjacent top FluidBlock
        /// </summary>
        public FluidBlock TopBlock;
        /// <summary>
        /// Adjacent bottom FluidBlock
        /// </summary>
        public FluidBlock BottomBlock;
        /// <summary>
        /// Adjacent left FluidBlock
        /// </summary>
        public FluidBlock LeftBlock;
        /// <summary>
        /// Adjacent right FluidBlock
        /// </summary>
        public FluidBlock RightBlock;

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
            UnsettleNeighbours();
        }
        /// <summary>
        /// Empties fluid block
        /// </summary>
        public void SetEmpty()
        {
            Weight = 0;
            Stable = false;
            UnsettleNeighbours();
        }
        /// <summary>
        /// Adds liquid to the fluid block
        /// </summary>
        /// <param name="amount">Amount of fluid to add</param>
        public void AddWeight(float amount)
        {
            Weight += amount;
            Stable = false;
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
            if (TopBlock != null && TopBlock.Weight > 0)
                height = 1;
            return height;
        }
    }

}