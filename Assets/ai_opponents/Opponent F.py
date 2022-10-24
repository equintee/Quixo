# -*- coding: UTF-8 -*-

from math import inf as infinity
from random import choice
import copy
import sys

class Player:
    def __init__(self):
        self.move_history = []
        self.name = None

    def set_move_history(self, move):
        self.move_history.append(move)


class Player(Player):
    def __init__(self):
        super().__init__()

        self.name = 'Uluc-sena-code' #defining the instance variable 'name'

    def empty_cells(self, state):
        cells = [] #cells is an empty list

        for x, row in enumerate(state.board): #0 row[0] \n 1 row[1] \n ... x row[x]
            for y, cell in enumerate(row): #0 cell[0] \n 1 cell[1] \n ... y cell[y]
                if cell == 0: cells.append([x, y])
        return cells

        """

                                 0 1 ... y           0     1         y
                    0 row0  =  0 a b ... c   =   0 cell0 cell1 ... celly
                    1 row1  =  1 d e ... f   =   1 cell0 cell1 ... celly
                    ...        ...           =   ...
                    x rowx  =  x k l ... m   =   x cell0 cell1 ... celly
        """

    def game_over(self,board):
        return self.wins(-1,board) or self.wins(1, board)
        #return whomever is the winner(-1 or 1) & the final state of the board

    def evaluate(self, board):
        if self.wins(1,board):
            score = +1
        elif self.wins(-1,board):
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
        if player == 1:
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
            newMoves = game.get_moves(newState) #newMoves(new available moves) are created according to the newState

            score = self.minimax(game, newState, newMoves, opponent_moves, depth - 1, -player)
            score[0], score[1], score[2] = x, y, shift

            if player == 1: #my AI player
                if score[3] > best[3]:
                    best = score    # max value
            else: #opponent player
                if score[3] < best[3]:
                    best = score    # min value

        return best

    def isCloseCinko(game_board,enemy):
        for i in range(5):
            count = 0
            emty = -1
            for j in range(5):
                if game_board[i][j] == enemy:
                    count += 1
                else:
                    emty = game_board[i][j]
            if count >= 3:
                return 'Y',i,emty

        for j in range(5):
            count = 0
            emty = -1
            for i in range(5):
                if game_board[i][j] == enemy:
                    count += 1
                else:
                    emty = game_board[i][j]
            if count >= 3:
                return 'D',i,emty
        return 'N',None,None

    def decide(self, game, state, available_moves, opponent_moves, queue):

        depth = len(available_moves)
        stateCopy = copy.deepcopy(state)
        player = state.current_player
        print("DEPTH:", depth)
        current_player = state.current_player
        if current_player == 1:
            enemy = -1
        else:
            enemy = 1

        if depth > 300 and self.isCloseCinko(enemy)[0]=='N':
            print("random player for a while")
            move = choice(available_moves)
            queue.put([move.row, move.column, move.shift])
            #return [move.row, move.column, move.shift]
        else:
            print("minmax player starts")
            move = self.minimax(game, stateCopy, available_moves, opponent_moves, 2, player)
            queue.put([move[0], move[1], move[2]])
            #return [move[0], move[1], move[2]]




