using System.Linq;
using UnityEditor;

public class SceneUtil
{
    // Constants
    private const string _scenesPath = "Assets/App/Scenes/";

    #region App lifecycle

    [MenuItem("Tools/Scene Utils/Only Main Scene #1")]
    static void OnlyMainScene()
    {
        var scenesPath = new string[]
        {
            _scenesPath + "Main.unity"
        };

        UnityEditor.EditorSettings.prefabRegularEnvironment = null;

        AddScenes(scenesPath);
        CloseAllScenesExcept(scenesPath);
    }


    [MenuItem("Tools/Scene Utils/Character Playground #2")]
    static void AddAllScenes()
    {
        var scenesPath = new string[]
        {
            "Assets/App/Scenes/UI.unity"
        };

        UnityEditor.EditorSettings.prefabRegularEnvironment = null;

        AddScenes(scenesPath);
        CloseAllScenesExcept(scenesPath);
    }

    #endregion

    #region Methods

    // ---------------------------------------------------------------------------------------------------------
    // Private Methods (static)
    // ---------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Add scene in hierarchy
    /// </summary>
    /// <param name="scenesPath">Scenes which we do want to add<</param>
    private static void AddScenes(params string[] scenesPath)
    {
        foreach (var scenePath in scenesPath)
        {
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath, UnityEditor.SceneManagement.OpenSceneMode.Additive);
        }
    }


    /// <summary>
    /// Close all scenes except scenesPath
    /// </summary>
    /// <param name="scenesPath">Scenes which we do not want to close</param>
    private static void CloseAllScenesExcept(params string[] scenesPath)
    {
        // Convert scene paths into scenes 
        UnityEngine.SceneManagement.Scene[] scenes = new UnityEngine.SceneManagement.Scene[scenesPath.Length];
        for (int i = 0; i < scenes.Length; i++)
            scenes[i] = UnityEditor.SceneManagement.EditorSceneManager.GetSceneByPath(scenesPath[i]);

        CloseAllScenesExcept(scenes);
    }


    /// <summary>
    /// Close all scenes except Scenes array
    /// </summary>
    /// <param name="scenes">Scenes which we do not want to close</param>
    private static void CloseAllScenesExcept(params UnityEngine.SceneManagement.Scene[] scenes)
    {
        var scenesInHierarchy = GetAllScenesInHierarchy();
        foreach (var scene in scenesInHierarchy)
        {
            if (!scenes.Contains(scene))
            {
                UnityEditor.SceneManagement.EditorSceneManager.CloseScene(scene, true);
            }
        }
    }


    /// <summary>
    /// Returns an array of all the Scenes currently open in the hierarchy
    /// </summary>
    private static UnityEngine.SceneManagement.Scene[] GetAllScenesInHierarchy()
    {
        UnityEngine.SceneManagement.Scene[] scenes = new UnityEngine.SceneManagement.Scene[UnityEditor.SceneManagement.EditorSceneManager.sceneCount];
        for (int i = 0; i < scenes.Length; i++)
            scenes[i] = UnityEditor.SceneManagement.EditorSceneManager.GetSceneAt(i);

        return scenes;
    }

    #endregion
}
