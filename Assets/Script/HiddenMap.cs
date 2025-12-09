using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HiddenMap : MonoBehaviour
{
    public GameObject hidden;
    private TilemapRenderer Renderer;
    public float ViewTime = 4f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnView();
        }
    }

    private void OnView()
    {
        Renderer = hidden.GetComponent<TilemapRenderer>();
        ViewOn();
        Invoke("ViewOff", ViewTime);
    }

    private void ViewOn() 
    {
        Renderer.enabled = true;
    }
    private void ViewOff() 
    {
        Renderer.enabled = false;
    }

}
