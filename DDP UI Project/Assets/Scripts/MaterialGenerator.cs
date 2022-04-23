using UnityEngine;
using UnityEditor;

public class MaterialGenerator
{
    #if UNITY_EDITOR

    [MenuItem("Assets/Add covers to scene")]
    private static void CreatePrefabMaterials()
    {
        int i = 0;
        foreach (Object o in Selection.objects)
        {

            if (o.GetType() != typeof(Texture2D))
            {
                Debug.LogError("This isn't a texture: " + o);
                continue;
            }

            Debug.Log("Creating material from: " + o);

            GameObject newCover = GameObject.CreatePrimitive(PrimitiveType.Plane);

            newCover.name = $"Cover {i}";

            newCover.transform.localScale = new Vector3(.4f, .4f, .4f);

            Cover coverRef = newCover.AddComponent<Cover>();

            coverRef.discSprite = o as Sprite;

            coverRef.texture = o as Texture2D;

            coverRef.SetTexture();

            //PrefabUtility.SaveAsPrefabAsset(newPlane, newAssetName);

            i++;
        }

        Debug.Log("Prefab materials created (mabye)");
    }

    #endif
}
