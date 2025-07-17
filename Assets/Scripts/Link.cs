using System.Collections;
using UnityEngine;

public class Link : MonoBehaviour
{
    private Tile _tile1, _tile2; // 第1、2次点选的icon
    private bool _select;
    private Camera _mainCamera;
    private int _linkType; // 一折(0)、二折(1)或三折(2)
    private DrawLine _drawLine;
    private Vector3 _z1, _z2; // 折点位置

    private void Start()
    {
        _mainCamera = Camera.main;
        _drawLine = GetComponent<DrawLine>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            IsSelect();
        }
    }

    private void IsSelect()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            GameObject hitObj = hit.transform.gameObject;
            Tile tile = hitObj.GetComponent<Tile>();

            if (_select)
            {
                if (tile == _tile1)
                    return;

                _tile2 = tile;
                _tile1.GetComponent<SpriteRenderer>().color = Color.white;
                _select = false;
                IsSame();
            }
            else
            {
                // 第1次点击
                _tile1 = tile;
                _tile1.GetComponent<SpriteRenderer>().color = Color.red;
                _select = true;
            }
        }
    }

    private void IsSame()
    {
        if (_tile1 != _tile2 && _tile1.value == _tile2.value)
        {
            IsLink(_tile1.x, _tile1.y, _tile2.x, _tile2.y);
        }
        else
        {
            _tile1 = null;
            _tile2 = null;
        }
    }
    
    private bool IsLink(int x1, int y1, int x2, int y2)
    {
        if (x1 == x2)
        {
            if (XLink(y1, y2, x1))
            {
                _linkType = 0;
                StartCoroutine(DestroyTile(x1, y1, x2, y2));
                return true;
            }
        } else if (y1 == y2)
        {
            if (YLink(x1, x2, y1))
            {
                _linkType = 0;
                StartCoroutine(DestroyTile(x1, y1, x2, y2));
                return true;
            }
        }

        if (OneCornerLink(x1, y1, x2, y2))
        {
            _linkType = 1;
            StartCoroutine(DestroyTile(x1, y1, x2, y2));
            return true;
        }

        if (TwoCornerLink(x1, y1, x2, y2))
        {
            _linkType = 2;
            StartCoroutine(DestroyTile(x1, y1, x2, y2));
            return true;
        }
        
        return false;
    }

    // 消除牌
    private IEnumerator DestroyTile(int x1, int y1, int x2, int y2)
    {
        _drawLine.DrawLinkLine(_tile1.gameObject, _tile2.gameObject, _linkType, _z1, _z2);
        yield return new WaitForSeconds(0.2f);
        MapController.testMap[x1, y1] = MapController.empty;
        MapController.testMap[x2, y2] = MapController.empty;
        Destroy(_tile1.gameObject);
        Destroy(_tile2.gameObject);
        _tile1 = null;
        _tile2 = null;
    }

    // 垂直直连检测
    private bool YLink(int x1, int x2, int y)
    {
        if (x1 > x2)
            (x1, x2) = (x2, x1);

        for (int i = x1 + 1; i <= x2; i++)
        {
            if (i == x2) //相邻
                return true;
            if (MapController.testMap[i, y] != MapController.empty)
                break;
        }

        return false;
    }

    // 水平直连检测
    private bool XLink(int y1, int y2, int x)
    {
        if (y1 > y2)
            (y1, y2) = (y2, y1);
        
        for (int i = y1 + 1; i <= y2; i++)
        {
            if (i == y2) // 相邻
                return true;
            if (MapController.testMap[x, i] != MapController.empty)
                break;
        }
        
        return false;
    }

    //  x1,y1(实)  x1,y2
    //           \
    //  x2,y1      x2,y2(实)
    //
    // 一折检测
    private bool OneCornerLink(int x1, int y1, int x2, int y2)
    {
        if (MapController.testMap[x1, y2] == MapController.empty)
        {
            if (XLink(y1, y2, x1) && YLink(x1, x2, y2))
            {
                _z1 = new Vector3(y2 * MapController.yMove, x1 * MapController.xMove, -1);
                return true;
            }
        }

        if (MapController.testMap[x2, y1] == MapController.empty)
        {
            if (XLink(y1, y2, x2) && YLink(x1, x2, y1))
            {
                _z1 = new Vector3(y1 * MapController.yMove, x2 * MapController.xMove, -1);
                return true;
            }
        }

        return false;
    }

    // x1,y1(实)  x1,y2
    //    |
    //    ()
    //         \
    // x3,y1      x3,y2(实)
    //
    // 二折检测
    private bool TwoCornerLink(int x1, int y1, int x2, int y2)
    {
        // 右探
        for (int i = y1 + 1; i <= MapController.colNum + 1; i++)
        {
            if (MapController.testMap[x1, i] == MapController.empty && OneCornerLink(x1, i, x2, y2))
            {
                _z2 = new Vector3(i * MapController.yMove, x1 * MapController.xMove, -1);
                return true;
            }
            if (MapController.testMap[x1, i] != MapController.empty)
                break;
        }
        
        // 左探
        for (int i = y1 - 1; i > -1; i--)
        {
            if (MapController.testMap[x1, i] == MapController.empty && OneCornerLink(x1, i, x2, y2))
            {
                _z2 = new Vector3(i * MapController.yMove, x1 * MapController.xMove, -1);
                return true;
            }
            if (MapController.testMap[x1, i] != MapController.empty)
                break;
        }
        
        // 下探
        for (int i = x1 + 1; i <= MapController.rowNum + 1; i++)
        {
            if (MapController.testMap[i, y1] == MapController.empty && OneCornerLink(i, y1, x2, y2))
            {
                _z2 = new Vector3(y1 * MapController.yMove, i * MapController.xMove, -1);
                return true;
            }
            if (MapController.testMap[i, y1] != MapController.empty)
                break;
        }

        // 上探
        for (int i = x1 - 1; i > -1; i--)
        {
            if (MapController.testMap[i, y1] == MapController.empty && OneCornerLink(i, y1, x2, y2))
            {
                _z2 = new Vector3(y1 * MapController.yMove, i * MapController.xMove, -1);
                return true;
            }
            if (MapController.testMap[i, y1] != MapController.empty)
                break;
        }
        
        return false;
    }
}
