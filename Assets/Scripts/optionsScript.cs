using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class optionsScript : MonoBehaviour
{
    public GameObject AgentSelectionUI;
    public void SelectAgent(int agentNumber){
        GamePlayDataHolder.Instance.selectedAgent = agentNumber;
        AgentSelectionUI.SetActive(false);
    }

    public void ShowAgentSelectionUI(){
        AgentSelectionUI.SetActive(true);
    }
    
        
}
