using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class QLearningTrain
{
        // Declare constants for the learning rate, discount factor, and exploration rate
    private const float learningRate = 0.1f;
    private const float discountFactor = 0.9f;
    private const float explorationRate = 0.1f;
    private int AISymbol = 1;
    // Declare a lookup table for the Q-values of each state-action pair
    private Dictionary<string, float> qValues = new Dictionary<string, float>();
    private minMax miniMaxAgent = new minMax(0, 2);

    // Train the agent by playing a series of games and updating the Q-values
    public void Train(int numGames)
    {
        // Initialize a random number generator
        var rng = new System.Random();

        // Play the specified number of games
        for (int i = 0; i < numGames; i++)
        {
            // Initialize the game state
            int[,] board = new int[5,5];
            for (int k = 0; k < board.GetLength(0); k++)
                for (int l = 0; l < board.GetLength(1); l++)
                    board[k, l] = 2;

            // Play the game until it's over
            while ((DetermineWinner(board) == -1))
            {
                int opponentSymbol = 1;
                List<int[]> oponnentMoves = AvaliableMoves(board, opponentSymbol);
                int[] opponentMove = oponnentMoves[rng.Next(0, oponnentMoves.Count)];
                int[,] nextBoard = MakeMove(board, opponentMove, AISymbol);

                List<int[]> avaliableMoves = AvaliableMoves(nextBoard, AISymbol);
                int[] action = ChooseAction(board, rng);

                // Take the action and observe the next state and reward
                nextBoard = MakeMove(board, action, AISymbol);
                float reward = GetReward(nextBoard);

                // Update the Q-value for the state-action pair
                UpdateQValue(board, action, reward, nextBoard);

                // Set the state to the next state
                board = nextBoard;
            }
        }

        SaveQTable("QLearningTable.json");
    }

    private int[] ChooseAction(int[,] board, System.Random rng)
    {
        // With probability epsilon, choose a random action
        if (rng.NextDouble() < explorationRate)
        {
            return AvaliableMoves(board, 2)[rng.Next(AvaliableMoves(board, 2).Count)];
        }
        // Otherwise, choose the action with the highest Q-value
        else
        {
            return GetBestAction(board);
        }
    }

    // Get the action with the highest Q-value for a given state
    private int[] GetBestAction(int[,] board)
    {
        float maxQValue = float.MinValue;
        int[] bestAction = null;

        foreach (int[] action in AvaliableMoves(board, 2))
        {
            float qValue = GetQValue(board, action);
            if (qValue > maxQValue)
            {
                maxQValue = qValue;
                bestAction = action;
            }
        }
        return bestAction;
    }

    private float GetReward(int[,] board)
    {
        return miniMaxAgent.MiniMax(board, 2, false);
    }
    // Update the Q-value for a state-action pair using the Q-learning algorithm
    private void UpdateQValue(int[,] state, int[] action, float reward, int[,] nextState)
    {
        float qValue = GetQValue(state, action);
        float maxNextQValue = GetMaxQValue(nextState);
        qValue = qValue + learningRate * (reward + discountFactor * maxNextQValue - qValue);
        SetQValue(state, action, qValue);
    }

    // Get the Q-value for a state-action pair
    private float GetQValue(int[,] state, int[] action)
    {
        string key = GetKey(state, action);
        if (qValues.ContainsKey(key))
        {
            return qValues[key];
        }
        else
        {
            return 0;
        }
    }

    // Set the Q-value for a state-action pair
    private void SetQValue(int[,] state, int[] action, float qValue)
    {
        string key = GetKey(state, action);
        qValues[key] = qValue;
    }

    // Get the maximum Q-value for a given state
    private float GetMaxQValue(int[,] state)
    {
        float maxQValue = float.MinValue;
        foreach (int[] action in AvaliableMoves(state, 2))
        {
            float qValue = GetQValue(state, action);
            if (qValue > maxQValue)
            {
                maxQValue = qValue;
            }
        }
        return maxQValue;
    }
    // Generate a unique key for a state-action pair
    private string GetKey(int[,] state, int[] action)
    {
        string key = "";
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                key += state[i,j].ToString();
            }
        }
        key += action[0].ToString() + action[1].ToString() + action[2].ToString();
        return key;
    }

    

    // Save the Q-table to a JSON file
    public void SaveQTable(string filePath)
    {
        string json = JsonConvert.SerializeObject(qValues);
        File.WriteAllText(filePath, json);
    }


    public int DetermineWinner(int[,] board){
        //Detirmine left diagonal winner
        for(int i = 0; i < 4; i++){
            if(board[i,i] != board[i+1,i+1] || board[i,i] == 2)
                break;
            if(i==3)
                return board[i,i];
        }

        //Detirmine right diagonal winner
        int j = 4;
        for(int i=0; i < 4; i++){
            if(board[i,j] != board[i + 1, j-1] || board[i,j] == 2)
                break;
            j--;
            if(i==3)
                return board[i + 1,j];
        }

        //Detirmine horizontal winner
        for(int i = 0; i < 5; i++){
            for(j = 0; j < 4; j++){
                if(board[i,j] != board[i,j + 1] || board[i,j] == 2)
                    break;

                if(j == 3)
                    return board[i,j];
            }
        }

        //Detirmine vertical winner
        for(int i = 0; i < 5; i++){
            for(j = 0; j < 4; j++){
                if(board[j, i] != board[j+1, i] || board[j,i] == 2)
                    break;
                    
                if(j == 3)
                    return board[j, i];
            }
        }

        return -1;
    }
    
    private List<int[]> AvaliableMoves(int[,] board, int turnSymbol){
        List<int[]> avaliablePieces = new List<int[]>();
        List<int[]> avaliableMoves = new List<int[]>(); 
        for(int j = 0; j < 5; j++){
            //Top row check
            if(board[0,j] == turnSymbol || board[0,j] == 2)
                avaliablePieces.Add(new int[] {0,j});

            // Bottom row check
            if(board[4,j] == turnSymbol || board[4,j] == 2)
                avaliablePieces.Add(new int[] {4,j}); 
        }

        for(int i = 1; i < 4; i++){
            //Left side check
            if(board[i,0] == turnSymbol || board[i,0] == 2)
                avaliablePieces.Add(new int[] {i,0});
            
            //Right side check
            if(board[i,4] == turnSymbol || board[i,4] == 2)
                avaliablePieces.Add(new int[] {i,4});
        }

        // 0:down, 1:up, 2:left, 3:right
        foreach(int[] avaliablePiece in avaliablePieces){
            //Check if piece can move right.
            if(avaliablePiece[1] != 4)
                avaliableMoves.Add(new int[] {avaliablePiece[0], avaliablePiece[1], 3});
            
            //Check if piece can move left.
            if(avaliablePiece[1] != 0)
                avaliableMoves.Add(new int[] {avaliablePiece[0], avaliablePiece[1], 2});

            //Check if piece can move up.
            if(avaliablePiece[0] != 0)
                avaliableMoves.Add(new int[] {avaliablePiece[0], avaliablePiece[1], 1});

            //Check if piece can move down.
            if(avaliablePiece[0] != 4)
                avaliableMoves.Add(new int [] {avaliablePiece[0], avaliablePiece[1], 0});
        }

        return avaliableMoves;
    }
    
    private int[,] MakeMove(int[,] board, int[] move, int symbol){
        // 0:down, 1:up, 2:left, 3:right
        switch(move[2])
        {
            case 0:
                for(int i = move[0] + 1; 5 > i; i++)
                    board[i - 1, move[1]] = board[i,move[1]];
                board[4, move[1]] = symbol;
                break;
            
            case 1:
                for(int i = move[0] - 1; -1 > i; i--)
                    board[i + 1, move[1]] = board[i, move[1]];
                board[0, move[1]] = symbol;
                break;
            
            case 2:
                for(int i = move[1] - 1; i > -1; i--)
                    board[move[0], i + 1] = board[move[0], i];
                board[move[0], 0] = symbol;
                break;
            
            case 3:
                for(int i = move[1] + 1; 5 > i; i++)
                    board[move[0], i - 1]  = board[move[0], i];
                board[move[0], 4] = symbol;
                break;
        }
        
        return board;
    }
}
