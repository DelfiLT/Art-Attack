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

        if (index == dialogs.Length - 1 && dialogUI.text == dialogs[index])
        {
            StartCoroutine(EndTutorial());

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
                    break ;
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
            OnDialogEnd();
        }
    }

    private void OnDialogEnd()
    {
        playerController.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Aura"))
        {
            playerController = collision.GetComponentInParent<PlayerController>();
            playerController.enabled = false;
            StartDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        this.enabled = false;
    }
}
