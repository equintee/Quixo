using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class QLearningAgent : IAgent
{
    private Dictionary<string, float> qValues = new Dictionary<string, float>();
    public QLearningAgent(){
        string json = File.ReadAllText("qValues.json");
        Dictionary<string, float> qValues = JsonConvert.DeserializeObject<Dictionary<string, float>>(json);
    }
    public int[] MakeMove(int[,] board)
    {
        string gameState = GetKey(board);
        string bestMove = qValues.OrderByDescending(kvp => kvp.Value)
            .Where(kvp => long.Parse(kvp.Key.Substring(0, kvp.Key.Length - 3)) == long.Parse(gameState))
            .Select(kvp => kvp.Key.Substring(kvp.Key.Length - 3)).First();

        int[] decision = {int.Parse(bestMove[0].ToString()),int.Parse(bestMove[1].ToString()), int.Parse(bestMove[2].ToString())};
        return decision;
    }


    private string GetKey(int[,] state)
    {
        string key = "";
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                key += state[i,j].ToString();
            }
        }

        return key;
    }

}
