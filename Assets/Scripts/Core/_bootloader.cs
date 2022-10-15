using UnityEngine;
using UnityEngine.SceneManagement;

public class _bootloader : MonoBehaviour
{
    [Scene] [SerializeField] private string[] m_scenes;

    private void Awake()
    {
        foreach (string scene in m_scenes) {
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        }
    }

    private void Start()
    {
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}
