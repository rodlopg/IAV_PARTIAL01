using UnityEngine;
// 1. You MUST include this namespace to use SceneManager
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Load a scene using its exact name as a string
    public static void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Load a scene using its index number from the Build Settings
    public static void LoadSceneByIndex(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    // Example: Reload the current active scene
    public void ReloadCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
