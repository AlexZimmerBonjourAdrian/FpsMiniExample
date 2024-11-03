using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSwitch : MonoBehaviour
 {
//     public static CSwitch Inst
//     {
//         get
//         {
//             if (_inst == null)
//             {
//                 GameObject obj = new GameObject("Switch");

//                 return obj.AddComponent<CSwitch>();
//             }
//             return _inst;
//         }
//     }
//     private static CSwitch _inst;
//     // Start is called before the first frame update
//     private void Awake()
//     {
//         if (_inst != null && _inst != this)
//         {
//             Destroy(gameObject);
//             return;
//         }
//         DontDestroyOnLoad(this.gameObject);
//         _inst = this;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         //Example Change Map in te Game
//         if(Input.GetKeyDown(KeyCode.Alpha1))
//         {
//             CLevelManager.Inst.LoadScene(0);
//         }
//         else if(Input.GetKeyDown(KeyCode.Alpha2))
//         {
//             CLevelManager.Inst.LoadScene(1);

//         }
//         else if(Input.GetKeyDown(KeyCode.Alpha3))
//         {
//             CLevelManager.Inst.LoadScene(2);
//         }
//         else if(Input.GetKeyDown(KeyCode.Alpha4))
//         {
//             CLevelManager.Inst.LoadScene(3);
//         }
//          else if(Input.GetKeyDown(KeyCode.Alpha4))
//         {
//             CLevelManager.Inst.LoadScene(4);
//         }
     
//     }

//     public void SwitchScene(string name)
//     {

//         CLevelManager.Inst.LoadSceneAsync(name);
//     }
  
    
}
