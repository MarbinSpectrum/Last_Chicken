using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ChangeSprite
{
    [ExecuteInEditMode]
    public class ChangeColorSprite : MonoBehaviour
    {
        static Dictionary<int,Material> materials = new Dictionary<int, Material>();
        [HideInInspector] public int isKEY = -1;
        public SpriteRenderer spriteRenderer;
        public Image image;

        public Material material;
        [System.Serializable]
        public struct ColorList
        {
            [Header("사용여부")]
            public bool UseColor;
            [Header("변경하는색상")]
            [ColorUsage(false)]
            public Color ChangeColor;
            [Header("변경되는색상")]
            [ColorUsage(false)]
            public Color Color;
        }

        public ColorList color1;
        public ColorList color2;
        public ColorList color3;
        public ColorList color4;

        [Range(0, 1)]
        public float Power;

        Material GetMat(int key)
        {
            try
            {
                return materials[isKEY];
            }
            catch
            {
                return null;
            }

        }

        void Update()
        {
            if (spriteRenderer && material)
            {
                Material tempMaterial;
                if (GetMat(isKEY) == null)
                {
                    tempMaterial = new Material(material);
                    isKEY = materials.Count;
                    materials[isKEY] = tempMaterial;
                }

                tempMaterial = GetMat(isKEY);

                tempMaterial.SetInt("_UseColor1", color1.UseColor ? 1 : 0);
                tempMaterial.SetColor("_ChangeColor1", color1.ChangeColor);
                tempMaterial.SetColor("_Color1", color1.Color);

                tempMaterial.SetInt("_UseColor2", color2.UseColor ? 1 : 0);
                tempMaterial.SetColor("_ChangeColor2", color2.ChangeColor);
                tempMaterial.SetColor("_Color2", color2.Color);

                tempMaterial.SetInt("_UseColor3", color3.UseColor ? 1 : 0);
                tempMaterial.SetColor("_ChangeColor3", color3.ChangeColor);
                tempMaterial.SetColor("_Color3", color3.Color);

                tempMaterial.SetInt("_UseColor4", color4.UseColor ? 1 : 0);
                tempMaterial.SetColor("_ChangeColor4", color4.ChangeColor);
                tempMaterial.SetColor("_Color4", color4.Color);

                tempMaterial.SetFloat("Power", Power);

                spriteRenderer.material = tempMaterial;
            }
            if (image && material)
            {
                Material tempMaterial;
                if (GetMat(isKEY) == null)
                {
                    tempMaterial = new Material(material);
                    isKEY = materials.Count;
                    materials[isKEY] = tempMaterial;
                }

                tempMaterial = GetMat(isKEY);

                tempMaterial.SetInt("_UseColor1", color1.UseColor ? 1 : 0);
                tempMaterial.SetColor("_ChangeColor1", color1.ChangeColor);
                tempMaterial.SetColor("_Color1", color1.Color);

                tempMaterial.SetInt("_UseColor2", color2.UseColor ? 1 : 0);
                tempMaterial.SetColor("_ChangeColor2", color2.ChangeColor);
                tempMaterial.SetColor("_Color2", color2.Color);

                tempMaterial.SetInt("_UseColor3", color3.UseColor ? 1 : 0);
                tempMaterial.SetColor("_ChangeColor3", color3.ChangeColor);
                tempMaterial.SetColor("_Color3", color3.Color);

                tempMaterial.SetInt("_UseColor4", color4.UseColor ? 1 : 0);
                tempMaterial.SetColor("_ChangeColor4", color4.ChangeColor);
                tempMaterial.SetColor("_Color4", color4.Color);

                tempMaterial.SetFloat("Power", Power);

                image.material = tempMaterial;
            }
        }
    }
}
