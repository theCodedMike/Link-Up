using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Done_UpGrade : MonoBehaviour {

    public Sprite[] upgradeSprites;
    public string upgradeName = "";

    private void Awake()
    {
        Sprite icon = upgradeSprites[Random.Range(0, upgradeSprites.Length)];//随机获取贴图
        upgradeName = icon.ToString();//道具名称的储存
        this.gameObject.GetComponent<SpriteRenderer>().sprite = icon;//贴图
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject, 0.8f);//销毁道具
    }
}
