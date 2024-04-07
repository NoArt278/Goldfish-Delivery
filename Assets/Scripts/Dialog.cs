using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    public List<string> lines;
    public int index = 0;

    [SerializeField] private TMP_Text dialogueText;
    private const float dialogueSpeed = 0.1f;
    private Coroutine dialogueCoroutine;

    private void Awake()
    {
        dialogueCoroutine = StartCoroutine(PlayDialog());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (dialogueCoroutine != null)
            {
                StopAllCoroutines();
                dialogueText.text = lines[index];
                dialogueCoroutine = null;
            }
            else
            {
                if (index < lines.Count - 1)
                {
                    dialogueText.text = "";
                    index++;
                    dialogueCoroutine = StartCoroutine(PlayDialog());
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }

    IEnumerator PlayDialog()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(dialogueSpeed);
        }
    }
}
