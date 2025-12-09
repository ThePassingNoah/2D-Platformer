using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Scene scene = SceneManager.GetActiveScene();
            int curScene = scene.buildIndex;
            int nextScene = curScene + 1;
            SceneManager.LoadScene(nextScene);
        }
    }
}
