using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Done_Link : MonoBehaviour {
    public Camera gameCamera;
    public static GameObject g1, g2;
    public int x1, x2, y1, y2,value1,value2;
    public bool select = false;
    public int linkType;
    public Vector3 z1, z2;
    public GameObject upgradePrefab;
    // Use this for initialization
    void Start () {
		
	}

    void Update()
    {
        if (Input.GetButtonDown("Fire1") &&(isStop == true))
        {
            IsSelect();
        }
    }
    public void IsSelect()
    {

        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);//鼠标位置作为射线方向
        RaycastHit hit = new RaycastHit();//生成射线
        if (Physics.Raycast(ray, out hit))
        {

            if (select == false)
            {
                g1 = hit.transform.gameObject;//第一个点击的物体为g1
                g1.GetComponent<SpriteRenderer>().color = Color.red;//将g1的颜色改为红色
                x1 = g1.GetComponent<Done_Tile>().x;//获取g1在数组中的位置及贴图编号
                y1 = g1.GetComponent<Done_Tile>().y;
                value1 = g1.GetComponent<Done_Tile>().value;
                select = true;
            }
            else
            {
                g2 = hit.transform.gameObject;
                g1.GetComponent<SpriteRenderer>().color = Color.white;
                x2 = g2.GetComponent<Done_Tile>().x;
                y2 = g2.GetComponent<Done_Tile>().y;
                value2 = g2.GetComponent<Done_Tile>().value;
                select = false;
                IsSame();
            }
        }
    }

    public void IsSame()
    {
        if ((value1 == value2)&&(g1.transform.position!=g2.transform.position))
        {
            IsLink(x1, y1, x2, y2);
        }
        else
        {
            x1 = x2 = y1 = y2 = value1 = value2 = 0;
        }
    }
    bool X_Link(int x1, int x2, int y2)
    {
        if (x1 > x2)
        {
            int n = x1;
            x1 = x2;
            x2 = n;
        }
        for (int i = x1 + 1; i <= x2; i++)
        {
            if (i == x2){return true; }//相邻
            if (Done_MapController.test_map[i, y2] != 0) { break; }//间隔若不为空，则跳出
        }
        return false;
    }
    bool Y_Link(int x1, int y1, int y2)
    {
        if (y1 > y2)
        {
            int n = y1;
            y1 = y2;
            y2 = n;
        }

        for (int i = y1 + 1; i <= y2; i++)
        {
            if (i == y2){return true;}//相邻
            if (Done_MapController.test_map[x1, i] != 0) { break; }//间隔若不为空，则跳出
        }
        return false;
    }


    bool OneCornerLink(int x1, int y1, int x2,int y2)
    {
        if (Done_MapController.test_map[x1, y2] == 0)
        {
            if (X_Link(x1, x2, y2) && Y_Link(x1, y1, y2))
            {
                z1 = new Vector3(x1*Done_MapController.xMove, -y2*Done_MapController.yMove, -1);
                return true;
            }
        }

        if (Done_MapController.test_map[x2, y1] == 0)
        { 
            if (X_Link(x1, x2, y1) && Y_Link(x2, y1, y2))
            {
                z1 = new Vector3(x2 * Done_MapController.xMove, -y1 * Done_MapController.yMove, -1);
                return true;
            }
        }
        return false;
    }

    bool TwoCornerLink(int x1, int y1, int x2, int y2)
    {
        //右探
        for (int i = x1+1; i < Done_MapController.columNum+2; i++)
        {
            
            if (Done_MapController.test_map[i, y1] == 0)
            {
                if (OneCornerLink(i, y1, x2, y2))
                {
                    z2 = new Vector3(i * Done_MapController.xMove, -y1 * Done_MapController.yMove, -1);
                    return true;
                }
            }

            if (Done_MapController.test_map[i, y1] != 0)
            {
                break;
            }
            

        }

        //左探
        for (int i = x1 - 1; i > -1; i--)
        {
            if (Done_MapController.test_map[i, y1] == 0)
            {
                if (OneCornerLink(i, y1, x2, y2))
                {
                    z2 = new Vector3(i * Done_MapController.xMove, -y1 * Done_MapController.yMove, -1);
                    return true;
                }
            }

            if (Done_MapController.test_map[i, y1] != 0)
            {
                break;
            }
            
        }

        //下探
        for (int i = y1 + 1; i < Done_MapController.rowNum+2; i++)
        {
            if (Done_MapController.test_map[x1, i] == 0)
            {
                if (OneCornerLink(x1, i, x2, y2))
                {
                    z2 = new Vector3(x1 * Done_MapController.xMove, -i * Done_MapController.yMove, -1);
                    return true;
                }
            }

            if (Done_MapController.test_map[x1, i] != 0)
            {
                break;
            }
            
        }

        //上探
        for (int i = y1 - 1; i > -1; i--)
        {
            if (Done_MapController.test_map[x1, i] == 0)
            {
                if (OneCornerLink(x1, i, x2, y2))
                {
                    z2 = new Vector3(x1 * Done_MapController.xMove, -i * Done_MapController.yMove, -1);
                    return true;
                }
            }

            if (Done_MapController.test_map[x1, i] != 0)
            {
                break;
            }
           
        }
        return false;
    }

    bool IsLink(int x1, int y1, int x2, int y2)
    {
        if (x1 == x2)
        {
            if (Y_Link(x1, y1, y2))
            {
                linkType = 0;
                StartCoroutine(destory(x1, y1, x2, y2));
                return true;
            }
        }
        else if (y1 == y2)
        {
            if (X_Link(x1, x2, y1))
            {
                linkType = 0;
                StartCoroutine(destory(x1, y1, x2, y2));
                return true;
            }
        }

         if (OneCornerLink(x1, y1, x2, y2))
        {
            linkType = 1;
            StartCoroutine(destory(x1, y1, x2, y2));
            return true;
        }

         if (TwoCornerLink(x1, y1, x2, y2))
        {
            linkType = 2;
            StartCoroutine(destory(x1, y1, x2, y2));
            return true;
        }
        return false;
    }

    IEnumerator destory(int x1,int y1,int x2,int y2)
    {
        
        FindObjectOfType<Done_DrawLine>().DrawLinkLine(g1, g2, linkType,z1,z2);
        //生成道具
        if (Random.value < 0.10)
        {
            GameObject g;
            g = Instantiate(upgradePrefab, new Vector3(8, -6, -1), Quaternion.identity);
            string name = g.GetComponent<Done_UpGrade>().upgradeName;
            performUpgrade(name);
        }
        yield return new WaitForSeconds(0.2f);
        Destroy(g1);
        Destroy(g2);
        Done_MapController.test_map[x1, y1] = 0;//刷新数组中g1的位置信息
        Done_MapController.test_map[x2, y2] = 0;//刷新数组中g2的位置信息
        x1 = x2 = y1 = y2 = value1 = value2 = 0;

    }

    /// <summary>
    /// 道具功能实现
    /// </summary>
    /// <param name="name"></param>
    void performUpgrade(string name)
    {
        name = name.Remove(name.Length - 21);
        switch (name)
        {
            case "plus":
                break;
            case "stop":
                isStoped();
                break;
            case "clock":
                break;
        }
    }
    bool isStop = true;
    void isStoped()
    {

        isStop = false;
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
            yield return new WaitForSeconds(3.0f);//禁手持续时间为3秒
            isStop = true;
    }
}
