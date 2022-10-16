using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    public void OnReturnToMainMenuPress()
    {
        Destroy(SFXManager.Instance.gameObject);
        Destroy(GameManager.Instance.gameObject);
        MusicManager.OnStopMusic.Invoke();
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
}
