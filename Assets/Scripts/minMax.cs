using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

public class minMax : IAgent
{
    private int[,] cubePositionRewards = new int[,]
    {
        {2,3,3,3,2},
        {3,1,1,1,3},
        {3,1,1,1,3},
        {3,1,1,1,3},
        {2,3,3,3,2}
    };

    private int AISymbol;
    private int opponentSymbol;
    private int[,] board;
    public int depth;
    //CUBE VALUES = 0:X 1:O 2:empty
    public minMax(int symbol, int depth){
        AISymbol = symbol;
        opponentSymbol = AISymbol == 0 ? 1 : 0;
        this.depth = depth;
    }

    public int[] MakeMove(int[,] board){
        Dictionary<int[], int> scores = new Dictionary<int[], int>();
        depth--;
        foreach(int[] avaliableMove in AvaliableMoves(board, AISymbol)){
            int[,] childBoard = MakeMove((int[,]) board.Clone(), avaliableMove, AISymbol);
            scores.Add(avaliableMove,MiniMax(childBoard, depth, false));
        }
            
        
        int topScore = int.MinValue;
        int[] topMove = new int[3];

        foreach(KeyValuePair<int[], int> moveScore in scores){
            if(moveScore.Value > topScore){
                topScore = moveScore.Value;
                topMove = moveScore.Key;
            }
        }
        return topMove;
    }
    public int MiniMax(int[,] board, int depth, bool maximazingPlayer){
        
        depth--;
        if(depth == 0)
            return(EvaluateBoard(board, AISymbol, depth));
        
        int winner = DetermineWinner(board);
        if(winner != -1)
            return winner == AISymbol ? 1000 + depth: -1000 + depth;
        
        List<int> scores = new List<int>();

        if(maximazingPlayer){
            foreach(int[] avaliableMove in AvaliableMoves(board, AISymbol)){
                int[,] childBoard = MakeMove((int[,]) board.Clone(), avaliableMove, AISymbol);
                int score = MiniMax(childBoard, depth, true);
                scores.Add(score);
            }
        }
        else{
            foreach(int[] avaliableMove in AvaliableMoves(board, opponentSymbol)){
                int[,] childBoard = MakeMove((int[,]) board.Clone(), avaliableMove, opponentSymbol);
                int score = MiniMax(childBoard, depth, true);
                scores.Add(score);
            }
        }

        return maximazingPlayer ? scores.Max() : scores.Min();
    }

    private int EvaluateBoard(int[,] board, int AISymbol, int depth){
        int boardValue = 0;
        int winner = DetermineWinner(board);
        if(winner != -1)
            return winner == AISymbol ? 1000 + depth: -1000 + depth;

        for(int i = 0; i < board.GetLength(0); i++)
            for(int j = 0; j < board.GetLength(1); j++){
                if(board[i,j] == AISymbol){
                    boardValue += cubePositionRewards[i,j];
                    continue;
                }
                else if(board[i,j] == 2)
                    continue;
                else
                    boardValue -= cubePositionRewards[i,j];
            }
        
        return boardValue;
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
