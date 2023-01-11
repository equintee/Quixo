using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayDataHolder : MonoBehaviour
{
    public static GamePlayDataHolder Instance { get; private set; }
    public int[,] gameState;
    public int[] suggestedCube;
    public int move;
    public bool AISuggestedMove = false;
    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
        
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
    }

    public void SetSuggestion(int[,] gameState, int[] suggestedCube, int move){
        this.gameState = gameState;
        this.suggestedCube = suggestedCube;
        this.move = move;
        AISuggestedMove = true;
    }
}
