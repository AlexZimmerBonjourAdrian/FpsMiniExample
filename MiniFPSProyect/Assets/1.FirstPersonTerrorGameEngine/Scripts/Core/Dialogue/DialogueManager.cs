using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public string speakerName;
        public string text;
        public AudioClip voiceLine;
        public float displayDuration = 3f;
        public float typingSpeed = 0.05f;
        public bool waitForInput = false;
        public string[] choices;
        public string[] choiceConsequences;
        public bool isImportant = false;
        public string[] requiredFlags;
        public string[] setFlags;
    }
    
    [System.Serializable]
    public class DialogueSequence
    {
        public string sequenceName;
        public List<DialogueLine> lines = new List<DialogueLine>();
        public bool isOneTime = false;
        public string[] requiredQuests;
        public string[] nextSequences;
    }
    
    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject choicesPanel;
    [SerializeField] private GameObject choiceButtonPrefab;
    
    [Header("Configuración")]
    [SerializeField] private List<DialogueSequence> dialogueSequences = new List<DialogueSequence>();
    [SerializeField] private float defaultDisplayDuration = 3f;
    [SerializeField] private float defaultTypingSpeed = 0.05f;
    
    [Header("Referencias")]
    [SerializeField] private QuestManager questManager;
    [SerializeField] private AudioManager audioManager;
    
    private List<string> triggeredSequences = new List<string>();
    private List<string> activeFlags = new List<string>();
    private DialogueSequence currentSequence;
    private int currentLineIndex;
    private bool isDisplayingDialogue;
    private Coroutine typingCoroutine;
    
    private void Start()
    {
        if (questManager == null)
        {
            questManager = FindObjectOfType<QuestManager>();
        }
        
        if (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
        }
        
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }
    
    public void StartDialogueSequence(string sequenceName)
    {
        DialogueSequence sequence = dialogueSequences.Find(s => s.sequenceName == sequenceName);
        if (sequence != null && !sequence.isOneTime || !triggeredSequences.Contains(sequenceName))
        {
            currentSequence = sequence;
            currentLineIndex = 0;
            DisplayNextLine();
        }
    }
    
    private void DisplayNextLine()
    {
        if (currentSequence == null || currentLineIndex >= currentSequence.lines.Count)
        {
            EndDialogue();
            return;
        }
        
        DialogueLine currentLine = currentSequence.lines[currentLineIndex];
        
        // Verificar requisitos
        // if (!CheckLineRequirements(currentLine))
        // {
        //     currentLineIndex++;
        //     DisplayNextLine();
        //     return;
        // }
        
        // Mostrar UI
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }
        
        // Actualizar nombre del hablante
        if (speakerNameText != null)
        {
            speakerNameText.text = currentLine.speakerName;
        }
        
        // Reproducir línea de voz si existe
        if (currentLine.voiceLine != null && audioManager != null)
        {
            audioManager.PlaySound($"dialogue_{currentSequence.sequenceName}_{currentLineIndex}", false);
        }
        
        // Mostrar texto
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        
        typingCoroutine = StartCoroutine(TypeText(currentLine));
    }
    
    private IEnumerator TypeText(DialogueLine line)
    {
        if (dialogueText != null)
        {
            dialogueText.text = "";
            isDisplayingDialogue = true;
            
            foreach (char c in line.text)
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(line.typingSpeed);
            }
            
            isDisplayingDialogue = false;
        }
        
        // Manejar opciones
        if (line.choices != null && line.choices.Length > 0)
        {
            DisplayChoices(line);
        }
        else if (line.waitForInput)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }
        else
        {
            yield return new WaitForSeconds(line.displayDuration);
        }
        
        // Aplicar consecuencias
        ApplyLineConsequences(line);
        
        currentLineIndex++;
        DisplayNextLine();
    }
    
    private void DisplayChoices(DialogueLine line)
    {
        if (choicesPanel != null && choiceButtonPrefab != null)
        {
            choicesPanel.SetActive(true);
            
            foreach (Transform child in choicesPanel.transform)
            {
                Destroy(child.gameObject);
            }
            
            for (int i = 0; i < line.choices.Length; i++)
            {
                GameObject buttonObj = Instantiate(choiceButtonPrefab, choicesPanel.transform);
                UnityEngine.UI.Button button = buttonObj.GetComponent<UnityEngine.UI.Button>();
                TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
                
                if (buttonText != null)
                {
                    buttonText.text = line.choices[i];
                }
                
                int choiceIndex = i;
                button.onClick.AddListener(() => HandleChoice(choiceIndex));
            }
        }
    }
    
    private void HandleChoice(int choiceIndex)
    {
        if (currentSequence != null && currentLineIndex < currentSequence.lines.Count)
        {
            DialogueLine currentLine = currentSequence.lines[currentLineIndex];
            
            if (choiceIndex < currentLine.choiceConsequences.Length)
            {
                string consequence = currentLine.choiceConsequences[choiceIndex];
                ProcessConsequence(consequence);
            }
        }
        
        if (choicesPanel != null)
        {
            choicesPanel.SetActive(false);
        }
        
        currentLineIndex++;
        DisplayNextLine();
    }
    
    private void ProcessConsequence(string consequence)
    {
        // Procesar consecuencias como activar misiones, cambiar flags, etc.
        if (consequence.StartsWith("QUEST:"))
        {
            string questName = consequence.Substring(6);
            if (questManager != null)
            {
                questManager.ActivateQuest(questManager.GetQuestByName(questName));
            }
        }
        else if (consequence.StartsWith("FLAG:"))
        {
            string flag = consequence.Substring(5);
            SetFlag(flag);
        }
    }
    
    private void ApplyLineConsequences(DialogueLine line)
    {
        if (line.setFlags != null)
        {
            foreach (string flag in line.setFlags)
            {
                SetFlag(flag);
            }
        }
    }
    
    // private bool CheckLineRequirements(DialogueLine line)
    // {
    //     if (line.requiredFlags != null)
    //     {
    //         foreach (string flag in line.requiredFlags)
    //         {
    //             if (!HasFlag(flag))
    //             {
    //                 return false;
    //             }
    //         }
    //     }
        
    //     if (line.requiredQuests != null)
    //     {
    //         foreach (string questName in line.requiredQuests)
    //         {
    //             if (!questManager.IsQuestCompleted(questName))
    //             {
    //                 return false;
    //             }
    //         }
    //     }
        
    //     return true;
    // }
    
    private void EndDialogue()
    {
        if (currentSequence != null)
        {
            if (currentSequence.isOneTime)
            {
                triggeredSequences.Add(currentSequence.sequenceName);
            }
            
            // Activar secuencias siguientes
            if (currentSequence.nextSequences != null)
            {
                foreach (string nextSequence in currentSequence.nextSequences)
                {
                    StartDialogueSequence(nextSequence);
                }
            }
        }
        
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
        
        currentSequence = null;
        currentLineIndex = 0;
    }
    
    public void SetFlag(string flag)
    {
        if (!activeFlags.Contains(flag))
        {
            activeFlags.Add(flag);
        }
    }
    
    public bool HasFlag(string flag)
    {
        return activeFlags.Contains(flag);
    }
    
    public void ClearFlags()
    {
        activeFlags.Clear();
    }
    
    public void ResetDialogue()
    {
        triggeredSequences.Clear();
        activeFlags.Clear();
        currentSequence = null;
        currentLineIndex = 0;
        
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }
} 