import sys
from match import Match
from math import inf as infinity
import time
import numpy as np

global PLAYER, x, y, shift
PLAYER = 0
x = 0
y = 0
shift = 0

class Player:
    def __init__(self):
        self.move_history = []
        self.name = None

    def set_move_history(self, move):
        self.move_history.append(move)

class Player(Player):
    def __init__(self):
        super().__init__()

        self.name = 'Burcin Aydogmus-Kaan Gurpinar'

    def decide(self, game, state, available_moves, opponent_moves, queue):
        ai_turn(game, state)
        queue.put([x, y, shift])
        #return [x, y, shift]


def evaluate(state):
    score = 0
    if state['winner'] is not None:
        if PLAYER == 1:
            score += 100
        else:
            score -= 100
    else:
        score += 0
    return score


def change_player():
    global PLAYER
    if PLAYER == 0:
        PLAYER = 1
    else:
        PLAYER = 0


def minimax(game, state, depth):
    global x, y, shift, PLAYER, board
    if depth == 0 or state['winner'] is not None:
        return evaluate(state), None

    moves = [m['space'] for m in game.get_moves(state)[1]]
    PLAYER = state['current_player']
    if PLAYER == 1:
        np.random.shuffle(moves)
        best_score = infinity
        best_move = None
        for i in moves:
            mov = game.create_move(state, i[0], i[1], i[2])
            board = game.apply_move(state, mov)
            score = minimax(game, board, depth - 1)[0]
            if score < best_score:
                best_score = score
                best_move = [mov['space'][0], mov['space'][1], mov['space'][2]]
        return best_score, best_move

    else:
        np.random.shuffle(moves)
        best_score = -infinity
        best_move = None
        for i in moves:
            mov = game.create_move(state, i[0], i[1], i[2])
            board = game.apply_move(state, mov)
            score = minimax(game, board, depth - 1)[0]
            if score > best_score:
                best_score = score
                best_move = [mov['space'][0], mov['space'][1], mov['space'][2]]
        return best_score, best_move


def ai_turn(game, state):
    global x, y, shift, move
    depth = 3
    move = minimax(game, state, depth)[1]
    print(move)
    x, y, shift = move[0], move[1], move[2]
