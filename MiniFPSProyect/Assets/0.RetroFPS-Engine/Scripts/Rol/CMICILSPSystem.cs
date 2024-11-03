using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Systema de rol template
public class CMICILSPSystem : MonoBehaviour
{
    //SINGLETON
    private static CMICILSPSystem _instance;

    public static CMICILSPSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("CMICILSPSystem");
                _instance = obj.AddComponent<CMICILSPSystem>();
                DontDestroyOnLoad(obj); 
            }
            return _instance;
        }
    }

        public StatTemplate CurrentStatsTemplate { get; private set; } 

    public StatTemplate GetStatTemplate()
    {
        return CurrentStatsTemplate;
    }

    // Start is called before the first frame update}
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public enum Stats
    {
        Sanity,
        Charm,
        Wits,
        Composure,
        Empathy
    }

     private Dictionary<Stats, int> currentStats = new Dictionary<Stats, int>()
    {
        { Stats.Sanity, 5 },
        { Stats.Charm, 5 },
        { Stats.Wits, 5 },
        { Stats.Composure, 5 },
        { Stats.Empathy, 5 }
    };


    // Constructor para inicializar los atributos (opcional)
    public void InitializeStats(int sa = 5, int ch = 5, int wi = 5, int wil = 5, int em = 5)
    {
        SetStat(Stats.Sanity, sa);
        SetStat(Stats.Charm, ch);
        SetStat(Stats.Wits, wi);
        SetStat(Stats.Composure, wil);
        SetStat(Stats.Empathy, em);
    }

    // [YarnParameter]
    //   [YarnAction]
    //     [YarnFunction]
    //       [YarnNode]
    //         [YarnStateInjector]
    //           [YarnCommand]

           
   public virtual int GetStat(Stats stat)
    {
        return currentStats[stat];
    }

   // [YarnFunction("GetStatByName")]
   //[YarnParameter("statName")] 
    public  virtual  int GetStatByName(string statName)
    {
        // Convertir el nombre de la estadística a su valor enum
        if (System.Enum.TryParse<Stats>(statName, out Stats stat))
        {
            return currentStats[stat];
        }
        else
        {
            Debug.LogError("Stat no encontrada: " + statName);
            return -1; // O cualquier otro valor que indique un error
        }
    }


  // [YarnParameter("statIndex")] 
 
public virtual  int GetStatByIndex(int statIndex)
{
    // Asegurarse de que el índice esté dentro del rango válido
    if (statIndex >= 0 && statIndex < System.Enum.GetValues(typeof(Stats)).Length)
    {
        return currentStats[(Stats)statIndex];
    }
    else
    {
        Debug.LogError("Índice de stat inválido: " + statIndex);
        return -1; 
    }
}

    public virtual  void PrintStats(StatTemplate template)
    {
        Debug.Log("============================");
        Debug.Log("Stats for " + template.Name + ":");

        foreach (var stat in template.BaseStats)
        {
            Debug.Log(stat.Key + ": " + stat.Value);
        }


    }
    public virtual  void SetStat(Stats stat, int value)
    {
        currentStats[stat] = Mathf.Clamp(value, 1, 10);
    }

    public virtual  void IncreaseStat(Stats stat, int amount)
    {
       currentStats[stat] = currentStats[stat] + amount; 
       
    }

    public virtual  void DecreaseStat(Stats stat, int amount)
    {
        SetStat(stat, currentStats[stat] - amount);
    }

    public  class StatTemplate
    {
        public string Name;
        public Dictionary<Stats, int> BaseStats;

        public StatTemplate(string name, Dictionary<Stats, int> baseStats)
        {
            Name = name;
            BaseStats = baseStats;
        }
    }

    // Plantillas de atributos
    public StatTemplate Detective = new StatTemplate("Detective", new Dictionary<Stats, int>() {
        { Stats.Sanity, 7 },
        { Stats.Charm, 5 },
        { Stats.Wits, 8 },
        { Stats.Composure, 6 },
        { Stats.Empathy, 4 }
    });

    public StatTemplate NinaMimada = new StatTemplate("NiñaMimada", new Dictionary<Stats, int>() {
        { Stats.Sanity, 6 },
        { Stats.Charm, 7 },
        { Stats.Wits, 4 },
        { Stats.Composure, 4 },
        { Stats.Empathy, 6 }
    });

    public StatTemplate HeroinaDeCapaBlanca = new StatTemplate("HeroínadeCapa Blanca", new Dictionary<Stats, int>() {
        { Stats.Sanity, 8 },
        { Stats.Charm, 8 },
        { Stats.Wits, 6 },
        { Stats.Composure, 7 },
        { Stats.Empathy, 8 }
    });

    public StatTemplate LenguaDePlata = new StatTemplate("LenguadePlata", new Dictionary<Stats, int>() {
        { Stats.Sanity, 6 },
        { Stats.Charm, 9 },
        { Stats.Wits, 7 },
        { Stats.Composure, 5 },
        { Stats.Empathy, 6 }
    });

    public StatTemplate FemmeFatale = new StatTemplate("FemmeFatale", new Dictionary<Stats, int>() {
        { Stats.Sanity, 6 },
        { Stats.Charm, 9 },
        { Stats.Wits, 7 },
        { Stats.Composure, 6 },
        { Stats.Empathy, 5 }
    });

    public StatTemplate MonstruoSinCorazon = new StatTemplate("MonstruoSinCorazón", new Dictionary<Stats, int>() {
        { Stats.Sanity, 2 },
        { Stats.Charm, 3 },
        { Stats.Wits, 6 },
        { Stats.Composure, 8 },
        { Stats.Empathy, 2 }
    });

    public StatTemplate LocaPerturbada = new StatTemplate("LocaPerturbada", new Dictionary<Stats, int>() {
        { Stats.Sanity, 1 },
        { Stats.Charm, 4 },
        { Stats.Wits, 7 },
        { Stats.Composure, 3 },
        { Stats.Empathy, 3 }
    });

    public StatTemplate HijaDePolitico = new StatTemplate("HijadePolítico", new Dictionary<Stats, int>() {
        { Stats.Sanity, 7 },
        { Stats.Charm, 8 },
        { Stats.Wits, 6 },
        { Stats.Composure, 7 },
        { Stats.Empathy, 4 }
    });

     // Update ApplyTemplate to set CurrentStatsTemplate and initialize currentStats:
    public virtual  void ApplyTemplate(StatTemplate template)
    {
        CurrentStatsTemplate = template; // Store the active template

        // Initialize currentStats based on the template
        foreach (var stat in template.BaseStats)
        {
            currentStats[stat.Key] = stat.Value; 
        }
    }

    public virtual  StatTemplate GetRandomTemplate()
{
    // Array with all your templates
    StatTemplate[] templates = new StatTemplate[] { 
        Detective, NinaMimada, HeroinaDeCapaBlanca, 
        LenguaDePlata, FemmeFatale, MonstruoSinCorazon, 
        LocaPerturbada, HijaDePolitico 
    };

    int randomIndex = Random.Range(0, templates.Length);
    return templates[randomIndex];
}
    
}


