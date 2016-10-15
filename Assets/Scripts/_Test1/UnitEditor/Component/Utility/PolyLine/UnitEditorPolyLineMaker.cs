using Seiro.Scripts.Graphics.PolyLine2D.Snap;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts._Test1.UnitEditor.Component.Utility.PolyLine {

	/// <summary>
	/// ユニットエディタのポリライン作成
	/// </summary>
	public class UnitEditorPolyLineMaker : UnitEditorPolyLineStateComponent {

		[Header("InputParameter")]
		public int addButton = 0;				//追加ボタン
		public int removeButton = 1;			//削除ボタン
		public KeyCode escKey = KeyCode.Escape;	//全削除/戻るボタン

		private UnitEditorPolyLineRenderer renderer;	//描画担当
		private UnitEditorPolyLineSnapper snapper;		//座標のスナップ担当

		private bool ended = false;				//終了フラグ
		private const float EPSILON = 0.01f;	//マウス移動差分の検出閾値

		[Header("Parameter")]
		public bool continuing = true;			//続けて描画する場合

		[Header("Callback")]
		public PolyLineEvent onMakeEnd;

		#region UnityEvent

		private void Update() {
			if (active) {
				InputCheck();
			}
		}

		#endregion

		#region VirtualFunction

		/// <summary>
		/// 遅延初期化
		/// </summary>
		public override void LateInitialize() {
			base.LateInitialize();

			//ユーティリティの取得
			renderer = polyLine.renderer;
			snapper = polyLine.snapper;
		}

		/// <summary>
		/// 開始
		/// </summary>
		public override void Enter() {
			base.Enter();

			ended = false;
		}

		#endregion

		#region Function

		/// <summary>
		/// 入力確認
		/// </summary>
		private void InputCheck() {
			//ボタン入力
			if (Input.GetMouseButtonDown(addButton)) {
				//追加
				Add();
			} else if (Input.GetMouseButtonDown(removeButton)) {
				//削除
				Remove();
			} else if (Input.GetKeyDown(escKey)) {
				//全削除/戻る
				Escape();
			} else {
				//移動量の確認
				CheckDelta();
			}
		}

		/// <summary>
		/// 追加
		/// </summary>
		private void Add() {
			Vector2 mPoint;
			if (!polyLine.GetMousePoint(out mPoint)) return;

			//例外検出
			//if (!polyLine.checker.AddCheck(renderer.GetVertices(), mPoint)) return;

			//スナップ
			Vector2 snapPoint;
			if (polyLine.snapper.Snap(mPoint, out snapPoint)) {
				mPoint = snapPoint;
				//クリック時のスナップのコールバックを呼ぶ
				snapper.CallPrevSnap();
				//その結果終了フラグが立った場合
				if (ended) {
					//終了
					End(false);
					return;
				}
			}

			//追加
			renderer.Add(mPoint);
			SetSnap();
		}

		/// <summary>
		/// 削除
		/// </summary>
		private void Remove() {
			//削除
			renderer.RemoveLast();
			//スナップの設定
			SetSnap();
			SetNoticeLine();
		}

		/// <summary>
		/// 戻る
		/// </summary>
		private void Escape() {

			//頂点数が0確認
			if (renderer.GetVertexCount() == 0) {
				//終了
				End(true);
			} else {
				//全削除するが終了はしない
				renderer.Clear();
				SetSnap();
			}
		}

		/// <summary>
		/// 移動量の確認
		/// </summary>
		private void CheckDelta() {
			//差分確認
			Vector2 mDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
			if (mDelta.magnitude > EPSILON) {
				SetNoticeLine();
			}
		}

		/// <summary>
		/// 予告線の設定
		/// </summary>
		private void SetNoticeLine() {
			int count = renderer.GetVertexCount();
			if (count == 0) {
				renderer.SetSubVertices(null);
				return;
			}
			//予告線の更新
			Vector2 mPoint;
			polyLine.GetMousePoint(out mPoint);
			//スナップ
			Vector2 snapPoint;
			if (snapper.Snap(mPoint, out snapPoint)) {
				mPoint = snapPoint;
			}
			//予告線の描画
			renderer.SetSubVertices(renderer.GetVertex(count - 1), mPoint);
		}

		/// <summary>
		/// スナップの設定
		/// </summary>
		private void SetSnap() {
			int count = renderer.GetVertexCount();
			float snapForce = 0.5f;

			//追加したスナップのクリア
			snapper.ClearAddSnap();

			//終了スナップ
			if (count >= 2) {
				Vector2 start = renderer.GetVertex(0);
				snapper.AddSnap(10, new PointSnap(start, snapForce), OnSnapEndPoint);
			}
		}

		/// <summary>
		/// 終了
		/// </summary>
		private void End(bool forceEnd) {
			//コールバック
			List<Vector2> vertices = renderer.GetVertices();
			onMakeEnd.Invoke(vertices.Count == 0 ? null : vertices);

			//描画
			renderer.Clear();

			//スナップ
			SetSnap();

			//無効化
			if (forceEnd || !continuing) {
				polyLine.DisactivateStateComponent();
			}

			//終了フラグを下ろす
			ended = false;
		}

		#endregion

		#region Callback

		/// <summary>
		/// 終了地点へのスナップ
		/// </summary>
		private void OnSnapEndPoint() {
			ended = true;
		}

		#endregion

	}
}