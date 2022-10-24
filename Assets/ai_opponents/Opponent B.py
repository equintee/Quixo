import sys
from player import Player
import numpy as np
import math
from copy import deepcopy

class Player:
    def __init__(self):
        self.move_history = []
        self.name = None

    def set_move_history(self, move):
        self.move_history.append(move)

class Player(Player):
    GOD = [(5,0),(4,18),(3,22),(2,32),(1,37)]
    GOOD_PLAYER = [(4, 0),(3, 18), (2, 30), (1, 37)]
    MEDIUM_PLAYER = [(2, 0), (1, 38)]
    DEPTH_4 = [(4,0),(2,29),(1,34)]

    def __init__(self):
        super().__init__()

        self.name = 'quixo_god'

    def decide(self, game, state, available_moves, opponent_moves, queue):
        # available_moves[] -> [{['player'],['space']}]
        # space -> [x=0,y=0,z=0]
        move = getBestMove(game, state, available_moves, self.GOOD_PLAYER)
        queue.put(move)
        #return move


def alphabeta(game, state, alpha, beta, depth):

    isTerminal = state['winner']
    if (isTerminal is not None or depth == 0):  # Return board value if we hit depth limit
        score = evaluate(state, depth)
        return (score, None)

    depth -= 1

    bestMove = None
    currentPlayer = state['current_player']
    possibleMoves = [d['space'] for d in game.get_moves(state)[1]]
    if (currentPlayer == 1):
        np.random.shuffle(possibleMoves)
        for move in possibleMoves:
            d = game.create_move(state, move[0], move[1], move[2])
            s = game.apply_move(state, d)
            val = alphabeta(game, s, alpha, beta, depth)[0]
            if (val > alpha):
                alpha = val
                bestMove = move
            if (alpha >= beta):
                break
        return (alpha, bestMove)
    else:
        for move in possibleMoves:
            d = game.create_move(state, move[0], move[1], move[2])
            s = game.apply_move(state, d)
            val = alphabeta(game, s, alpha, beta, depth)[0]
            if (val < beta):
                beta = val
                bestMove = move
            if (alpha >= beta):
                break
        return (beta, bestMove)


def find_occurence(state, key):
    occCount = 0
    for r in range(0, 5):
        row = state[r, :]
        col = state[:, r]
        y = np.where(row == key)[0]
        z = np.where(col == key)[0]
        if (len(y) == 4):
            ok = (y[-1] - y[0] == len(y) - 1)
            if (ok):
                occCount += 1
        if (len(z) == 4):
            ok = (z[-1] - z[0] == len(z) - 1)
            if (ok):
                occCount += 1
    return occCount

# Evaluates the current game state
def evaluate(state, depth):
    isTerminal = state['winner']
    if (isTerminal == 'o'):  # Maximizer won (O = 1)
        return 100 + depth  # Less moves is better
    elif (isTerminal == 'x'):  # Minimizer Won (X = -1)
        return -100 - depth  # Less moves is better
    elif (isTerminal == 'Draw'):
        return 0
    else:
        currentPlayer = state['current_player']
        board = state['board']

        value = 0
        occValue = find_occurence(board,currentPlayer) * 20

        if(board[2, 2] == 1):
            value += 15
        elif(board[2,2] == -1):
            value -= 15

        if(currentPlayer == -1):
            occValue *= -1

        frequency = np.unique(board, return_counts=True)
        uniqueCount = len(frequency[0])
        if (uniqueCount == 3):  # Contains blank pieces
            minimizerPieceCount = frequency[1][0]
            maximizerPieceCount = frequency[1][2]
        elif (uniqueCount > 1):
            minimizerPieceCount = frequency[1][0]
            maximizerPieceCount = frequency[1][1]
        else:
            return 0
        value = (maximizerPieceCount - minimizerPieceCount) + occValue
        return value

# Return the best move using alphabeta search
def getBestMove(game,state,available_moves, depthLevels):
    possibleMoveCount = len(available_moves)
    # Iterative deepening based on possible move count
    depth = 0
    for depthLvl in depthLevels:
        if (possibleMoveCount > depthLvl[1]):
            depth = depthLvl[0]
        else:
            break

    move = alphabeta(game, state, -math.inf, math.inf, depth)[1]

    return move
