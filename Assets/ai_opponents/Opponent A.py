import sys
from math import inf as infinity
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

        self.name = 'MKostekli_ATuranlı'
        self.player_id = 0

    def decide(self, game, state, available_moves, opponent_moves, queue):
        self.player_id = state.current_player
        if len(available_moves) > 37:
            best_move = self.minimax_search(game,state,available_moves,1,self.player_id)
        elif len(available_moves) > 17 and len(available_moves) <= 37:
            best_move = self.minimax_search(game, state, available_moves, 2, self.player_id)
        else:
            best_move = self.minimax_search(game, state, available_moves, 3, self.player_id)

        queue.put([best_move[0], best_move[1], best_move[2]])
        #return best_move[0], best_move[1], best_move[2]

    def minimax_search(self,game,state,available_moves,depth,player):
        if(player == self.player_id): # if level is max
            best = [-1, -1, -1, -infinity]
        else: # if level is min
            best = [-1, -1, -1, +infinity]


        isThereWinner = game.check_for_winner(state.board)
        if depth == 0 or isThereWinner:
            if isThereWinner:
                score = self.evaluate_state(game, state)
            else:
                score = 0
            return [-1, -1, -1, score]


        temp_player = copy.deepcopy(player)
        for move in available_moves:
            temp_state = copy.deepcopy(state)
            temp_state = game.apply_move(temp_state,move)
            current_available_moves  = game.get_moves(temp_state)
            score = self.minimax_search(game, temp_state, current_available_moves, depth-1, -temp_player)
            score[0], score[1], score[2] = move.row, move.column, move.shift

            if temp_player == self.player_id:
                if score[3] > best[3]:
                    best = score

            else:
                if score[3] < best[3]:
                    best = score

        return best



    def evaluate_state(self,game,state):
        winner = game.determine_winner(state.board)
        if winner == self.player_id:  #if AI win
            score = +1
        elif winner == -self.player_id: # Opponent win
            score = -1
        else:
            score = 0
        return score



