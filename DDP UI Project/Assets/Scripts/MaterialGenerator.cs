using UnityEngine;
using UnityEditor;

public class MaterialGenerator
{
    [MenuItem("Assets/Create Materials")]
    private static void CreateMaterials()
    {
        foreach (Object o in Selection.objects)
        {

            if (o.GetType() != typeof(Texture2D))
            {
                Debug.LogError("This isn't a texture: " + o);
                continue;
            }

            Debug.Log("Creating material from: " + o);

            Texture2D selected = o as Texture2D;

            Material material = new Material(Shader.Find("Unlit/Texture"));
            material.mainTexture = (Texture)o;

            string savePath = AssetDatabase.GetAssetPath(selected);
            savePath = savePath.Substring(0, savePath.LastIndexOf('/') + 1);

            string newAssetName = savePath + selected.name + ".mat";

            AssetDatabase.CreateAsset(material, newAssetName);

            AssetDatabase.SaveAssets();

        }

        Debug.Log("Materials created (mabye)");
    }

    [MenuItem("Assets/Add covers to scene")]
    private static void CreatePrefabMaterials()
    {
        foreach (Object o in Selection.objects)
        {

            if (o.GetType() != typeof(Texture2D))
            {
                Debug.LogError("This isn't a texture: " + o);
                continue;
            }

            Debug.Log("Creating material from: " + o);

            Texture2D selected = o as Texture2D;

            Material material = new Material(Shader.Find("Unlit/Texture"));
            material.mainTexture = (Texture)o;

            GameObject newPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);

            newPlane.transform.localScale = new Vector3(.4f, .4f, .4f);

            newPlane.GetComponent<Renderer>().material = material;
            
            //PrefabUtility.SaveAsPrefabAsset(newPlane, newAssetName);
        }

        Debug.Log("Prefab materials created (mabye)");
    }
}
