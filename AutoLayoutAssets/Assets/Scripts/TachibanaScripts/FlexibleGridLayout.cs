using UnityEngine;

public class FlexibleGridLayout : MonoBehaviour {
    public float padding;
    public float spacing;

    private void Start() {
        ArrangeItems();
    }

    private void ArrangeItems() {
        var parentWidth = GetComponent<RectTransform>().rect.width;
        var yOffset = 0f;
        var nextRowYOffset = 0f;
        var currentXOffset = 0f;

        foreach (RectTransform child in transform) {
            if (!child.gameObject.activeSelf) continue;

            var childWidth = child.rect.width;
            var childHeight = child.rect.height;

            if (currentXOffset + childWidth + padding > parentWidth) {
                currentXOffset = 0f;
                yOffset -= (nextRowYOffset + spacing);
                nextRowYOffset = 0;
            }

            child.anchoredPosition = new Vector2(currentXOffset, yOffset);
            currentXOffset += childWidth + spacing;

            if (childHeight > nextRowYOffset) nextRowYOffset = childHeight;
        }
    }
}