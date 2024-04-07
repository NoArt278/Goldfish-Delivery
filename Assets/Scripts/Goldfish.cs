using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goldfish : MonoBehaviour
{
    private List<Package> heldPackages;
    private LineRenderer lr;
    private House destHouse;
    private const float moveSpeed = 1f;
    private bool selectable;
    private Rigidbody2D rb;
    private Coroutine flipDir;
    private SpriteRenderer sr;
    [SerializeField] private Transform postOffice;
    [SerializeField] private PackageBubble packageBubble;
    [SerializeField] private Sprite down, up;

    [HideInInspector] public LevelManager levelManager;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        heldPackages = new List<Package>();
        lr.enabled = false;
        selectable = true;
        packageBubble.Show();
        StartCoroutine(SwimAnim());
    }

    public void AddPackage(Package p)
    {
        heldPackages.Add(p);
        packageBubble.SetPackageSprite(p.GetComponent<SpriteRenderer>().sprite, p.GetComponent<SpriteRenderer>().color);
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
            if (flipDir != null)
            {
                StopCoroutine(flipDir);
            }
            flipDir = StartCoroutine(FlipDirection(180));
        }
        else if (destHouse.transform.position.x >= transform.position.x && transform.rotation.eulerAngles.y != 0)
        {
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
            packageBubble.SetPackageSprite(heldPackages[heldPackages.Count - 1].GetComponent<SpriteRenderer>().sprite,
                        heldPackages[heldPackages.Count - 1].GetComponent<SpriteRenderer>().color);
        } else
        {
            selectable = false;
            packageBubble.Hide();
            destHouse = null;
            if (postOffice.position.x < transform.position.x && transform.rotation.eulerAngles.y != 180)
            {
                if (flipDir != null)
                {
                    StopCoroutine(flipDir);
                }
                flipDir = StartCoroutine(FlipDirection(180));
            }
            else if (postOffice.position.x >= transform.position.x && transform.rotation.eulerAngles.y != 0)
            {
                if (flipDir != null)
                {
                    StopCoroutine(flipDir);
                }
                flipDir = StartCoroutine(FlipDirection(0));
            }
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
        packageBubble.transform.position = transform.position + new Vector3(0.7f, 0.6f);
        if (destHouse != null)
        {
            rb.MovePosition(Vector3.MoveTowards(transform.position, destHouse.transform.position, moveSpeed * Time.fixedDeltaTime));
        } else if (!selectable) // Done delivering packages
        {
            rb.MovePosition(Vector3.MoveTowards(transform.position, postOffice.position, moveSpeed * Time.fixedDeltaTime));
        }
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
