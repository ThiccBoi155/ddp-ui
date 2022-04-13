using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    public Texture2D texture;
    public Sprite discSprite = null;

    public void SetTexture()
    {
        Material material = new Material(Shader.Find("Unlit/Texture"));
        material.mainTexture = texture;

        GetComponent<Renderer>().material = material;
    }

    void CreateSprite()
    {
        Rect imageRect = new Rect(0f, 0f, texture.width, texture.height);

        Vector2 pivot = new Vector2(.5f, .5f);

        float pixelsPerUnit;

        if (texture.width < texture.height)
            pixelsPerUnit = texture.width;
        else
            pixelsPerUnit = texture.height;

        discSprite = Sprite.Create(texture, imageRect, pivot, pixelsPerUnit);

        discSprite.name = "nice name";
    }

    public Sprite GetDiscSprite()
    {
        if (discSprite == null)
            CreateSprite();

        return discSprite;
    }
}
