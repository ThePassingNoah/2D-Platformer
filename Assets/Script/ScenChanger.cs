using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenChanger : MonoBehaviour
{
    public void ScenChange()
    {
        SceneManager.LoadScene("Stage1");
    }
}
