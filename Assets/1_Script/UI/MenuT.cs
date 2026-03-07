using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuT : MonoBehaviour
{
    public void Start_BTN()
    {
        SceneManager.LoadScene("GameScene");
    }
}
