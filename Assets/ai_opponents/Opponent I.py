# -*- coding: UTF-8 -*-

"""
    @author = "İlkay Tevfik Devran"
    @contact = "devrani@mef.edu.tr"
    @date = "17.01.2019"
    @version = 1.2
"""

from math import inf as infinity
from random import choice
import copy
import sys

# myAI manual
# myAI myAI

class Player:
    def __init__(self):
        self.move_history = []
        self.name = None

    def set_move_history(self, move):
        self.move_history.append(move)


class Player(Player):
    def __init__(self):
        super().__init__()

        self.name = 'DevrAnI'

    def empty_cells(self, state):
        cells = []

        for x, row in enumerate(state.board):
            for y, cell in enumerate(row):
                if cell == 0: cells.append([x, y])
        return cells

    def game_over(self,board):
        return self.wins(-1,board) or self.wins(1, board)

    def evaluate(self, board):
        if self.wins(1,board): # my AI player
            #print(player)
            score = +1
        elif self.wins(-1,board): # opponent player
            score = -1
        else:
            score = 0

        return score

    def wins(self, player, board):
        win_states= [
            [board[0][0], board[0][1], board[0][2], board[0][3], board[0][4]],
            [board[1][0], board[1][1], board[1][2], board[1][3], board[1][4]],
            [board[2][0], board[2][1], board[2][2], board[2][3], board[2][4]],
            [board[3][0], board[3][1], board[3][2], board[3][3], board[3][4]],
            [board[4][0], board[4][1], board[4][2], board[4][3], board[4][4]],
            [board[0][0], board[1][0], board[2][0], board[3][0], board[4][0]],
            [board[0][1], board[1][1], board[2][1], board[3][1], board[4][1]],
            [board[0][2], board[1][2], board[2][2], board[3][2], board[4][2]],
            [board[0][3], board[1][3], board[2][3], board[3][3], board[4][3]],
            [board[0][4], board[1][4], board[2][4], board[3][4], board[4][4]],
            [board[0][0], board[1][1], board[2][2], board[3][3], board[4][4]],
            [board[0][4], board[1][3], board[2][2], board[3][1], board[4][0]],
        ]
        if [player, player, player, player, player] in win_states:
            return True
        else:
            return False

    def minimax(self, game, state, available_moves, opponent_moves, depth, player):
        if player == 1: # if player is my AI
            best = [-1, -1, -1, -infinity]
        else:
            best = [-1, -1, -1, infinity]

        if depth == 0 or self.game_over(state.board):
            score = self.evaluate(state.board)
            return [-1, -1, -1, score]

        for av_move in available_moves:
            x, y, shift  = av_move.row, av_move.column, av_move.shift

            stateCopy = copy.deepcopy(state)
            newState = game.apply_move(stateCopy, av_move)
            newMoves = game.get_moves(newState)

            score = self.minimax(game, newState, newMoves, opponent_moves, depth - 1, -player)
            score[0], score[1], score[2] = x, y, shift

            if player == 1:
                if score[3] > best[3]:
                    best = score    # max value
            else:
                if score[3] < best[3]:
                    best = score    # min value

        return best

    def decide(self, game, state, available_moves, opponent_moves, queue):

        depth = len(available_moves)    # version 1.0
        stateCopy = copy.deepcopy(state)
        player = state.current_player
        print("DEPTH:", depth)
        if depth > 36: # First move play randomly
            print("RANDOMM")
            move = choice(available_moves)
            queue.put([move.row, move.column, move.shift])
            #return [move.row, move.column, move.shift]
        else:
            print("MINMAX")
            move = self.minimax(game, stateCopy, available_moves, opponent_moves, 2, player)    # Depth will may be changed. It's 3 for now.
            queue.put([move[0], move[1], move[2]])
            #return [move[0], move[1], move[2]]
