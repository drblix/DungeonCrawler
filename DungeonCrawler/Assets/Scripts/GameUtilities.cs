using UnityEngine;

public static class GameUtilities
{
    /// <summary>
    /// Fetches the requested asset by the provided file name
    /// </summary>
    /// <param name="fileName">Name of the file requested</param>
    /// <returns>Returns the prefab/file that was requested</returns>
    /// <exception cref="System.Exception">Throws if the provided file name could not be found</exception>
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
