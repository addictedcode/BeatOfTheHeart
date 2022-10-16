using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public void OnBackPress()
    {
        SceneManager.UnloadSceneAsync(gameObject.scene.name);
    }
}
