using UnityEngine;
using Random = UnityEngine.Random;

public class MapController : MonoBehaviour
{
    public GameObject tilePrefab;
    public int rowNum = 14;
    public int colNum = 18;
    public Sprite[] tiles; // 贴图数组
    private static int[,] tempMap; // 初始化偶数张牌以及被随机打乱的数组
    private static int[,] testMap; // 储存被打乱后的tempMap以及在周围加上一圈0
    private static float xMove = 0.71f;
    private static float yMove = 0.71f;


    private void Start()
    {
        tempMap = new int[rowNum, colNum];
        testMap = new int[rowNum + 2, colNum + 2];
        for (int i = 0; i < rowNum; i++)
        {
            for (int j = 0; j < colNum; j += 2)
            {
                int idx = Random.Range(0, tiles.Length);
                tempMap[i, j] = idx;
                tempMap[i, j + 1] = idx;
            }
        }
        
        Shuffle();

        // 把tempMap赋给testMap
        for (int i = 0, row = testMap.GetLength(0); i < row; i++)
        {
            for (int j = 0, col = testMap.GetLength(1); j < col; j++)
            {
                if (i == 0 || j == 0 || i == row - 1 || j == col - 1)
                {
                    testMap[i, j] = 0;
                    continue;
                }
                
                testMap[i, j] = tempMap[i - 1, j - 1];
            }
        }
        
        BuildMap();
    }
    
    // 洗牌
    private void Shuffle()
    {
        for (int i = 0; i < rowNum; i++)
        {
            for (int j = 0; j < colNum; j++)
            {
                int newI = Random.Range(0, rowNum);
                int newJ = Random.Range(0, colNum);
                (tempMap[i, j], tempMap[newI, newJ]) = (tempMap[newI, newJ], tempMap[i, j]);
            }
        }
    }


    private void BuildMap()
    {
        int row = testMap.GetLength(0);
        int col = testMap.GetLength(1);
        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < col; y++)
            {
                if (x == 0 || y == 0 || x == row - 1 || y == col - 1)
                    continue;
                
                GameObject tileObj = Instantiate(tilePrefab, transform, true);
                tileObj.transform.position = new Vector3(y * yMove, x * xMove, 0);
                int idxValue = testMap[x, y];
                tileObj.GetComponent<SpriteRenderer>().sprite = tiles[idxValue];
                Tile tile = tileObj.GetComponent<Tile>();
                tile.x = x;
                tile.y = y;
                tile.value = idxValue;
            }
        }
    }
}
