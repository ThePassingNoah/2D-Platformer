using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HiddenMap : MonoBehaviour
{
    public GameObject Hidden;
    public GameObject FakeHidden;
    private TilemapRenderer Renderer;
    private TilemapRenderer FakeRenderer;
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
        Renderer = Hidden.GetComponent<TilemapRenderer>();
        FakeRenderer = FakeHidden.GetComponent<TilemapRenderer>();
        ViewOn();
        Invoke("ViewOff", ViewTime);
    }

    private void ViewOn() 
    {
        Renderer.enabled = true;
        FakeRenderer.enabled = true;
    }
    private void ViewOff()
    {
        Renderer.enabled = false;
        FakeRenderer.enabled = false;
    }

}
