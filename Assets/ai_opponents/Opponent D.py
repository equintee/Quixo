import sys
import random
"""
class MoveNode: #Node implementation for moves
    def __init__(self):
        self.row=None
        self.column=None
        self.shift=None
        self.parent=None
        self.children =[]
    def createChild(self,r,c,s):
        newChild=MoveNode()
        newChild.row =r
        newChild.column=c
        newChild.shift=s
        newChild.parent=self
        self.children.append(newChild)
    def mychildren(self,game,state,root,available_moves):
        for i in available_moves:
            createChild(self,i.row,i.column,i.shift)
"""

class Player:
    def __init__(self):
        self.move_history = []
        self.name = None

    def set_move_history(self, move):
        self.move_history.append(move)


class Player(Player):
    def __init__(self):
        super().__init__()

        self.name = 'denizanilcolak'




    def decide(self, game, state, available_moves, opponent_moves, queue):
            counter =0
            for i in range (0,5): #check for the load factor of board
                for j in range (0,5):
                    if state.board[i][j]== 0:
                        counter+=1
      # if counter >= 10:#if the board is not fully loaded like more than 10 points
            row_moves = []
            column_moves = []
            if (state.current_player * 1):#if the studentcode is playing it tries to manuplate the opponents move according to its direction
                manuplating_row=opponent_moves[len(opponent_moves)-1].row
                manuplating_column=opponent_moves[len(opponent_moves)-1].column
                manuplating_shift=opponent_moves[len(opponent_moves) - 1].shift
                if(manuplating_shift == 1 or manuplating_shift ==3): #if the direction is 1 or 3 it means we have to manuplate row
                    #control row
                    for i in available_moves:
                        if(i.row == manuplating_row and i.shift == manuplating_shift):#with using the shift and column of the opponents move we choose a move from available moves
                            row_moves.append(i)


                    if len(row_moves)!=0:
                        selected = row_moves[random.randint(0, len(row_moves) - 1)]
                        move = selected
                        print("Selected:", selected)
                    else:
                        move=available_moves[random.randint(0,len(available_moves)-1)]
                    for j in row_moves:
                        print(j)

                    queue.put([move.row, move.column, move.shift])
                    # return [
                    #     move.row,
                    #     move.column,
                    #     move.shift
                    # ]

                else:#if the direction is 0 or 2 it means we have to manuplate column
                    #control column
                    for i in available_moves:
                        if (i.column == manuplating_column and i.shift==manuplating_shift): #with using the shift and column of the opponents move we choose a move from available moves
                            column_moves.append(i)


                    if len(column_moves)!=0:
                        selected = column_moves[random.randint(0, len(column_moves) - 1)]
                        move = selected
                        print("Selected:", selected)
                    else:
                        move = available_moves[random.randint(0, len(available_moves) - 1)]
                    for j in column_moves:
                            print(j)
                    queue.put([move.row, move.column, move.shift])
                    # return [
                    #     move.row,
                    #     move.column,
                    #     move.shift
                    # ]
"""
        else: #if the board loaded enough we try to decide on the move with using trees of moves.With using tree we can select the possible winning move sequence.
             selected_root=available_moves[random.randint(0, len(available_moves) - 1)]
             root=MoveNode()
             root.row=selected_root.row
             root.column=selected_root.column
             root.shift=selected_root.shift"""




