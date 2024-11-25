using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index;

    public GameObject contButton;

    public float wordSpeed;
    public bool playerIsClose;

     // Add Button References for choices
    public GameObject choicePanel;
    public Button choice1Button;
    public Button choice2Button;
    public TextMeshProUGUI choice1Text;
    public TextMeshProUGUI choice2Text;

    private bool isChoosing = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose && !isChoosing)
        {
            if (dialoguePanel.activeInHierarchy)
            {
                // Dialogue already active
            }
            else
            {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }
        if (dialogueText.text == dialogue[index])
        {
            contButton.SetActive(true);
        }
    }

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false); // Hide choice panel
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        contButton.SetActive(false);

        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            zeroText();
            ShowChoices(); // After dialogue is over, show choices
        }
    }

    // Show dialogue choices
    public void ShowChoices()
    {
        isChoosing = true;
        choicePanel.SetActive(true); // Display the choice buttons
        choice1Text.text = "“A secret.”";
        choice2Text.text = "“A shadow.”";

        choice1Button.onClick.AddListener(() => ChoiceSelected(1));
        choice2Button.onClick.AddListener(() => ChoiceSelected(2));
    }

    // Handle player choice
    public void ChoiceSelected(int choiceIndex)
    {
        isChoosing = false;
        choicePanel.SetActive(false); // Hide choices once selected

        if (choiceIndex == 1)
        {
            dialogueText.text = "“Smart cat. You may go ahead.”";
        }
        else if (choiceIndex == 2)
        {
            dialogueText.text = "WRONG!!";
        }

        StartCoroutine(HideDialogueAfterDelay(4f));
        //StartCoroutine(Exitgame(4f));

        dialoguePanel.SetActive(true); // Show dialogue after selection
    }

    IEnumerator HideDialogueAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay); // Wait for the given delay time
        dialoguePanel.SetActive(false); // Hide the dialogue panel
    
    }

    IEnumerator Exitgame(float delay)
    {

    yield return new WaitForSeconds(delay); // Wait for the given delay time
    Application.Quit(); // Exit game
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
        #endif
    
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
        }
    }

}
