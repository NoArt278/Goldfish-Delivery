using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFish : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private Sprite down, up;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(SwimAnim());
    }
    IEnumerator SwimAnim()
    {
        while (true)
        {
            sr.sprite = down;
            yield return new WaitForSeconds(1);
            sr.sprite = up;
            yield return new WaitForSeconds(1);
        }
    }
}
