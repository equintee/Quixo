import sys
import numpy
from random import randint
from math import inf as infinity
from copy import deepcopy
sys.setrecursionlimit(100000000)    # increase limit of recursion

class Player:
    def __init__(self):
        self.move_history = []
        self.name = None

    def set_move_history(self, move):
        self.move_history.append(move)

def minimax(state, available_moves, game, depth, last_move, init_b):
    init_player = state.current_player  # hold current player

    for x in range(5):
        if len(available_moves) >= 5:
            if init_player == state.current_player:
                best = [-1, -1, -1, -infinity]  # make AI score min
            else:
                best = [-1, -1, -1, +infinity]  # make opponent score max

            if available_moves[x].row != last_move[0] or available_moves[x].column != last_move[1] or available_moves[x].shift != last_move[2]:
                # prevent to last moves and current moves are the same
                game.apply_move(state, available_moves[x])
                last_move[0] = available_moves[x].row
                last_move[1] = available_moves[x].column
                last_move[2] = available_moves[x].shift
                flag = 0

            elif available_moves[x].row == last_move[0] and available_moves[x].column == last_move[1] and available_moves[x].shift == last_move[2]:
                # if current move same as last move, choose random move from available moves
                index = randint(0, len(available_moves) - 1)
                game.apply_move(state, available_moves[index])
                last_move[0] = available_moves[index].row
                last_move[1] = available_moves[index].column
                last_move[2] = available_moves[index].shift
                flag = 1


            state.current_player = state.current_player * -1    # cahnge player (1 means '0', -1 means 'x')
            if depth == 0:
                if game.check_for_winner(state.board) == True:
                    if init_player == state.current_player:
                        score = [-1, -1, -1, 10]    # increase score when AI won
                    else:
                        score = [-1, -1, -1, -10]   # decrease score when AI lost
                else:
                    score = [-1, -1, -1, -1]

            elif depth != 0:
                score = minimax(state, game.get_moves(state), game, depth - 1, last_move, init_b)   # call minimax recursively

            if flag == 0:
                # add values of current moves to score
                score[0], score[1], score[2] = available_moves[x].row, available_moves[x].column, available_moves[x].shift
            else:
                score[0], score[1], score[2] = available_moves[index].row, available_moves[index].column, available_moves[index].shift

            if state.current_player == init_player:
                if score[3] > best[3]:
                    # compare previos scores and current. if current bigger, exchange
                    best = score  # max value
            else:
                if score[3] < best[3]:
                    best = score  # min value

            return score
        elif len(available_moves) < 5:
            index = randint(0, len(available_moves) - 1)
            move = available_moves[index]
            game.apply_move(state, move)
            game.print_board(state)

            return [
                move.row,
                move.column,
                move.shift
            ]

class Player(Player):
    def __init__(self):
        super().__init__()

        self.name = 'Sena - Görey'

    def decide(self, game, state, available_moves, opponent_moves, queue):
        # copy the status of the game, to make changes
        gameCopy = deepcopy(game)
        stateCopy = deepcopy(state)

        # choose first move random
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
            init_b = state.board
            last_move = [-1, -1, -1]
            move = minimax(stateCopy, available_moves, gameCopy, 5, last_move, init_b)
            queue.put([move[0], move[1], move[2]])
            # return [
            #     move[0],
            #     move[1],
            #     move[2]
            # ]
