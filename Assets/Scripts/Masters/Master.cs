using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Master : MonoBehaviour
{
    private EarthPower EarthPowerScript;
    private IcePower IcePowerScript;
    private FirePower FirePowerScript;

    [Header("Master Type")]
    [SerializeField] private MasterType masterType;

    [Header("Power's UI")]
    [SerializeField] private GameObject earthPowerUI;
    [SerializeField] private GameObject icePowerUI;
    [SerializeField] private GameObject firePowerUI;

    [Header("Master Dialogs")]
    [SerializeField] private TextMeshProUGUI dialogUI;
    [SerializeField] private string[] dialogs;

    private float textSpeed = 0.2f;
    int index;

    void Start()
    {
        EarthPowerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<EarthPower>();
        IcePowerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<IcePower>();
        FirePowerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<FirePower>();

        EarthPowerScript.enabled = false;
        IcePowerScript.enabled = false;
        FirePowerScript.enabled = false;

        earthPowerUI.SetActive(false);
        icePowerUI.SetActive(false);
        firePowerUI.SetActive(false);

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
                    EarthPowerScript.enabled = true;
                    earthPowerUI.SetActive(true);
                    break;
                case MasterType.Ice:
                    IcePowerScript.enabled = true;
                    icePowerUI.SetActive(true);
                    break;
                case MasterType.Fire:
                    FirePowerScript.enabled = true;
                    firePowerUI.SetActive(true);
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Aura"))
        {
            StartDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        this.enabled = false;
    }
}
