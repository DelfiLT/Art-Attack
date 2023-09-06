using System.Collections;
using TarodevController;
using TMPro;
using UnityEngine;

public class Master : MonoBehaviour
{
    PlayerController playerController;

    [Header("Master Type")]
    [SerializeField] private MasterType masterType;

    [Header("Master Dialogs")]
    [SerializeField] private TextMeshProUGUI dialogUI;
    [SerializeField] private string[] dialogs;

    private float textSpeed = 0.2f;
    int index;
    bool dialogCompleted = false;

    void Start()
    {
        dialogUI.text = string.Empty;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(dialogUI.text == dialogs[index])
            {
                NextLine();
            } else
            {
                StopAllCoroutines();
                dialogUI.text = dialogs[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(WriteLine());
    }

    IEnumerator WriteLine()
    {
        foreach(char letter in dialogs[index].ToCharArray())
        {
            dialogUI.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    IEnumerator EndTutorial()
    {
        yield return new WaitForSeconds(4f);
        dialogUI.text = string.Empty;
    }

    public void NextLine()
    {
        if(index < dialogs.Length - 1)
        {
            index++;
            dialogUI.text = string.Empty;
            StartCoroutine(WriteLine());
        }
        else
        {
            dialogCompleted = true;
            OnDialogEnd();
        }
    }

    private void OnDialogEnd()
    {
        playerController.enabled = true;

        switch (masterType)
        {
            case MasterType.Earth:
                GameManager.Instance.AddPower(PowerType.Earth);
                break;
            case MasterType.Ice:
                GameManager.Instance.AddPower(PowerType.Ice);
                break;
            case MasterType.Fire:
                GameManager.Instance.AddPower(PowerType.Fire);
                break;
            default:
                break;
        }

        StartCoroutine(EndTutorial());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!dialogCompleted && collision.CompareTag("Aura"))
        {
            playerController = collision.GetComponentInParent<PlayerController>();
            playerController.enabled = false;
            StartDialogue();
        }
    }
}
