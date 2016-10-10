using UnityEngine;
using Scripts._Test.PolyPartsEditor.Common;
using Seiro.Scripts.Graphics.PolyLine2D;

namespace Scripts._Test.PolyPartsEditor.Component.Adjuster {
	
	/// <summary>
	/// ポリゴンパーツエディタの座標調整
	/// </summary>
	class PolyPartsPositionAdjuster : PolyPartsAdjusterComponent {

		public PolyLine2DEditor polyLineEditor;

		private bool active = false;
		private PolyPartsObject selected;

		private bool dragged = false;
		private Vector3 offset;

		#region UnityEvent

		private void Update() {
			UpdateDrag();
		}

		#endregion

		#region VirtualFunction

		/// <summary>
		/// 初期化
		/// </summary>
		public override void Initialize(PolyPartsEditor editor, PolyPartsAdjuster adjuster) {
			base.Initialize(editor, adjuster);

			//コールバックの設定
			adjustMenu.moveBtn.onClick.RemoveListener(OnMoveButtonClicked);
			adjustMenu.moveBtn.onClick.AddListener(OnMoveButtonClicked);
		}

		/// <summary>
		/// 開始
		/// </summary>
		public override void Enter() {
			base.Enter();
			//コンポーネントを活性化
			adjuster.ActivateComponent(this);
			//選択ポリゴンの取得
			selected = adjuster.GetSelected();
			//コールバックの設定
			selected.onDown.AddListener(OnDown);
			selected.onUp.AddListener(OnUp);

			active = true;
		}

		/// <summary>
		/// 終了
		/// </summary>
		public override void Exit() {
			base.Exit();
			//コールバックの設定
			selected.onDown.RemoveListener(OnDown);
			selected.onUp.RemoveListener(OnUp);

			active = false;
		}

		#endregion

		#region Function

		/// <summary>
		/// ドラッグ開始
		/// </summary>
		private void StartDrag() {
			
			//座標の取得
			Vector2 point;
			if (polyLineEditor.GetMousePoint(out point)) {
				//スナップ
				polyLineEditor.supporter.Clear();
				polyLineEditor.supporter.AddDefaultSnap();
				polyLineEditor.supporter.Draw();
				Vector2 snapPoint;
				if (polyLineEditor.supporter.Snap(point, out snapPoint)) {
					point = snapPoint;
				}

				//オフセット
				offset = (Vector3)point - selected.transform.localPosition;

				dragged = true;
			}			
		}

		/// <summary>
		/// ドラッグ終了
		/// </summary>
		private void EndDrag() {
			//スナップ
			polyLineEditor.supporter.Clear();
			polyLineEditor.supporter.Draw();

			dragged = false;
		}

		/// <summary>
		/// ドラッグ更新
		/// </summary>
		private void UpdateDrag() {
			if (!dragged) return;
			
			//座標取得
			Vector2 point;
			if (polyLineEditor.GetMousePoint(out point)) {
				//スナップ
				Vector2 snapPoint;
				if (polyLineEditor.supporter.Snap(point, out snapPoint)) {
					point = snapPoint;
				}
				//位置計算
				selected.transform.localPosition = (Vector3)point - offset;
			}
		}

		#endregion

		#region Callback

		/// <summary>
		/// ポインタ押下
		/// </summary>
		private void OnDown(PolyPartsObject polyObj) {
			StartDrag();

		}

		/// <summary>
		/// ポインタ押上
		/// </summary>
		private void OnUp(PolyPartsObject polyObj) {
			EndDrag();
		}

		#endregion

		#region UICallback

		/// <summary>
		/// 座標調整ボタンのクリック
		/// </summary>
		private void OnMoveButtonClicked() {
			Enter();
		}

		#endregion
	}
}