using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Done_MapController : MonoBehaviour
{
    
    public GameObject tile;
    public static int rowNum = 14;//方块行数  
    public static int columNum = 18;//方块列数  
    public static int[,] temp_map;
    public static int[,] test_map;
    public Sprite[] tiles;//方块数组
    public static float xMove = 0.61f;
    public static float yMove = 0.61f;



    void Awake()
    {
        test_map = new int[columNum + 2, rowNum + 2];
        temp_map = new int[columNum, rowNum];
        for (int i = 0; i < rowNum; i++)
        {
            for (int j = 0; j < columNum; j = j + 2)
            {
                int temp = Random.Range(0, tiles.Length);
                temp_map[j, i] = temp;  //同时生成2张一样的牌 确保不出现单数牌
                temp_map[j+1, i] = temp;
            }
        }
        ChangeMap();

        for (int i = 0; i < rowNum + 2; i++)
        {
            for (int j = 0; j < columNum + 2; j++)
            {
                if (i == 0 || j == 0 || i == rowNum + 1 || j == columNum + 1)
                {
                    test_map[j, i] = 0;
                }
                else
                {
                    test_map[j, i] = temp_map[j - 1, i - 1];
                }
            }
        }
        BuildMap();
        FindObjectOfType<Done_DrawLine>().CreatLine();
    }

    public void ChangeMap()//将储存ID的数组打乱
    {
        for (int i = 0; i < rowNum; i++)
        {
            for (int j = 0; j < columNum; j++)
            {
                int temp = temp_map[j, i];
                int randomRow = Random.Range(0, rowNum);
                int randomColum = Random.Range(0, columNum);
                temp_map[j, i] = temp_map[randomColum, randomRow];
                temp_map[randomColum, randomRow] = temp;
            }
        }
    }

    public void BuildMap()
    {
        int i = 0;//数组中的行
        int j = 0;//数组中的列
        GameObject g;
        for (int y = 0; y < rowNum+2; y++) //x，y表示实际坐标
        {
            for (int x = 0; x < columNum+2; x++)
            {

                
                    g = Instantiate(tile) as GameObject;
                    g.transform.position = new Vector3(x * xMove, -y * yMove, 0);
                    Sprite icon = tiles[test_map[j, i]];
                    g.GetComponent<SpriteRenderer>().sprite = icon;
                    g.GetComponent<Done_Tile>().x = x;//储存牌的属性
                    g.GetComponent<Done_Tile>().y = y;
                    g.GetComponent<Done_Tile>().value = test_map[j, i];
                if (x == 0 || y == 0 || x == columNum + 1 || y == rowNum + 1)
                {
                    g.GetComponentInChildren<SpriteRenderer>().enabled = false;
                }
                j++;
            }
            i++;
            j = 0;
        }
    }


    // Update is called once per frame

}
