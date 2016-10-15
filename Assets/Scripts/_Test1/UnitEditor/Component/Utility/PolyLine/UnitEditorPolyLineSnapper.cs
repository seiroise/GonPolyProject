using UnityEngine;
using Seiro.Scripts.Graphics.PolyLine2D.Snap;
using System.Collections.Generic;
using System;
using Seiro.Scripts.Graphics;

namespace Scripts._Test1.UnitEditor.Component.Utility.PolyLine {

	/// <summary>
	/// ユニットエディタのポリラインスナッパー
	/// </summary>
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public class UnitEditorPolyLineSnapper : UnitEditorPolyLineComponent {

		/// <summary>
		/// 優先度付きスナップ
		/// </summary>
		public class PrioritySnap {
			public int priority;
			public BaseSnap snap;

			public PrioritySnap(int priority, BaseSnap snap) {
				this.priority = priority;
				this.snap = snap;
			}
		}

		[Header("Parameter")]
		public Color color = Color.white;
		public float snapForce = 0.5f;			//スナップ有効範囲
		public bool initWithShow = true;		//起動と同時に表示

		private MeshFilter mf;
		private List<PrioritySnap> snaps;										//デフォルトも含む全てのスナップ
		private List<PrioritySnap> addSnaps;									//追加したスナップ
		private Dictionary<PrioritySnap, Action> addSnapCallbackDic;	//追加したスナップのコールバック
		private bool show = false;												//表示しているか
		private PrioritySnap prevSnap = null;									//前回のスナップ

		#region VirtualFunction

		/// <summary>
		/// 初期化
		/// </summary>
		public override void Initialize(UnitEditor unitEditor, UnitEditorPolyLine polyLine) {
			base.Initialize(unitEditor, polyLine);

			mf = GetComponent<MeshFilter>();
			snaps = new List<PrioritySnap>();
			addSnaps = new List<PrioritySnap>();
			addSnapCallbackDic = new Dictionary<PrioritySnap, Action>();

			//デフォルトスナップの追加
			AddDefaultSnap();

			//表示
			if(initWithShow) Show();
		}

		#endregion

		#region PrivateFunction

		/// <summary>
		/// デフォルトスナップの追加
		/// </summary>
		private void AddDefaultSnap() {
			snaps.Add(new PrioritySnap(0, new GridSnap(0.25f, 4, snapForce)));
		}

		/// <summary>
		/// 座標のスナップ。スナップした要素を返す
		/// </summary>
		private PrioritySnap Snap(List<PrioritySnap> snaps, Vector2 point, out Vector2 result) {
			result = Vector2.zero;

			if (snaps.Count == 0) {
				return null;
			}
			float minDistance = float.MaxValue;
			Vector2 sample;
			PrioritySnap snap = null;

			for (int i = 0; i < snaps.Count; ++i) {
				PrioritySnap e = snaps[i];
				if (e.snap.Snap(point, out sample)) {
					if (snap == null) {
						result = sample;
						snap = e;
						minDistance = (sample - point).magnitude;
					} else if (e.priority > snap.priority) {
						result = sample;
						snap = e;
						minDistance = (sample - point).magnitude;
					} else if (snap.priority == e.priority) {
						//距離を測る
						float distance = (sample - point).magnitude;
						if (minDistance > distance) {
							result = sample;
							snap = e;
							minDistance = distance;
						}
					}
				}
			}
			prevSnap = snap;
			return snap;
		}

		#endregion

		#region Function

		/// <summary>
		/// 補助線への座標のスナップ
		/// </summary>
		public bool Snap(Vector2 point, out Vector2 result) {
			PrioritySnap snap = Snap(snaps, point, out result);

			if (snap != null) {
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		/// スナップの追加
		/// </summary>
		public void AddSnap(int priority, BaseSnap snap, Action callback = null) {
			PrioritySnap pSnap = new PrioritySnap(priority, snap);
			snaps.Add(pSnap);
			addSnaps.Add(pSnap);
			if (callback != null) {
				addSnapCallbackDic.Add(pSnap, callback);
			}

			//表示
			if (show) Show();
		}

		/// <summary>
		/// スナップの表示
		/// </summary>
		public void Show() {
			//要素数確認
			int count = snaps.Count;
			if (count <= 0) return;

			//簡易メッシュ群の作成
			EasyMesh[] eMeshes = new EasyMesh[count];
			for (int i = 0; i < snaps.Count; ++i) {
				eMeshes[i] = snaps[i].snap.GetEasyMesh(color);
			}

			//描画
			mf.mesh = EasyMesh.ToMesh(eMeshes);
			show = true;
		}

		/// <summary>
		/// スナップの非表示
		/// </summary>
		public void Hide() {
			if (!show) return;
			mf.mesh = null;
			show = false;
		}

		/// <summary>
		/// 追加したスナップの消去
		/// </summary>
		public void ClearAddSnap() {
			int count = addSnaps.Count;
			if (count > 0) {
				for (int i = 0; i < count; ++i) {
					snaps.Remove(addSnaps[i]);
				}
			}
			addSnaps.Clear();
			addSnapCallbackDic.Clear();

			//表示の更新
			if(show) Show();
		}

		/// <summary>
		/// 前回のスナップのコールバック呼び出し
		/// </summary>
		public void CallPrevSnap() {
			if (prevSnap == null) return;
			//呼び出し
			if (addSnapCallbackDic.ContainsKey(prevSnap)) {
				addSnapCallbackDic[prevSnap]();
			}
		}

		#endregion
	}
}