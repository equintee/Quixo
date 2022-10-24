import sys
import numpy
from random import randint
from math import inf as infinity
from copy import deepcopy
sys.setrecursionlimit(100000000)

class Player:
    def __init__(self):
        self.move_history = []
        self.name = None

    def set_move_history(self, move):
        self.move_history.append(move)


def minimax(state, available_moves, game, depth, last_move, init_b, player):
    current_player = state.current_player
    init_board = state.board
    # print(init_board)
    score =[-1, -1, -1, 0]
    for x in range(20):

        if len(available_moves) > 20:
            if current_player == player:
                best = [-1, -1, -1, -infinity]
            else:
                best = [-1, -1, -1, +infinity]
            #current_move = available_moves[x]

            if available_moves[x].row != last_move[0] and available_moves[x].column != last_move[1] and available_moves[
                x].shift != last_move[2]: # if last move is not available, do next move in the list
                game.apply_move(state, available_moves[x])
                last_move[0] = available_moves[x].row
                last_move[1] = available_moves[x].column
                last_move[2] = available_moves[x].shift
                t = 0

            else: # if last move is availabe in the list, then pick another random move in the list
                index = randint(0, len(available_moves) - 1)
                game.apply_move(state, available_moves[index])
                last_move[0] = available_moves[index].row
                last_move[1] = available_moves[index].column
                last_move[2] = available_moves[index].shift
                t = 1

            player = -player # turn returns to other player
            if depth == 0: # if depth is 0 that means no more move and have to calculate score
                if game.check_for_winner(state.board) == True:
                    if current_player == state.current_player:
                        score[3] = score[3] + 10 # win situation
                    else:
                        score[3] = score[3] - 10 # lose situation
                else:
                    score[3] = score[3] # draw situation

            elif depth != 0: # if depth is not 0, than there must be recursive function
                score = minimax(state, game.get_moves(state), game, depth - 1, last_move, init_b, player)

            if t == 0: # its about our last move and what score will be
                score[0], score[1], score[2] = available_moves[x].row, available_moves[x].column, available_moves[x].shift
            else:
                score[0], score[1], score[2] = available_moves[index].row, available_moves[index].column, available_moves[index].shift

            if current_player == player:
                if score[3] < best[3]: # min value
                    best = score

            else:
                if score[3] > best[3]:# max value
                    best = score

            return best

        else: # if there is low numver of indexes picks random because of time.
            a = randint(0, len(available_moves) - 1)
            r = [available_moves[a].row, available_moves[a].column, available_moves[a].shift]

            return r


class Player(Player):
    def __init__(self):
        super().__init__()

        self.name = 'Gupse'

    def decide(self, game, state, available_moves, opponent_moves, queue):

        # Copying game and state to not effect the real game. Simulate on copies.
        gameCopy = deepcopy(game)
        stateCopy = deepcopy(state)

        if len(opponent_moves) == 0:
            index = randint(0, len(available_moves) - 1)
            move = available_moves[index]
            queue.put([move.row, move.column, move.shift])
            # return [
            #     move.row,
            #     move.column,
            #     move.shift
            # ]

        else:
            index = randint(0, len(available_moves) - 1)
            init_b = state.board
            move = available_moves[index]
            depth = len(available_moves)
            last_move = [-1, -1, -1]
            player = state.current_player
            moveR = minimax(stateCopy, available_moves, gameCopy, 20, last_move, init_b, player)
            queue.put([moveR[0], moveR[1], moveR[2]])
            # return [
            #     moveR[0],
            #     moveR[1],
            #     moveR[2]
            # ]
