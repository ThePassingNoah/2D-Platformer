using System.Collections;
using UnityEngine;

public class BlankMap : MonoBehaviour
{
    public GameObject blankmap;
    public float onDuration = 2f;
    public float offDuration = 2f;
    public bool blank2 = false;

    private void Start()
    {
        if(!blank2)
            StartCoroutine(BlinkLoop());
        else
            StartCoroutine(BlinkLoop2());
    }

    IEnumerator BlinkLoop()
    {
        while (true)
        {
            blankmap.SetActive(true);
            yield return new WaitForSeconds(onDuration);

            blankmap.SetActive(false);
            yield return new WaitForSeconds(offDuration);
        }
    }

    IEnumerator BlinkLoop2()
    {
        while (true)
        {
            blankmap.SetActive(false);
            yield return new WaitForSeconds(onDuration);

            blankmap.SetActive(true);
            yield return new WaitForSeconds(offDuration);
        }
    }
}
