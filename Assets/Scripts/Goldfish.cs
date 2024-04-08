using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goldfish : MonoBehaviour
{
    private List<Package> heldPackages;
    private LineRenderer lr;
    private House destHouse, tempDest;
    private const float moveSpeed = 2f;
    private bool selectable, started;
    private Rigidbody2D rb;
    private Coroutine flipDir;
    private SpriteRenderer sr;
    private AudioSource audSource;
    [SerializeField] private Transform postOffice;
    [SerializeField] private PackageBubble packageBubble;
    [SerializeField] private Sprite down, up;

    [HideInInspector] public LevelManager levelManager;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audSource = GetComponent<AudioSource>();
        heldPackages = new List<Package>();
        lr.enabled = false;
        selectable = false;
        started = false;
        StartCoroutine(SwimAnim());
        StartCoroutine(StartWait());
    }

    public void AddPackage(Package p)
    {
        heldPackages.Add(p);
        packageBubble.SetPackageSprite(p.GetComponent<SpriteRenderer>().sprite, p.GetComponent<SpriteRenderer>().color);
    }

    public void SelectHouse(House h)
    {
        tempDest = h;
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
            if (levelManager.selectedGoldfish == this)
            {
                levelManager.selectedGoldfish = null;
            }
            lr.enabled = false;
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
            audSource.Play();
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
            if (tempDest != null)
            {
                if (destHouse != null)
                {
                    destHouse.SetSender(null);
                }
                destHouse = tempDest;
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
        }
    }

    void FixedUpdate()
    {
        packageBubble.transform.position = transform.position + new Vector3(0.7f, 0.6f);
        if (destHouse != null)
        {
            rb.MovePosition(Vector3.MoveTowards(transform.position, destHouse.transform.position, moveSpeed * Time.fixedDeltaTime));
        } else if (!selectable && started) // Done delivering packages
        {
            rb.MovePosition(Vector3.MoveTowards(transform.position, postOffice.position, moveSpeed * Time.fixedDeltaTime));
        }
    }

    IEnumerator StartWait()
    {
        // Wait until house is done showing wanted packages
        yield return new WaitForSeconds(5f);
        selectable = true;
        started = true;
        packageBubble.Show();
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
