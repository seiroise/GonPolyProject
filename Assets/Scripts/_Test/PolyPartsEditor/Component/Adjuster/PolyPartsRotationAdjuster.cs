using UnityEngine;
using Seiro.Scripts.Graphics.PolyLine2D;
using Scripts._Test.PolyPartsEditor.Common;
using Seiro.Scripts.Geometric;
using Seiro.Scripts.Graphics.PolyLine2D.Snap;

namespace Scripts._Test.PolyPartsEditor.Component.Adjuster {

	/// <summary>
	/// ポリゴンパーツエディタの角度調整
	/// </summary>
	class PolyPartsRotationAdjuster : PolyPartsAdjusterComponent {

		public PolyLine2DEditor polyLineEditor;

		private bool active = false;
		private PolyPartsObject selected;

		private bool controlled = false;
		private Vector2 point;

		#region UnityEvent

		private void Update() {
			UpdateControl();
		}

		#endregion

		#region VirtualFunction

		/// <summary>
		/// 初期化
		/// </summary>
		public override void Initialize(PolyPartsEditor editor, PolyPartsAdjuster adjuster) {
			base.Initialize(editor, adjuster);

			//コールバックの設定
			adjustMenu.rotateBtn.onClick.RemoveListener(OnRotateButtonClicked);
			adjustMenu.rotateBtn.onClick.AddListener(OnRotateButtonClicked);
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
		/// 操作開始
		/// </summary>
		private void StartControl() {

			//座標の取得
			if (polyLineEditor.GetMousePoint(out point)) {
				//スナップ
				polyLineEditor.supporter.Clear();
				polyLineEditor.supporter.AddSnap(10, new RadialSnap(point, 8, 1f));
				polyLineEditor.supporter.Draw();

				controlled = true;
			}
		}
		
		/// <summary>
		/// 操作終了
		/// </summary>
		private void EndControl() {
			//スナップ
			polyLineEditor.supporter.Clear();
			polyLineEditor.supporter.Draw();

			controlled = false;
		}

		/// <summary>
		/// 操作更新
		/// </summary>
		private void UpdateControl() {
			if (!controlled) return;

			//座標取得
			Vector2 mPoint;
			if (polyLineEditor.GetMousePoint(out mPoint)) {
				//スナップ
				Vector2 snapPoint;
				if (polyLineEditor.supporter.Snap(mPoint, out snapPoint)) {
					mPoint = snapPoint;
				}
				//角度計算
				float angle = GeomUtil.TwoPointAngle(mPoint, point);
				selected.transform.localEulerAngles = new Vector3(0f, 0f, angle);
			}
		}

		#endregion

		#region Callback

		/// <summary>
		/// ポインタ押下
		/// </summary>
		private void OnDown(PolyPartsObject polyObj) {
			StartControl();
		}

		/// <summary>
		///ポインタ押上
		/// </summary>
		private void OnUp(PolyPartsObject polyObj) {
			EndControl();
		}

		#endregion

		#region UICallback

		/// <summary>
		/// 角度調整ボタンのクリック
		/// </summary>
		private void OnRotateButtonClicked() {
			Enter();
		}

		#endregion
	}
}