from random import randint

class Player:
    def __init__(self):
        self.move_history = []
        self.name = None

    def set_move_history(self, move):
        self.move_history.append(move)

class Player(Player):
    def __init__(self):
        super().__init__()

        self.name = 'random'

    def decide(self, game, state, available_moves, opponent_moves, queue):
        index = randint(0, len(available_moves) - 1)
        move = available_moves[index]
        queue.put([move.row, move.column, move.shift])
        # return [
        #     move.row,
        #     move.column,
        #     move.shift
        #     ]
