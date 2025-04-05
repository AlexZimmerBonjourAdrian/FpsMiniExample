using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    [System.Serializable]
    public class QuestObjective
    {
        public string objectiveName;
        public string description;
        public bool isCompleted;
        public bool isOptional;
        public GameObject[] relatedObjects;
        public string[] requiredItems;
        public int[] requiredItemQuantities;
        public string[] requiredEvents;
        public string[] requiredQuests;
    }
    
    [System.Serializable]
    public class Quest
    {
        public string questName;
        public string description;
        public List<QuestObjective> objectives = new List<QuestObjective>();
        public bool isCompleted;
        public bool isActive;
        public string[] rewards;
        public string[] nextQuests;
    }
    
    [Header("Configuración")]
    [SerializeField] private List<Quest> availableQuests = new List<Quest>();
    [SerializeField] private bool autoActivateNextQuests = true;
    
    [Header("Referencias")]
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private HorrorEventManager eventManager;
    
    private List<Quest> activeQuests = new List<Quest>();
    private List<Quest> completedQuests = new List<Quest>();
    
    private void Start()
    {
        if (inventoryManager == null)
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
        }
        
        if (eventManager == null)
        {
            eventManager = FindObjectOfType<HorrorEventManager>();
        }
        
        // Activar misiones iniciales
        foreach (Quest quest in availableQuests)
        {
            if (quest.isActive)
            {
                ActivateQuest(quest);
            }
        }
    }
    
    public void ActivateQuest(Quest quest)
    {
        if (!quest.isActive && !quest.isCompleted)
        {
            quest.isActive = true;
            activeQuests.Add(quest);
            Debug.Log($"Misión activada: {quest.questName}");
        }
    }
    
    public void CompleteQuest(string questName)
    {
        Quest quest = activeQuests.Find(q => q.questName == questName);
        if (quest != null && !quest.isCompleted)
        {
            quest.isCompleted = true;
            quest.isActive = false;
            activeQuests.Remove(quest);
            completedQuests.Add(quest);
            
            // Activar misiones siguientes
            if (autoActivateNextQuests && quest.nextQuests != null)
            {
                foreach (string nextQuestName in quest.nextQuests)
                {
                    Quest nextQuest = availableQuests.Find(q => q.questName == nextQuestName);
                    if (nextQuest != null)
                    {
                        ActivateQuest(nextQuest);
                    }
                }
            }
            
            Debug.Log($"Misión completada: {quest.questName}");
        }
    }
    
    public void UpdateQuestProgress(string questName)
    {
        Quest quest = activeQuests.Find(q => q.questName == questName);
        if (quest != null)
        {
            bool allObjectivesCompleted = true;
            
            foreach (QuestObjective objective in quest.objectives)
            {
                if (!objective.isCompleted)
                {
                    bool objectiveCompleted = CheckObjectiveCompletion(objective);
                    objective.isCompleted = objectiveCompleted;
                    
                    if (!objectiveCompleted)
                    {
                        allObjectivesCompleted = false;
                    }
                }
            }
            
            if (allObjectivesCompleted)
            {
                CompleteQuest(questName);
            }
        }
    }
    
    private bool CheckObjectiveCompletion(QuestObjective objective)
    {
        // Verificar objetos relacionados
        if (objective.relatedObjects != null)
        {
            foreach (GameObject obj in objective.relatedObjects)
            {
                if (obj != null && obj.activeInHierarchy)
                {
                    return false;
                }
            }
        }
        
        // Verificar items requeridos
        if (objective.requiredItems != null && objective.requiredItemQuantities != null)
        {
            for (int i = 0; i < objective.requiredItems.Length; i++)
            {
                if (i < objective.requiredItemQuantities.Length)
                {
                    int quantity = inventoryManager.GetItemQuantity(objective.requiredItems[i]);
                    if (quantity < objective.requiredItemQuantities[i])
                    {
                        return false;
                    }
                }
            }
        }
        
        // Verificar eventos requeridos
        if (objective.requiredEvents != null)
        {
            foreach (string eventName in objective.requiredEvents)
            {
                // if (!eventManager.IsEventTriggered(eventName))
                // {
                //     return false;
                // }
            }
        }
        
        // Verificar misiones requeridas
        if (objective.requiredQuests != null)
        {
            foreach (string questName in objective.requiredQuests)
            {
                Quest requiredQuest = completedQuests.Find(q => q.questName == questName);
                if (requiredQuest == null)
                {
                    return false;
                }
            }
        }
        
        return true;
    }
    
    public List<Quest> GetActiveQuests()
    {
        return activeQuests;
    }
    
    public List<Quest> GetCompletedQuests()
    {
        return completedQuests;
    }
    
    public Quest GetQuestByName(string questName)
    {
        return availableQuests.Find(q => q.questName == questName);
    }
    
    public bool IsQuestActive(string questName)
    {
        return activeQuests.Exists(q => q.questName == questName);
    }
    
    public bool IsQuestCompleted(string questName)
    {
        return completedQuests.Exists(q => q.questName == questName);
    }
    
    public void ResetQuests()
    {
        activeQuests.Clear();
        completedQuests.Clear();
        
        foreach (Quest quest in availableQuests)
        {
            quest.isActive = false;
            quest.isCompleted = false;
            
            foreach (QuestObjective objective in quest.objectives)
            {
                objective.isCompleted = false;
            }
        }
    }
} 