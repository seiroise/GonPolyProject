using Seiro.Scripts.EventSystems;
using UnityEngine;
using System.Collections.Generic;
using Seiro.Scripts.Geometric;
using Seiro.Scripts.Geometric.Polygon.Concave;
using Seiro.Scripts.Graphics;
using Seiro.Scripts.Utility;

namespace Scripts._Test1.UnitEditor.Common.Parts {
	
	/// <summary>
	/// ユニットを構成するパーツのオブジェクト
	/// </summary>
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
	public class PartsObject : MonoBehaviour, ICollisionEventHandler {

		[Header("Polygon")]
		public Color polygonColor = Color.white;	//デフォルトの色
		[Range(0f, 1f)]
		public float disableColorScale = 0.75f;		//無効化時の色倍率
		public float polygonColorLerpT = 10f;		//変化率
		private LerpColor lerpPolygonColor;			//色変更用

		[Header("Outline")]
		public float outlineWidth = 0.1f;			//アウトラインの色
		public float outlineColorLerpT = 10f;		//変化率
		public Color overOutlineColor = new Color(0.75f, 0.5f, 0f);
		public Color clickOutlineColor = new Color(0.1f, 0.25f, 0f);
		private Color normalOutlineColor = new Color(0f, 0f, 0f, 0f);
		private LerpColor lerpOutlineColor;

		[Header("Callback")]
		public PartsEvent onDown;
		public PartsEvent onUp;
		public PartsEvent onClick;

		//形状データ
		private List<Vector2> vertices;		//頂点
		private Rect inclusionRect;			//包括矩形
		private ConcavePolygon polygon;		//ポリゴンデータ

		//簡易メッシュ
		private EasyMesh drawEMesh;			//表示用メッシュ
		private EasyMesh subEMesh;			//サブメッシュ
		private EasyMesh[] drawEMeshes;		//描画用メッシュ領域

		//描画
		private bool draw;					//描画フラグ

		//無効化
		private bool disabled = false;

		//コンポーネント
		private MeshFilter mf;
		private MeshCollider mc;

		//ポインタイベント
		private bool overed = false;		//被っているか

		#region Update

		private void Awake() {
			mf = GetComponent<MeshFilter>();
			mc = GetComponent<MeshCollider>();
			lerpPolygonColor = new LerpColor(polygonColor, polygonColor);
			lerpOutlineColor = new LerpColor(normalOutlineColor, normalOutlineColor);
		}

		private void Update() {
			if (vertices == null) return;
			UpdatePolygon();
			UpdateOutline();

			Draw();
		}

		#endregion

		#region PrivateFunction

		/// <summary>
		/// 描画
		/// </summary>
		private void Draw() {
			if (!draw) return;
			//描画用簡易ポリゴンの作成
			drawEMeshes[0] = drawEMesh;	//本体
			drawEMeshes[1] = EasyMesh.MakePolyLine2D(vertices, outlineWidth, lerpOutlineColor.Value);	//アウトライン
			Mesh mesh = EasyMesh.ToMesh(drawEMeshes);
			//描画
			mf.mesh = mesh;
			if (mc) {
				if (!disabled) {
					mc.sharedMesh = mesh;
				} else {
					mc.sharedMesh = null;
				}
			}
			draw = false;
		}

		/// <summary>
		/// ポリゴンの更新
		/// </summary>
		private void UpdatePolygon() {
			if (!lerpPolygonColor.Processing) return;
			lerpPolygonColor.Update(polygonColorLerpT * Time.deltaTime);
			//簡易メッシュの色変更
			drawEMesh.SetColor(lerpPolygonColor.Value);
			draw = true;
		}

		/// <summary>
		/// アウトラインの更新
		/// </summary>
		private void UpdateOutline() {
			if (!lerpOutlineColor.Processing) return;
			lerpOutlineColor.Update(outlineColorLerpT * Time.deltaTime);
			draw = true;
		}

		#endregion

		#region PublicFunction

		/// <summary>
		/// 頂点の設定。始点と終点が結ばれていること
		/// </summary>
		public void SetVertices(List<Vector2> vertices, Color color) {
			//包括矩形から原点からのオフセットを求め適用する
			Rect rect = GeomUtil.CalculateRect(vertices);
			for (int i = 0; i < vertices.Count; ++i) {
				vertices[i] -= rect.center;
			}
			this.vertices = vertices;
			//座標をずらす
			transform.localPosition = rect.center;
			//改めて包括矩形を求める
			inclusionRect = GeomUtil.CalculateRect(vertices);
			//ポリゴンの生成
			//末尾を削除(一時的)
			vertices.RemoveAt(vertices.Count - 1);
			polygon = new ConcavePolygon(vertices);
			//末尾に先頭を追加
			vertices.Add(vertices[0]);

			//簡易メッシュの確保
			drawEMesh = polygon.ToEasyMesh(color);
			subEMesh = polygon.ToEasyMesh(color);

			//描画用メッシュの領域確保
			drawEMeshes = new EasyMesh[2];

			//色
			SetPolygonColor(color);

			draw = true;
		}

		/// <summary>
		/// 頂点の設定。始点と終点は離れていること
		/// </summary>
		public void SetVertices(List<Vector2> vertices) {
			SetVertices(vertices, polygonColor);
		}

		/// <summary>
		/// 頂点の取得
		/// </summary>
		public List<Vector2> GetVertices() {
			List<Vector2> vertices = new List<Vector2>();
			//オフセットを加える
			Vector2 offset = transform.localPosition;
			for (int i = 0; i < this.vertices.Count; ++i) {
				vertices.Add(this.vertices[i] + offset);
			}
			return vertices;
		}

		/// <summary>
		/// 色の変更
		/// </summary>
		public void SetPolygonColor(Color color) {
			polygonColor = color;
			if (!disabled) {
				lerpPolygonColor.SetTarget(color);
			} else {
				lerpPolygonColor.SetTarget(color * disableColorScale);
			}
			//サブ簡易メッシュの色変更
			subEMesh.SetColor(color);
		}

		/// <summary>
		/// 有効化
		/// </summary>
		public void Enable() {
			if (!disabled) return;
			disabled = !disabled;
			SetPolygonColor(polygonColor);
		}

		/// <summary>
		/// 無効化
		/// </summary>
		public void Disable() {
			if (disabled) return;
			disabled = !disabled;
			SetPolygonColor(polygonColor);
		}

		#endregion

		#region ICollisionEventHadler

		public void OnPointerEnter(RaycastHit hit) {
			if (disabled) return;
			lerpOutlineColor.SetTarget(overOutlineColor);
			overed = true;
		}

		public void OnPointerExit(RaycastHit hit) {
			if (disabled) return;
			lerpOutlineColor.SetTarget(normalOutlineColor);
			overed = false;
		}

		public void OnPointerDown(RaycastHit hit) {
			if (disabled) return;
			lerpOutlineColor.SetTarget(clickOutlineColor);
			//コールバック
			onDown.Invoke(this);
		}

		public void OnPointerUp(RaycastHit hit) {
			if (disabled) return;
			lerpOutlineColor.SetTarget(overOutlineColor);
			//コールバック
			onUp.Invoke(this);
		}

		public void OnPointerClick(RaycastHit hit) {
			if (disabled) return;
			//コールバック
			onClick.Invoke(this);
		}

		#endregion
	}
}