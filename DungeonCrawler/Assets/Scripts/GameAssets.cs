using UnityEngine;

public static class GameAssets
{
    public static GameObject LoadPrefabFromFile(string fileName)
    {
        Debug.Log(string.Format("Fetching prefab of name '{0}'", fileName));

        var loadedObject = Resources.Load<GameObject>(fileName);

        if (loadedObject != null)
        {
            return loadedObject;
        }
        else
        {
            throw new System.Exception(string.Format("File of name '{0}' not found!", fileName));
        }
    }
}
