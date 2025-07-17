using System.Collections;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    private LineRenderer _line1, _line2, _line3;

    public void CreateLine()
    {
        GameObject line = new GameObject("line1");
        _line1 = line.AddComponent<LineRenderer>();
        _line1.startColor = Color.green;
        _line1.endColor = Color.green;
        _line1.startWidth = 0.1f;
        _line1.endWidth = 0.1f;
        _line1.positionCount = 2;

        line = new GameObject("line2");
        _line2 = line.AddComponent<LineRenderer>();
        _line2.startColor = Color.green;
        _line2.endColor = Color.green;
        _line2.startWidth = 0.1f;
        _line2.endWidth = 0.1f;
        _line2.positionCount = 2;
        
        line = new GameObject("line3");
        _line3 = line.AddComponent<LineRenderer>();
        _line3.startColor = Color.green;
        _line3.endColor = Color.green;
        _line3.startWidth = 0.1f;
        _line3.endWidth = 0.1f;
        _line3.positionCount = 2;
    }

    // 绘制连接线
    public void DrawLinkLine(GameObject g1, GameObject g2, int linkType, Vector3 z1, Vector3 z2)
    {
        if (linkType == 0)
        {
            _line1.SetPosition(0, g1.transform.position + new Vector3(0, 0, -1));
            _line1.SetPosition(1, g2.transform.position + new Vector3(0, 0, -1));
        }

        if (linkType == 1)
        {
            _line1.SetPosition(0, g1.transform.position);
            _line1.SetPosition(1, z1);
            _line2.SetPosition(0, z1);
            _line2.SetPosition(1, g2.transform.position);
        }

        if (linkType == 2)
        {
            _line1.SetPosition(0, g1.transform.position);
            _line1.SetPosition(1, z2);
            _line2.SetPosition(0, z2);
            _line2.SetPosition(1, z1);
            _line3.SetPosition(0, z1);
            _line3.SetPosition(1, g2.transform.position);
        }

        StartCoroutine(DestroyLine());
    }

    private IEnumerator DestroyLine()
    {
        yield return new WaitForSeconds(0.2f);
        _line1.SetPosition(0, Vector3.zero);
        _line1.SetPosition(1, Vector3.zero);
        
        _line2.SetPosition(0, Vector3.zero);
        _line2.SetPosition(1, Vector3.zero);
        
        _line3.SetPosition(0, Vector3.zero);
        _line3.SetPosition(1, Vector3.zero);
    }
}
