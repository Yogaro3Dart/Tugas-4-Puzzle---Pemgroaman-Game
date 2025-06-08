using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class UIMenuManager : MonoBehaviour
{
    [Tooltip("Tempat Scene Tujuan dari Tombol Play Game")]
    public Object sceneNameToLoad;
    public void PlayGame()
    {
        if(sceneNameToLoad == null)
        {
            Debug.LogError("Scene belum dipasang, tolong drag ke inspektor!");
            return;
        }
        SceneManager.LoadScene(sceneNameToLoad.name);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game..");
        Application.Quit();
    }
}
