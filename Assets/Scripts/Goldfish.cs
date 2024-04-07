using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Goldfish : MonoBehaviour
{
    private List<Package> heldPackages;
    private LineRenderer lr;
    private House destHouse;
    private const float moveSpeed = 0.5f;
    private bool selectable, isFlipped;
    private Rigidbody2D rb;
    private Coroutine flipDir;
    [SerializeField] private SpriteRenderer packageSprite;

    [HideInInspector] public LevelManager levelManager;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        heldPackages = new List<Package>();
        lr.enabled = false;
        selectable = true;
    }

    public void AddPackage(Package p)
    {
        heldPackages.Add(p);
        packageSprite.sprite = p.GetComponent<SpriteRenderer>().sprite;
        packageSprite.color = p.GetComponent<SpriteRenderer>().color;
    }

    public void SelectHouse(House h)
    {
        if (destHouse != null)
        {
            destHouse.SetSender(null);
        }
        destHouse = h;
        destHouse.SetSender(this);
        if (destHouse.transform.position.x < transform.position.x && transform.rotation.eulerAngles.y != 180)
        {
            isFlipped = true;
            if (flipDir != null)
            {
                StopCoroutine(flipDir);
            }
            flipDir = StartCoroutine(FlipDirection(180));
        }
        else if (destHouse.transform.position.x >= transform.position.x && transform.rotation.eulerAngles.y != 0)
        {
            isFlipped = false;
            if (flipDir != null)
            {
                StopCoroutine(flipDir);
            }
            flipDir = StartCoroutine(FlipDirection(0));
        }
    }

    public Package GetCurrentPackage()
    {
        return heldPackages[heldPackages.Count - 1];
    }

    public void RemovePackage()
    {
        heldPackages.RemoveAt(heldPackages.Count - 1);
        if (heldPackages.Count > 0)
        {
            packageSprite.sprite = heldPackages[heldPackages.Count - 1].GetComponent<SpriteRenderer>().sprite;
            packageSprite.color = heldPackages[heldPackages.Count - 1].GetComponent<SpriteRenderer>().color;
        } else
        {
            selectable = false;
            packageSprite.transform.parent.gameObject.SetActive(false);
            destHouse = null;
        }
    }

    private void OnMouseDown()
    {
        if (selectable)
        {
            lr.enabled = true;
            levelManager.selectedGoldfish = this;
        }
    }

    private void OnMouseDrag()
    {
        if (selectable)
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    private void OnMouseUp()
    {
        if (selectable)
        {
            lr.enabled = false;
            levelManager.selectedGoldfish = null;
        }
    }

    void FixedUpdate()
    {
        packageSprite.transform.parent.position = transform.position + new Vector3(1.1f * (isFlipped ? -1 : 1), 0.8f);
        if (destHouse != null)
        {
            rb.MovePosition(Vector3.Lerp(transform.position, destHouse.transform.position, moveSpeed * Time.fixedDeltaTime));
        }
    }

    IEnumerator FlipDirection(float angle)
    {
        while (transform.rotation.eulerAngles.y != angle)
        {
            Quaternion rot = Quaternion.Euler(new Vector3(0, angle));
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, 10 * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
    }
}
