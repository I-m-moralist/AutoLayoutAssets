using UnityEngine;

public class CircularLayout : MonoBehaviour {
    // 配置するオブジェクト
    public GameObject[] layoutObjectPrefabs;
    
    // 配置方向
    public bool isClockwise = true;

    public float prefabMargin = 0.8f;
    
    // 半径
    private float radius;

    private void Start() {
        // アタッチされているオブジェクトのRectTransformからサイズを取得し、その半分の値を半径として設定
        var rectTransform = GetComponent<RectTransform>();
        radius = Mathf.Max(rectTransform.rect.width, rectTransform.rect.height) * 0.5f;
        LayoutObjectsInCircle();
    }

    private void LayoutObjectsInCircle() {
        var angleStep = 360f / layoutObjectPrefabs.Length;

        for (var i = 0; i < layoutObjectPrefabs.Length; i++) {
            float angle;
            if(isClockwise) {
                angle = 90 - angleStep * i;
            }
            else {
                angle = angleStep * i + 90;
            }
            
            // プレハブのRectTransformからサイズを取得し、その半分の値を半径に追加
            var prefabRectTransform = layoutObjectPrefabs[i].GetComponent<RectTransform>();
            var prefabRadius = Mathf.Max(prefabRectTransform.rect.width, prefabRectTransform.rect.height) * 0.5f;

            var position = CalculateObjectPosition(angle, prefabRadius, prefabRadius);

            // プレハブからオブジェクトを生成して配置
            var layoutObject = Instantiate(layoutObjectPrefabs[i], this.transform);
            layoutObject.transform.localPosition = position;
            layoutObject.transform.localScale = Vector3.one;
        }
    }

    private Vector3 CalculateObjectPosition(float angle, float additionalRadius, float prefabRadius) {
        var margin = prefabRadius * prefabMargin;
        var totalRadius = radius + additionalRadius + margin;
        var x = totalRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
        var y = totalRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
        var position = new Vector3(x, y, 0f);
        return position;
    }
}
