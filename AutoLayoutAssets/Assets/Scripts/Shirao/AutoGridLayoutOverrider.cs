using UnityEngine;
using UnityEngine.UI;

namespace CustomUIParts {
    /// <summary>
    /// UnityEngine.UI.GridLayoutGroup をAutoLayout仕様にする
    /// </summary>
    [RequireComponent(typeof(GridLayoutGroup))]
    public class AutoGridLayoutOverrider : MonoBehaviour {
        /// <summary>
        /// オートレイアウト対応するGridLayoutGroup
        /// </summary>
        [SerializeField] GridLayoutGroup grid = default;

        /// <summary>
        /// サイズ調整に使用するRectTransform
        /// </summary>
        RectTransform rect = default;

        #region サイズ変動要素の比較用キャッシュ

        // Grid内オブジェクトの個数
        int beforeChildCount;

        // RectTransformからのデータ
        float beforeWidth, beforeHeight;

        // GridLayoutGroupからのデータ
        Vector2 beforeCellSize;
        Vector2 beforeSpacing;
        GridLayoutGroup.Axis beforeAxis;
        GridLayoutGroup.Constraint beforeConstraint;
        int beforeConstraingCount;

        #endregion


        /// <summary>
        /// InspectorでのReset動作定義
        /// </summary>
        void Reset() {
            grid = GetComponent<GridLayoutGroup>();
        }

        /// <summary>
        /// Unityライフサイクル関数
        /// </summary>
        void Awake() {
            // サイズ調整の基準となるRectTransformを取得
            rect = GetComponent<RectTransform>();
            while (rect.rect.width == 0 && rect.rect.height == 0) {
                // Streach前提でサイズが取得できないときは、親・祖先オブジェクトに遡って取得しにいく
                rect = rect.transform.parent.GetComponent<RectTransform>();
            }
        }

        /// <summary>
        /// Unityライフサイクル関数
        /// </summary>
        void Update() {
            ResetLayoutSize();
        }

        /// <summary>
        /// レイアウトサイズ変更
        /// </summary>
        void ResetLayoutSize() {
            // レイアウト状態に変更が無ければスキップ
            bool skipFlag =
                beforeWidth == rect.rect.width &&
                beforeHeight == rect.rect.height &&
                beforeChildCount == grid.transform.childCount &&
                beforeSpacing.x == grid.spacing.x &&
                beforeSpacing.y == grid.spacing.y &&
                beforeAxis == grid.startAxis &&
                beforeConstraint == grid.constraint;

            // 幅固定指定の場合は、前回サイズから変更がないか追加チェック
            switch (grid.constraint) {
                case GridLayoutGroup.Constraint.FixedColumnCount:
                    skipFlag &= beforeCellSize.y == grid.cellSize.y;
                    skipFlag &= beforeConstraingCount == grid.constraintCount;
                    break;
                case GridLayoutGroup.Constraint.FixedRowCount:
                    skipFlag &= beforeCellSize.x == grid.cellSize.x;
                    skipFlag &= beforeConstraingCount == grid.constraintCount;
                    break;
            }

            if (skipFlag) {
                return;
            }

            // 何かしら状況が変わったのでレイアウト設定を変える

            float x = grid.cellSize.x;
            float y = grid.cellSize.y;

            switch (grid.constraint) {
                // x,y:ともに可変（領域全体を埋める）
                case GridLayoutGroup.Constraint.Flexible:
                    int childCount = grid.transform.childCount;
                    int i = 0;
                    while (Mathf.Pow(++i, 2) < childCount) {
                    }

                    int tateNum, yokoNum;
                    if (Mathf.Pow(i, 2) == childCount) {
                        tateNum = yokoNum = i;
                    }
                    else if (grid.startAxis == GridLayoutGroup.Axis.Horizontal) {
                        yokoNum = i;
                        tateNum = childCount / i + (childCount % i != 0 ? 1 : 0);
                    }
                    else {
                        // 縦方向優先に並べる動作は挙動が特殊
                        yokoNum = i;
                        if (childCount <= i * (i - 1)) {
                            tateNum = childCount / i + (childCount % i != 0 ? 1 : 0);
                        }
                        else {
                            tateNum = i;
                        }
                    }

                    x = (rect.rect.width - grid.spacing.x * (tateNum - 1)) / tateNum;
                    y = (rect.rect.height - grid.spacing.y * (yokoNum - 1)) / yokoNum;
                    break;

                // width のみ可変
                case GridLayoutGroup.Constraint.FixedColumnCount:
                    x = (rect.rect.width - grid.spacing.x * (grid.constraintCount - 1)) / grid.constraintCount;
                    break;

                // height のみ可変
                case GridLayoutGroup.Constraint.FixedRowCount:
                    y = (rect.rect.height - grid.spacing.y * (grid.constraintCount - 1)) / grid.constraintCount;
                    break;
            }

            beforeCellSize = grid.cellSize = new Vector2(x, y);

            // 更新処理スキップ判定用に諸々キャッシュ
            beforeChildCount = grid.transform.childCount;

            beforeWidth = rect.rect.width;
            beforeHeight = rect.rect.height;

            beforeSpacing = grid.spacing;
            beforeAxis = grid.startAxis;
            beforeConstraint = grid.constraint;
            beforeConstraingCount = grid.constraintCount;
        }
    }
}