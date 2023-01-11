using UnityEngine;

public class AIHandeler : MonoBehaviour
{
    private float deltaTime = 0f;
    [HideInInspector]public GameObject AIPickedCube;
    private int[] position = new int[2];
    [HideInInspector]public int move = -1;
    [Range(1,5)]
    public int minMaxDepth;
    private IAgent AIAgent;
    public static bool AITurn = false;
    gameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GetComponent<gameController>();
        AIAgent = new minMax(1, minMaxDepth);
        AITurn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (AITurn)
        {
            deltaTime += Time.deltaTime;
            if(move == -1)
            {
                int[] AIMove =  AIAgent.MakeMove(gameController.gameBoardToArray());
                position = new int[2] {AIMove[0], AIMove[1]};
                move = AIMove[2];
                AIPickedCube = gameController.cubeList[position[0]][position[1]];
                gameController.selectedCube = AIPickedCube;
                AIPickedCube.GetComponent<cubeController>().cubeValue = gameController.turn;
                AIPickedCube.transform.localRotation = AIPickedCube.GetComponent<cubeController>().cubeValue == 2 ? Quaternion.Euler(90, 0, 0) : Quaternion.Euler(AIPickedCube.GetComponent<cubeController>().cubeValue * 180 + 180, 0, 0);

            }

            if (deltaTime < 1) AIPickedCube.transform.localPosition = new Vector3(AIPickedCube.transform.localPosition.x, 1 * deltaTime, AIPickedCube.transform.localPosition.z);

            if (deltaTime > 1 && deltaTime < 3) AIMovePiece(move, deltaTime);

            if (deltaTime > 3)
            {
                gameController.placeCube();
                deltaTime = 0;
                move = -1;
                gameController.resetAllCubes();
                gameController.turn = gameController.turn == 1 ? 0 : 1;
                AITurn = false;
                gameController.checkGameStatus();
            }

            


        }
    }


    private int[] AIPickPiece()
    {
        int row, col;
        while (true)
        {
            row = Random.Range(0, 5);
            if (row == 0 || row == 4) col = Random.Range(0, 5);
            else col = Random.Range(0, 2) == 1 ? 4 : 0;
            if (gameController.cubeList[row][col].GetComponent<cubeController>().cubeValue != 0) break;
        }

        int[] position = new int[2];
        position[0] = row;
        position[1] = col;

        return position;
    }
    public int AIPickMove(int[] position)
    {
        int move = -1;

        while (move == -1)
        {
            int temp = Random.Range(0, 4);

            if (temp == 0 && position[0] == 4) continue;
            if (temp == 1 && position[0] == 0) continue;
            if (temp == 2 && position[1] == 0) continue;
            if (temp == 3 && position[1] == 4) continue;

            move = temp;
        }

        return move;
    }

    public void AIMovePiece(int move, float deltaTime)
    {
 
        float destinationRatio = (deltaTime-1) / 2;
        switch (move)
        {
            case 0:
                AIPickedCube.transform.localPosition = new Vector3(AIPickedCube.GetComponent<cubeController>().defaultPosition.x, 1, AIPickedCube.GetComponent<cubeController>().defaultPosition.z + (1.72f - AIPickedCube.GetComponent<cubeController>().defaultPosition.z) * destinationRatio);
                
                for(int i = position[0] + 1; i<=4; i++)
                {
                    GameObject cubeToMove = gameController.cubeList[i][position[1]];
                    float destination = (-0.86f * destinationRatio) + cubeToMove.GetComponent<cubeController>().defaultPosition.z;
                    cubeToMove.transform.localPosition = new Vector3(cubeToMove.transform.localPosition.x, cubeToMove.transform.localPosition.y, destination);
                }
                break;
            case 1:
                AIPickedCube.transform.localPosition = new Vector3(AIPickedCube.GetComponent<cubeController>().defaultPosition.x, 1, AIPickedCube.GetComponent<cubeController>().defaultPosition.z + (-1.72f - AIPickedCube.GetComponent<cubeController>().defaultPosition.z) * destinationRatio);

                for (int i = position[0] - 1; i >= 0; i--)
                {
                    GameObject cubeToMove = gameController.cubeList[i][position[1]];
                    float destination = (0.86f * destinationRatio) + cubeToMove.GetComponent<cubeController>().defaultPosition.z;
                    cubeToMove.transform.localPosition = new Vector3(cubeToMove.transform.localPosition.x, cubeToMove.transform.localPosition.y, destination);
                }
                break;
            case 2:
                AIPickedCube.transform.localPosition = new Vector3(AIPickedCube.GetComponent<cubeController>().defaultPosition.x + (1.72f - AIPickedCube.GetComponent<cubeController>().defaultPosition.x) * destinationRatio, 1, AIPickedCube.GetComponent<cubeController>().defaultPosition.z);

                for (int i = position[1] - 1; i >= 0; i--)
                {
                    GameObject cubeToMove = gameController.cubeList[position[0]][i];
                    float destination = (-0.86f * destinationRatio) + cubeToMove.GetComponent<cubeController>().defaultPosition.x;
                    cubeToMove.transform.localPosition = new Vector3(destination, cubeToMove.transform.localPosition.y, cubeToMove.transform.localPosition.z);
                }

                break;
            case 3:
                AIPickedCube.transform.localPosition = new Vector3(AIPickedCube.GetComponent<cubeController>().defaultPosition.x + (-1.72f - AIPickedCube.GetComponent<cubeController>().defaultPosition.x) * destinationRatio, 1, AIPickedCube.GetComponent<cubeController>().defaultPosition.z);

                for (int i = position[1] + 1; i <= 4; i++)
                {
                    GameObject cubeToMove = gameController.cubeList[position[0]][i];
                    float destination = (0.86f * destinationRatio) + cubeToMove.GetComponent<cubeController>().defaultPosition.x;
                    cubeToMove.transform.localPosition = new Vector3(destination, cubeToMove.transform.localPosition.y, cubeToMove.transform.localPosition.z);
                }
                break;
        }
    }
}
