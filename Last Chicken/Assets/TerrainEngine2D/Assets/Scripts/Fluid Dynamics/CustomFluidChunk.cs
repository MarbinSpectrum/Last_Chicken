using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainEngine2D;

public class CustomFluidChunk : FluidChunk
{
    public bool surface;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();
    }

    public override void BuildChunk()
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
                        if (surface)
                        {
                            if (y + chunk.ChunkY + 1 < advancedFluidBlocks.GetLength(1))
                            {
                                if (!World.Instance.GetBlockLayer(1).IsBlockAt(x + chunk.ChunkX, y + chunk.ChunkY + 1))
                                {
                                    AdvancedFluidBlock checkfluid = advancedFluidBlocks[x + chunk.ChunkX, y + chunk.ChunkY + 1];
                                    if (checkfluid.Weight <= minWeight)
                                        blockGridMesh.CreateBlock(x, y, zOrder, new Vector2(0, 0), 0, 1, 1, 1, height, color);
                                }
                            }
                        }
                        else
                        {
                            if (y + chunk.ChunkY + 1 < advancedFluidBlocks.GetLength(1))
                            {
                                if (!World.Instance.GetBlockLayer(1).IsBlockAt(x + chunk.ChunkX, y + chunk.ChunkY + 1))
                                {
                                    AdvancedFluidBlock checkfluid = advancedFluidBlocks[x + chunk.ChunkX, y + chunk.ChunkY + 1];
                                    if (checkfluid.Weight > minWeight)
                                        blockGridMesh.CreateBlock(x, y, zOrder, new Vector2(0, 0), 0, 1, 1, 1, height, color);
                                }
                                else
                                    blockGridMesh.CreateBlock(x, y, zOrder, new Vector2(0, 0), 0, 1, 1, 1, height, color);
                            }
                            else
                                blockGridMesh.CreateBlock(x, y, zOrder, new Vector2(0, 0), 0, 1, 1, 1, height, color);
                        }
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
                        if (surface)
                        {
                            if (y + chunk.ChunkY + 1 < advancedFluidBlocks.GetLength(1))
                            {
                                if (!World.Instance.GetBlockLayer(1).IsBlockAt(x + chunk.ChunkX, y + chunk.ChunkY + 1))
                                {
                                    AdvancedFluidBlock checkfluid = advancedFluidBlocks[x + chunk.ChunkX, y + chunk.ChunkY + 1];
                                    if (checkfluid.Weight <= minWeight)
                                        blockGridMesh.CreateBlock(x, y, zOrder, new Vector2(0, 0), 0, 1, 1, 1, height, color);
                                }
                            }
                        }
                        else
                        {
                            if (y + chunk.ChunkY + 1 < advancedFluidBlocks.GetLength(1))
                            {
                                if (!World.Instance.GetBlockLayer(1).IsBlockAt(x + chunk.ChunkX, y + chunk.ChunkY + 1))
                                {
                                    AdvancedFluidBlock checkfluid = advancedFluidBlocks[x + chunk.ChunkX, y + chunk.ChunkY + 1];
                                    if (checkfluid.Weight > minWeight)
                                        blockGridMesh.CreateBlock(x, y, zOrder, new Vector2(0, 0), 0, 1, 1, 1, height, color);
                                }
                                else
                                    blockGridMesh.CreateBlock(x, y, zOrder, new Vector2(0, 0), 0, 1, 1, 1, height, color);
                            }
                            else
                                blockGridMesh.CreateBlock(x, y, zOrder, new Vector2(0, 0), 0, 1, 1, 1, height, color);
                        }
                    }
                }
            }
        }
        //Update the mesh
        blockGridMesh.UpdateMesh();
    }
}
