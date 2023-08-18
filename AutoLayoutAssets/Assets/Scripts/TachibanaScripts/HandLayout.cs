using UnityEngine;

public class HandLayout : MonoBehaviour {
    
    // カードのプレハブ
    public GameObject cardPrefab;
    
    // カードの数
    public int numberOfCards = 5;
    
    // 扇形の最大の回転角度
    public float maxRotationAngle = 45f;

    void Start() {
        LayoutCards();
    }

    void LayoutCards() {
        
        // カードプレハブの幅と高さを取得
        var cardWidth = cardPrefab.GetComponent<RectTransform>().rect.width;
        var cardHeight = cardPrefab.GetComponent<RectTransform>().rect.height;
        var radius = cardHeight / 2f / Mathf.Tan(Mathf.Deg2Rad * maxRotationAngle / 2f);

        var angleStep = maxRotationAngle * 2 / (numberOfCards - 1);
        var startAngle = -maxRotationAngle;

        for (var i = 0; i < numberOfCards; i++)
        {
            // カードのインスタンス化
            var cardInstance = Instantiate(cardPrefab, transform);
            
            // カードのローカル回転を設定
            var rotationAngle = startAngle + angleStep * i;
            cardInstance.transform.localRotation = Quaternion.Euler(0, 0, -rotationAngle);

            // 回転の後、カードの位置を半径に基づいて調整
            var y = -radius + (radius * Mathf.Cos(Mathf.Deg2Rad * rotationAngle));
            var x = radius * Mathf.Sin(Mathf.Deg2Rad * rotationAngle);
            
            cardInstance.transform.localPosition = new Vector3(x, y, 0);
        }
    }
}