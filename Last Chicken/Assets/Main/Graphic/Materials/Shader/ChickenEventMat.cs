using System.Collections.Generic;
using UnityEngine;

public class ChickenEventMat : Material
{
    public Texture SpriteTexture => mainTexture;

    private const string eventShaderName = "Custom/CreateChickenEvent";

    private static readonly Shader shader = Shader.Find(eventShaderName);
    private static readonly List<ChickenEventMat> sharedMaterials = new List<ChickenEventMat>();

    public ChickenEventMat(Texture spriteTexture)
        : base(shader)
    {
        if (!shader) Debug.LogError($"`{eventShaderName}` shader not found. Make sure the shader is included to the build.");
        mainTexture = spriteTexture;
    }

    public static Material GetSharedFor(CreateChickenEvent spriteGlow)
    {
        for (int i = 0; i < sharedMaterials.Count; i++)
        {
            if (sharedMaterials[i].SpriteTexture == spriteGlow.Renderer.sprite.texture)
                return sharedMaterials[i];
        }

        var material = new ChickenEventMat(spriteGlow.Renderer.sprite.texture);
        material.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor | HideFlags.NotEditable;
        sharedMaterials.Add(material);

        return material;
    }
}
