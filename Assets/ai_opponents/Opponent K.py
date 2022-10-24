import sys
import numpy as np
import random
import copy

class Player:
    def __init__(self):
        self.move_history = []
        self.name = None

    def set_move_history(self, move):
        self.move_history.append(move)


class Player(Player):
    def __init__(self):
        super().__init__()

        self.move = None
        self.name = 'Mert Genco - Abdulkadir Kundakcioglu'

    def decide(self, game, state, available_moves, opponent_moves, queue):
        # print(score(game, state))
        # value = minimax(game, state, 0, True if state.current_player == 1 else False)
        # print(value[0], value[1])

        # prefered_move = value[1]
        print(score(game, state))
        root = Node(game, state)
        # prefered_move = (root.search_max() if state.current_player == 1 else root.search_min())[1]
        result = root.search_depth(_max=True if state.current_player == 1 else False, depth=0, max_depth=1)
        prefered_move = result[1]
        print(result[0], result[1])
        queue.put([prefered_move.row, prefered_move.column, prefered_move.shift])
        #return [prefered_move.row, prefered_move.column , prefered_move.shift]

def _score(game, state):
    board = copy.deepcopy(state.board)

    if game.check_for_winner(state.board) == True:
        if game.determine_winner(state.board) == 1:
            return float("inf")
        else:
            return float("-inf")

    total = 0

    for i in range(0, len(board)):
        for j in range(0, len(board[i])):
            if board[i][j] != 0:
                score = board[i][j]
                if board[i][j] > 0:
                    if j != 0 and board[i][j - 1] > 0:
                        score *= board[i][j - 1] * 2
                    if i != 0 and board[i - 1][j] > 0:
                        score *= board[i - 1][j] * 2
                if board[i][j] < 0:
                    if j != 0 and board[i][j - 1] < 0:
                        score = -abs(score * (board[i][j - 1] * 2))
                    if i != 0 and board[i - 1][j] > 0:
                        score = -abs(score * (board[i - 1][j] * 2))
                board[i][j] = score
                total += score

    return total

def score(game, state):
    board = state.board

    if game.check_for_winner(state.board) == True:
        if game.determine_winner(state.board) == 1:
            return float("inf")
        else:
            return float("-inf")

    us = 0
    enemy = 0

    for i in range(0, len(board)):
        _us = 0
        _enemy = 0
        for j in range(0, len(board[i])):
            if board[i][j] != 0:
                score = 2
                if board[i][j] == 1:
                    for k in range(j, 0, -1):
                        if board[i][k] == 1:
                            score *= 2
                        else:
                            break
                    for k in range(j, len(board)):
                        if board[i][k] == 1:
                            score *= 2
                        else:
                            break
                    _us += score
                else:
                    for k in range(j, 0, -1):
                        if board[i][k] == 1:
                            score *= 2
                        else:
                            break
                    for k in range(j, len(board)):
                        if board[i][k] == 1:
                            score *= 2
                        else:
                            break
                    _enemy += score
        us += _us
        enemy += _enemy

    for j in range(0, len(board)):
        _us = 0
        _enemy = 0
        for i in range(0, len(board[j])):
            if board[i][j] != 0:
                score = 2
                if board[i][j] == 1:
                    for k in range(j, 0, -1):
                        if board[i][k] == 1:
                            score *= 3
                        else:
                            break
                    for k in range(j, len(board)):
                        if board[i][k] == 1:
                            score *= 3
                        else:
                            break
                    _us += score
                else:
                    for k in range(j, 0, -1):
                        if board[i][k] == 1:
                            score *= 2
                        else:
                            break
                    for k in range(j, len(board)):
                        if board[i][k] == 1:
                            score *= 2
                        else:
                            break
                    _enemy += score
        us += _us
        enemy += _enemy

    return us - enemy

def minimax(game, state, depth, is_maximizing_player, alpha=float("-inf"), beta=float("inf")):
    if game.check_for_winner(state.board) == True:
        return (score(game, state), None)

    if depth == 4:
        return (score(game, state), None)

    moves = game.get_moves(state)

    if is_maximizing_player:
        best_score = float("-inf")
        best_move = None

        for move in moves:
            _state = game.apply_move(copy.deepcopy(state), move)
            value = minimax(game, _state, depth+1, False, alpha=alpha, beta=beta)

            if best_score < value[0]:
                best_move = move
                best_score = value[0]

            alpha = max(alpha, best_score)

            if beta <= alpha:
                break
        #print(depth, is_maximizing_player, best_score, best_move)
        return (best_score, best_move)
    else:
        best_score = float("inf")
        best_move = None

        for move in moves:
            _state = game.apply_move(copy.deepcopy(state), move)
            value = minimax(game, _state, depth+1, True, alpha=alpha, beta=beta)

            if best_score > value[0]:
                best_move = move
                best_score = value[0]

            beta = min(alpha, best_score)

            if beta <= alpha:
                break
        #print(depth, is_maximizing_player, best_score, best_move)
        return (best_score, best_move)

class Node:
    def __init__(self, game, state):
        self.game = game
        self.state = state
        self.children = []
        self.init_moves()
        self.score = score(game, state)

    def init_moves(self):
        self.moves = self.game.get_moves(self.state)
        random.shuffle(self.moves)

    def generate_child(self):
        if len(self.moves) == len(self.children):
            return False

        move = self.moves[len(self.children)]

        _state = self.game.apply_move(copy.deepcopy(self.state), move)
        new_node = Node(self.game, _state)
        self.children.append(new_node)
        return True

    def search_max(self):
        best_score = float("-inf")
        best_move = None
        while self.generate_child():
            if best_score < self.children[len(self.children) - 1].score:
                best_score = self.children[len(self.children) - 1].score
                best_move = self.moves[len(self.children) - 1]
        return (best_score, best_move)

    def search_min(self):
        best_score = float("inf")
        best_move = None
        while self.generate_child():
            if best_score > self.children[len(self.children) - 1].score:
                best_score = self.children[len(self.children) - 1].score
                best_move = self.moves[len(self.children) - 1]
        return (best_score, best_move)

    def search_depth(self, _max=True, depth=0, max_depth=float("inf"), alpha=float("-inf"), beta=float("inf")):
        if depth == max_depth:
            return self.search_max() if _max else self.search_min()

        if _max:
            best_score = float("-inf")
            best_move = None

            while self.generate_child():
                child = self.children[len(self.children) - 1]
                value = child.search_depth(_max=False, depth=depth+1, max_depth=max_depth, alpha=alpha, beta=beta)
                value = value[0]
                if best_score < value:
                    best_move = self.moves[len(self.children) - 1]
                    best_score = value

                alpha = max(alpha, best_score)

                if beta <= alpha:
                    break
            return best_score, best_move
        else:
            best_score = float("inf")
            best_move = None

            while self.generate_child():
                child = self.children[len(self.children) - 1]
                value = child.search_depth(_max=True, depth=depth+1, max_depth=max_depth, alpha=alpha, beta=beta)
                value = value[0]
                if best_score > value:
                    best_move = self.moves[len(self.children) - 1]
                    best_score = value

                beta = min(beta, best_score)

                if beta <= alpha:
                    break
            return best_score, best_move

