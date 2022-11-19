using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

public class minMax
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
    //CUBE VALUES = 0:X 1:O 2:empty
    public minMax(int symbol, int[,] board){
        AISymbol = symbol;
        opponentSymbol = AISymbol == 0 ? 1 : 0;
        this.board = board;
    }

    public int[] MiniMax(int[,] board, int depth){
        Dictionary<int[], int> scores = new Dictionary<int[], int>();
        foreach(int[] avaliableMove in AvaliableMoves(board, AISymbol))
            scores[avaliableMove] = MiniMax(board, depth, true);
        
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
        
        if(depth == 0)
            return(EvaluateBoard(board, AISymbol));
        
        List<int> scores = new List<int>();
        depth--;

        if(maximazingPlayer){
            foreach(int[] avaliableMove in AvaliableMoves(board, AISymbol)){
                int[,] childBoard = MakeMove((int[,]) board.Clone(), avaliableMove, AISymbol);
                scores.Append(MiniMax(childBoard, depth, false));
            }
        }
        else{
            foreach(int[] avaliableMove in AvaliableMoves(board, opponentSymbol)){
                int[,] childBoard = MakeMove((int[,]) board.Clone(), avaliableMove, opponentSymbol);
                scores.Append(MiniMax(childBoard, depth, true));
            }
        }

        return maximazingPlayer ? scores.Max() : scores.Min();
    }

    private int EvaluateBoard(int[,] board, int AISymbol){
        int boardValue = 0;

        for(int i = 0; i < board.GetLength(0); i++)
            for(int j = 0; j < board.GetLength(1); j++){
                if(board[i,j] == AISymbol){
                    boardValue += cubePositionRewards[i,j];
                    continue;
                }
                else if(board[i,j] == 2)
                    continue;
                else
                    boardValue -= board[i,j];
            }
        
        return boardValue;
    }

    
    private List<int[]> AvaliableMoves(int[,] board, int turnSymbol){
        List<int[]> avaliablePieces = new List<int[]>();
        List<int[]> avaliableMoves = new List<int[]>(); 
        for(int j = 0; j < 5; j++){
            //Top row check
            if(board[0,j] == turnSymbol && board[0,j] == 2)
                avaliablePieces.Add(new int[] {0,j});

            // Bottom row check
            if(board[4,j] == turnSymbol && board[4,j] == 2)
                avaliablePieces.Add(new int[] {4,j}); 
        }

        for(int i = 1; i < 4; i++){
            //Left side check
            if(board[i,0] == turnSymbol && board[i,0] == 2)
                avaliablePieces.Add(new int[] {i,0});
            
            //Right side check
            if(board[i,4] == turnSymbol && board[i,4] == 2)
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
                break;
            
            case 1:
                for(int i = move[0] - 1; -1 > i; i--)
                    board[i + 1, move[1]] = board[i, move[1]];
                break;
            
            case 2:
                for(int i = move[1] - 1; i > -1; i--)
                    board[move[0], i + 1] = board[move[0], i];
                break;
            
            case 3:
                for(int i = move[1] + 1; 5 > i; i++)
                    board[move[0], i - 1]  = board[move[0], i];
                break;
        }
        
        board[move[0], move[1]] = symbol;
        return board;
    }
}
