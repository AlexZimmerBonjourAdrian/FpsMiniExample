using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CLevelDialogue : CLevelGeneric
{

   [SerializeField] private TextMeshProUGUI SanityText;
   [SerializeField] private TextMeshProUGUI EmpatyText;
   [SerializeField] private TextMeshProUGUI CharmText;
   [SerializeField] private TextMeshProUGUI WitsText;
   [SerializeField] private TextMeshProUGUI ComposureText;
   [SerializeField] private TextMeshProUGUI NameArquetipe;


   void Start()   
   {

      
       CMICILSPSystem.Instance.ApplyTemplate(CMICILSPSystem.Instance.Detective); 
       
        CMICILSPSystem.Instance.PrintStats(CMICILSPSystem.Instance.CurrentStatsTemplate);


 
       CMICILSPSystem.Instance.GetStat(CMICILSPSystem.Stats.Sanity).ToString();
       
 

   }

   private void Update()
   {
      if(CEngineManager.Inst.GetIsDebug())
      {
      if(Input.GetKeyDown(KeyCode.E))
      {

         CMICILSPSystem.Instance.IncreaseStat(CMICILSPSystem.Stats.Charm, 2);
      }

      
      if(Input.GetKeyDown(KeyCode.Q))
      {

         CMICILSPSystem.Instance.DecreaseStat(CMICILSPSystem.Stats.Charm, 2);
      }

      //set random arquetipe by pressed Button R
      if(Input.GetKeyDown(KeyCode.R))
      {
         CMICILSPSystem.Instance.ApplyTemplate(CMICILSPSystem.Instance.GetRandomTemplate()); 
      }
   }

      if(EmpatyText != null && SanityText != null && CharmText != null && WitsText != null && ComposureText != null)
       {
          NameArquetipe.text = CMICILSPSystem.Instance.CurrentStatsTemplate.Name;
         SanityText.text = CMICILSPSystem.Instance.GetStat(CMICILSPSystem.Stats.Sanity).ToString();
         EmpatyText.text = CMICILSPSystem.Instance.GetStat(CMICILSPSystem.Stats.Empathy).ToString();
         CharmText.text= CMICILSPSystem.Instance.GetStat(CMICILSPSystem.Stats.Charm).ToString();
         WitsText.text= CMICILSPSystem.Instance.GetStat(CMICILSPSystem.Stats.Wits).ToString();
         ComposureText.text= CMICILSPSystem.Instance.GetStat(CMICILSPSystem.Stats.Composure).ToString();
        
       }
      else
      {
         Debug.LogError("Hay una stat no cargada");
      }
  
   }
}
   
