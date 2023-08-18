using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VariableGridHorizontal : MonoBehaviour {

    [SerializeField] private float spacingX = 0;
    [SerializeField] private float spacingY = 0;
    [SerializeField] private float childHeight = 88;
    
    // Start is called before the first frame update
    void Start() {
        //RectTransform panelRect = GetComponent<RectTransform>();
        var screenWidth = Screen.width;//panelRect.sizeDelta.x);

        float x = 0;
        float y = 0;
        var children = GetComponentInChildren<RectTransform>();
        //子要素がいなければ終了
        if (children.childCount == 0) {
            return;
        }
        foreach(RectTransform ob in children) {
            // 配置するオブジェクトが画面サイズを超えそうだったら改行
            if (x + ob.rect.width * ob.localScale.x > screenWidth) {
                x = 0;
                y += -(childHeight + spacingY);
            }
            ob.anchoredPosition = new Vector2(x, y);
            // TODO スケールとの兼ね合い
            // ob.sizeDelta = new Vector2(ob.rect.width, childHeight);
            // 次の子オブジェクトの横方向の配置位置を計算
            x += ob.rect.width * ob.localScale.x + spacingX;
        }
    }

    // Update is called once per frame
    void Update() {
    }
}