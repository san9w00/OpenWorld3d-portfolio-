using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuT : MonoBehaviour
{
    public void Start_BTN()
    {
        SceneLoader.Instance.LoadScene("GameScene");
        SoundManager.Instance.PlaySFX(SFXType.UIClick);
    }
}
