using Scripts._Test1.UnitEditor.Component.Control.Select;
using Scripts._Test1.UnitEditor.Component.StateMachine;
using Scripts._Test1.UnitEditor.Component.Utility.Marker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts._Test1.UnitEditor.Component.Utility.PolyLine {
	
	/// <summary>
	/// ユニットエディタのポリライン調整
	/// </summary>	
	public class UnitEditorPolyLineAdjuster : UnitEditorState {

		/// <summary>
		/// 調整モード
		/// </summary>
		public enum Mode {
			None,
			Adjust,
			Remove
		}

		[Header("InputParameter")]
		public KeyCode escKey = KeyCode.Escape;	//終了キー
		public int removeModeButton = 1;		//削除モードボタン

		[Header("Marker")]
		public UnitEditorMarker marker;
		public string vertexMarker = "VertexMarker";
		public string addMarker = "AddMarker";
		public string removeMarker = "RemoveMarker";
		private List<SpriteMarker> markers;

		[Header("Callback")]
		public PolyLineEvent onEndAdjust;
		public PolyLineEvent onChangeVertex;

		//PolyLineコンポーネント
		private UnitEditorPolyLine polyLine;
		private UnitEditorPolyLineRenderer renderer;	//描画担当
		private UnitEditorPolyLineSnapper snapper;		//座標のスナップ担当

		private const float EPSILON = 0.01f;	//マウス移動差分の検出閾値

		//Mode
		private Mode mode = Mode.Adjust;		//モード

		//Adjust
		private List<Vector2> adjustVertices;   //調整頂点群
		private int adjustVertsCount;           //調整頂点群の数
		private int adjustIndex;                //調整頂点番号
		private bool adjusting = false;         //調整中

		//Connected
		private bool connected;                 //先頭と末尾の接続
		private bool adjustConnected;           //接続点の調整

		#region UnitEvent

		private void Update() {
			if (!activated) return;
			InputCheck();
		}

		#endregion

		#region VirtualFunction

		/// <summary>
		/// 初期化
		/// </summary>
		public override void Initialize(UnitEditor unitEditor, UnitEditorStateMachine owner) {
			base.Initialize(unitEditor, owner);

			polyLine = (UnitEditorPolyLine)owner;

			markers = new List<SpriteMarker>();
		}

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

			//調整用頂点が設定されていなければ無効化
			if (adjustVertices == null) {
				owner.DisactivateState();
			} else {
				//線の表示
				renderer.SetVertices(adjustVertices);
				//調整モード開始
				StartAdjustMode();
			}
		}

		/// <summary>
		/// 終了
		/// </summary>
		public override void Exit() {
			base.Exit();

			if (adjusting) {
				adjusting = false;
			}
			if (mode == Mode.Remove) {
				mode = Mode.Adjust;
			}

			//コールバック
			onEndAdjust.Invoke(renderer.GetVertices());

			//表示していたものをクリア
			HideMarkers();
			renderer.Clear();

			adjustVertices = null;
		}

		#endregion

		#region PrivateFunction

		/// <summary>
		/// 入力確認
		/// </summary>
		private void InputCheck() {

			switch (mode) {
				//調整
				case Mode.Adjust:
					if (adjusting) {
						Vector2 mDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
						if (mDelta.magnitude > EPSILON) {
							SetNoticeLine();
						}
						if (Input.GetMouseButtonUp(0)) {
							Vector2 point;
							if (polyLine.GetMousePoint(out point)) {
								Vector2 snapPoint;
								if (snapper.Snap(point, out snapPoint)) {
									point = snapPoint;
								}
								EndVertexMove(point);
							}
						}
					} else {
						if (Input.GetMouseButtonDown(removeModeButton)) {
							StartRemoveMode();
						}
					}
					break;

				//削除
				case Mode.Remove:
					if (Input.GetMouseButtonDown(removeModeButton)) {
						StartAdjustMode();
					}
					break;
			}
			//終了
			if (Input.GetKeyDown(escKey)) {
				owner.DisactivateState();
			}
		}

		/// <summary>
		/// 予告線の設定
		/// </summary>
		private void SetNoticeLine() {

			//マウス座標
			Vector2 mPoint;
			polyLine.GetMousePoint(out mPoint);
			//スナップ
			Vector2 snapPoint;
			if (snapper.Snap(mPoint, out snapPoint)) {
				mPoint = snapPoint;
			}

			//予告線の描画
			if (adjustConnected) {
				//接続点の変更
				Vector2 p1 = adjustVertices[1];
				Vector2 p2 = adjustVertices[adjustVertsCount - 2];
				renderer.SetSubVertices(p1, mPoint, p2);
			} else {
				Vector2 p1 = adjustVertices[(adjustIndex + adjustVertsCount - 1) % adjustVertsCount];
				Vector2 p2 = adjustVertices[(adjustIndex + 1) % adjustVertsCount];
				renderer.SetSubVertices(p1, mPoint, p2);
			}
		}

		/// <summary>
		/// 頂点移動開始
		/// </summary>
		private void StartVertexMove(List<Vector2> vertices, int index) {
			adjustVertices = vertices;
			adjustVertsCount = adjustVertices.Count;
			adjustIndex = index;

			//接続している場合
			if (connected) {
				adjustConnected = (index == 0 || index == adjustVertsCount - 1);
			}
			HideMarkers();
			SetNoticeLine();
			adjusting = true;
		}

		/// <summary>
		/// 頂点移動終了
		/// </summary>
		private void EndVertexMove(Vector2 movedPoint) {
			//頂点座標の変更
			if (adjustConnected) {
				int prevLast = adjustVertsCount - 1;
				renderer.Change(0, movedPoint);
				renderer.Change(prevLast, movedPoint);
				adjustVertices[0] = movedPoint;
				adjustVertices[prevLast] = movedPoint;
			} else {
				renderer.Change(adjustIndex, movedPoint);
				adjustVertices[adjustIndex] = movedPoint;
			}

			renderer.ClearSubLine();
			ShowAdjustMarkers(adjustVertices);
			adjusting = false;

			//コールバック
			onChangeVertex.Invoke(renderer.GetVertices());
		}

		/// <summary>
		/// 調整モード開始
		/// </summary>
		private void StartAdjustMode() {
			if (mode == Mode.Adjust) return;
			mode = Mode.Adjust;
			HideMarkers();
			ShowAdjustMarkers(renderer.GetVertices());
		}

		/// <summary>
		/// 削除モード開始
		/// </summary>
		private void StartRemoveMode() {
			if (mode == Mode.Remove) return;
			mode = Mode.Remove;
			HideMarkers();
			ShowRemoveMarkers(renderer.GetVertices());
		}

		/// <summary>
		/// 頂点の挿入
		/// </summary>
		private void InsertVertex(int index, Vector2 point) {
			Debug.Log("InserVertex");
			renderer.Insert(index, point);
			StartVertexMove(renderer.GetVertices(), index);
		}

		/// <summary>
		/// 頂点の削除
		/// </summary>
		private void RemoveVertex(int index) {
			int count = renderer.GetVertexCount();
			if (connected && count <= 4) {
				return;
			} else if (count == 1) {
				//最後の頂点を削除
				Exit();
			}

			//削除処理
			if (index == 0 && connected) {
				Vector2 point = renderer.GetVertex(1);
				renderer.Change(count - 1, point);
			}
			renderer.Remove(index);

			HideMarkers();
			ShowRemoveMarkers(renderer.GetVertices());

			//コールバック
			onChangeVertex.Invoke(renderer.GetVertices());
		}

		#endregion

		#region MarkerFunction

		/// <summary>
		/// 頂点リストから調整用マーカーの表示
		/// </summary>
		private void ShowAdjustMarkers(List<Vector2> vertices) {
			int count = vertices.Count;
			//中点マーカー
			for (int i = 0; i < count - 1; ++i) {
				markers.Add(InstantiateAddMarker((i + 1).ToString(), vertices[i], vertices[i + 1]));
			}
			//頂点マーカー
			if (connected) --count;
			for (int i = 0; i < count; ++i) {
				markers.Add(InstantiateAdjustMarker(i.ToString(), vertices[i]));
			}
		}

		/// <summary>
		/// 頂点リストから削除用マーカーの表示
		/// </summary>
		private void ShowRemoveMarkers(List<Vector2> vertices) {
			int count = vertices.Count;
			//頂点マーカー
			if (connected) --count;
			for (int i = 0; i < count; ++i) {
				markers.Add(InstantiateRemoveMarker(i.ToString(), vertices[i]));
			}
		}

		/// <summary>
		/// マーカーの非表示
		/// </summary>
		private void HideMarkers() {
			for (int i = markers.Count - 1; i >= 0; --i) {
				markers[i].gameObject.SetActive(false);
				markers.RemoveAt(i);
			}
		}

		/// <summary>
		/// 調整マーカーの生成
		/// </summary>
		private SpriteMarker InstantiateAdjustMarker(string markerName, Vector2 point) {
			if (!marker) return null;
			//マーカーの取得
			SpriteMarker m = marker.PopMarker(vertexMarker, point);
			m.gameObject.name = markerName;
			m.gameObject.SetActive(true);
			//コールバックの設定
			m.onDown.RemoveListener(OnVertexMarkerDown);
			m.onDown.AddListener(OnVertexMarkerDown);

			return m;
		}

		/// <summary>
		/// 追加マーカーの生成
		/// </summary>
		private SpriteMarker InstantiateAddMarker(string markerName, Vector2 p1, Vector2 p2) {
			if (!marker) return null;
			Vector2 point = (p2 - p1) * 0.5f + p1;
			//マーカーの取得
			SpriteMarker m = marker.PopMarker(addMarker, point);
			m.gameObject.name = markerName;
			m.gameObject.SetActive(true);
			//コールバックの設定
			m.onDown.RemoveListener(OnAddMarkerDown);
			m.onDown.AddListener(OnAddMarkerDown);
			return m;
		}

		/// <summary>
		/// 削除マーカーの生成
		/// </summary>
		private SpriteMarker InstantiateRemoveMarker(string markerName, Vector2 point) {
			if (!marker) return null;
			//マーカーの取得
			SpriteMarker m = marker.PopMarker(removeMarker, point);
			m.gameObject.name = markerName;
			m.gameObject.SetActive(true);
			//コールバックの設定
			m.onDown.RemoveListener(OnRemoveMarkerDown);
			m.onDown.AddListener(OnRemoveMarkerDown);
			return m;
		}
	
		#endregion

		#region PublicFunction

		/// <summary>
		/// 調整する頂点群を設定する。connectedは先頭と末尾の接続しているか
		/// Activateする前にこれを呼ぶ必要あり
		/// </summary>
		public void SetVertices(List<Vector2> vertices, bool connected = false) {
			adjustVertices = vertices;
			this.connected = connected;
			mode = Mode.None;
		}

		#endregion

		#region MarkerCallback

		/// <summary>
		/// 頂点マーカーの押下
		/// </summary>
		private void OnVertexMarkerDown(GameObject gObj) {
			int index;
			if (!int.TryParse(gObj.name, out index)) return;
			StartVertexMove(renderer.GetVertices(), index);
		}

		/// <summary>
		/// 追加マーカーの押下
		/// </summary>
		private void OnAddMarkerDown(GameObject gObj) {
			int index;
			if (!int.TryParse(gObj.name, out index)) return;
			//頂点の挿入
			InsertVertex(index, gObj.transform.localPosition);
		}

		/// <summary>
		/// 削除マーカーの押下
		/// </summary>
		private void OnRemoveMarkerDown(GameObject gObj) {
			int index;
			if (!int.TryParse(gObj.name, out index)) return;
			//頂点の挿入
			RemoveVertex(index);
		}

		#endregion
	}
}