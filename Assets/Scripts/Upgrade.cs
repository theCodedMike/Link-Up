using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Upgrade : MonoBehaviour
{
    public Sprite[] upgradeSprites;
    [HideInInspector]
    public string upgradeName;
    
    
    public void SetParam()
    {
        Sprite icon = upgradeSprites[Random.Range(0, upgradeSprites.Length)];
        upgradeName = icon.name;
        GetComponent<SpriteRenderer>().sprite = icon;
        StartCoroutine(DestroyObj());
    }

    private IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
}
