using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    public void OnReturnToMainMenuPress()
    {
        Destroy(SFXManager.Instance.gameObject);
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
