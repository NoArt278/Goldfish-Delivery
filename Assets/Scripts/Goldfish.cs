using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Goldfish : MonoBehaviour
{
    private List<Package> heldPackages;
    private LineRenderer lr;
    private House destHouse;
    private const float moveSpeed = 0.5f;
    private bool selectable;
    private Rigidbody2D rb;
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
    }

    public Package GetCurrentPackage()
    {
        return heldPackages[heldPackages.Count - 1];
    }

    public void RemovePackage()
    {
        heldPackages.RemoveAt(heldPackages.Count - 1);
        int remainingHouseCount = levelManager.GetHouseList().Count;
        if (remainingHouseCount > 0 && heldPackages.Count > 0)
        {
            int chosenIdx = Random.Range(0, levelManager.GetHouseList().Count);
            SelectHouse(levelManager.GetHouseList()[chosenIdx]);
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
        if (destHouse != null)
        {
            Vector2 direction = new Vector2(destHouse.transform.position.x, destHouse.transform.position.y) - new Vector2(transform.position.x, transform.position.y);
            rb.MoveRotation(Quaternion.FromToRotation(Vector3.up, direction));
            rb.MovePosition(Vector3.Lerp(transform.position, destHouse.transform.position, moveSpeed * Time.fixedDeltaTime));
        }
    }
}
