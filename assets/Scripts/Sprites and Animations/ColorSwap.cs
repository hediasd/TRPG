using UnityEngine;
using System.Collections.Generic;
using System;

public class ColorSwap : MonoBehaviour
{
    Texture2D textr2D;
    SpriteRenderer spriteRenderer;
    Sprite spriteBase;

    public List<Color32> news = new List<Color32>();
    public List<Color32> olds = new List<Color32>();

    int mode = 0;

    public void Startup(Sprite spr)
    {
        spriteBase = spr;
        spriteRenderer = GetComponent<SpriteRenderer>();
        init();
        swapColors();
    }

    public void swapColors()
    {
        for (int i = 0; i < news.Count; i++)
        {
            if(mode==0) textr2D.SetPixel((int)olds[i].r, 0, news[i]);
            //else textr2D.SetPixel((int)olds[i].g, 0, news[i]);
            //indexColorSwap(olds[i].g, news[i]);   
        }
        textr2D.Apply();
    }

    public void indexColorSwap(float index, Color color)
    {
        //mSpriteColors[(int)index] = color;
        textr2D.SetPixel((int)index, 0, color);
    }

    public void init()
    {
        Color32[] c32 = spriteBase.texture.GetPixels32();

        for (int i = 0; i < c32.Length; i++)
        {
            if(!olds.Contains(c32[i]) && c32[i].a > 0.1f) olds.Add(c32[i]);
            if(olds.Count == 3) break;
        }

        olds.Reverse();
        Texture2D colorSwapTex = new Texture2D(256, 1, TextureFormat.RGBA32, false, false);
        colorSwapTex.filterMode = FilterMode.Point;
        for (int i = 0; i < colorSwapTex.width; i++)
            colorSwapTex.SetPixel(i, 0, new Color(0.0f, 0.0f, 0.0f, 0.0f));
        colorSwapTex.Apply();
        spriteRenderer.material.SetTexture("_SwapTex", colorSwapTex);
        textr2D = colorSwapTex;

    }


    /*public void ClearColor(SwapIndex index)
    {
        Color c = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        mSpriteColors[(int)index] = c;
        texture.SetPixel((int)index, 0, c);
    }*/

}
