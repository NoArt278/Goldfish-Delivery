using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageBubble : MonoBehaviour
{
    private const float scaleFactor = 1f;
    [SerializeField] SpriteRenderer packageSprite;

    public void SetPackageSprite(Sprite sprite, Color color)
    {
        packageSprite.sprite = sprite;
        packageSprite.color = color;
    } 

    public void Show()
    {
        StartCoroutine(Appear());
    }

    public void Hide()
    {
        StartCoroutine(Disappear());
    }

    IEnumerator Appear()
    {
        while (transform.localScale.x < 1)
        {
            transform.localScale += new Vector3(scaleFactor, scaleFactor, scaleFactor) * Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.one;
    }

    IEnumerator Disappear()
    {
        while (transform.localScale.x > 0)
        {
            transform.localScale -= new Vector3(scaleFactor, scaleFactor, scaleFactor) * Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.zero;
    }
}
