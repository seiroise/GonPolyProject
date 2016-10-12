using UnityEngine;
using System.Collections.Generic;
using Seiro.Scripts.Geometric.Polygon.Concave;
using Seiro.Scripts.Graphics;
using Seiro.Scripts.EventSystems;
using Seiro.Scripts.Utility;
using Seiro.Scripts.Geometric;

namespace Scripts._Test.PolyPartsEditor.Common {

	/// <summary>
	/// ポリゴンパーツオブジェクト
	/// </summary>
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
	public class PolyPartsObject : MonoBehaviour, ICollisionEventHandler {

		[Header("Polygon")]
		[SerializeField]
		private Color polygonColor = Color.white;
		[Range(0f, 1f)]
		public float disableColorScale = 0.75f;     //無効化時の色倍率
		private bool disabled = false;
		private LerpColor lerpPolygonColor;

		[Header("Outline")]
		public float width = 1f;
		public float lerpT = 10f;
		public Color overOutlineColor = new Color(0.75f, 0.5f, 0f);
		public Color clickOutlineColor = new Color(0.1f, 0.25f, 0f);
		private Color normalOutlineColor = new Color(0f, 0f, 0f, 0f);
		private LerpColor lerpOutlineColor;
		private List<Vector2> vertices;
		public List<Vector2> Vertices { get { return vertices; } }

		[Header("Callback")]
		public PolyPartsObjectEvent onClick;
		public PolyPartsObjectEvent onDown;
		public PolyPartsObjectEvent onUp;
		public PolyPartsObjectEvent onDraw;

		//内部パラメータ
		private MeshFilter mf;
		private MeshCollider mc;

		private ConcavePolygon polygon; //元データ
		private EasyMesh drawEMesh;         //描画簡易メッシュ
		private EasyMesh targetColorEMesh;	//目標色簡易メッシュ
		private EasyMesh[] eMeshes;		//描画簡易メッシュ領域
		private Rect inclusionRect;		//包括矩形
		private bool overed = false;    //被っているか
		private bool draw = false;      //描画フラグ

		#region UnityEvent

		private void Awake() {
			mf = GetComponent<MeshFilter>();
			mc = GetComponent<MeshCollider>();
			lerpPolygonColor = new LerpColor(polygonColor, polygonColor);
			lerpOutlineColor = new LerpColor(normalOutlineColor, normalOutlineColor);
		}

		private void Update() {

			UpdatePolygon();
			UpdateOutline();

			Draw();
		}

		#endregion

		#region Function

		/// <summary>
		/// 頂点の設定
		/// </summary>
		public void SetVertices(List<Vector2> vertices) {
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
			vertices.RemoveAt(vertices.Count -1);		//末尾を一時的に削除
			polygon = new ConcavePolygon(vertices);
			vertices.Add(vertices[0]);					//末尾に始点を追加

			//簡易メッシュの確保
			drawEMesh = polygon.ToEasyMesh(polygonColor);
			targetColorEMesh = polygon.ToEasyMesh(polygonColor);
			//コールバック
			onDraw.Invoke(this);

			//描画用簡易メッシュ領域の確保
			eMeshes = new EasyMesh[2];

			draw = true;
		}

		/// <summary>
		/// 頂点の取得
		/// </summary>
		public List<Vector2> GetVertices() {
			List<Vector2> vertices = new List<Vector2>();
			Vector2 offset = transform.localPosition;
			for(int i = 0; i < this.vertices.Count; ++i) {
				vertices.Add(this.vertices[i] + offset);
			}
			return vertices;
		}

		/// <summary>
		/// 有効化
		/// </summary>
		public void Enable() {
			if(!disabled) return;
			disabled = !disabled;
			SetPolygonColor(polygonColor);
		}

		/// <summary>
		/// 無効化
		/// </summary>
		public void Disable() {
			if(disabled) return;
			disabled = !disabled;
			SetPolygonColor(polygonColor);
		}

		/// <summary>
		/// 描画
		/// </summary>
		private void Draw() {
			if(!draw) return;
			//描画用簡易ポリゴンの作成
			eMeshes[0] = drawEMesh;
			eMeshes[1] = EasyMesh.MakePolyLine2D(vertices, width, lerpOutlineColor.Value);
			Mesh mesh = EasyMesh.ToMesh(eMeshes);
			//描画
			mf.mesh = mesh;
			if(mc) {
				if(!disabled) {
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
			if(!lerpPolygonColor.Processing) return;
			lerpPolygonColor.Update(lerpT * Time.deltaTime);
			//簡易メッシュの色変更
			drawEMesh.SetColor(lerpPolygonColor.Value);
			draw = true;
		}

		/// <summary>
		/// アウトラインの更新
		/// </summary>
		private void UpdateOutline() {
			if(!lerpOutlineColor.Processing) return;
			lerpOutlineColor.Update(lerpT * Time.deltaTime);
			draw = true;
		}

		/// <summary>
		/// 色の変更
		/// </summary>
		public void SetPolygonColor(Color color) {
			polygonColor = color;
			if(!disabled) {
				lerpPolygonColor.SetTarget(color);
			} else {
				lerpPolygonColor.SetTarget(color * disableColorScale);
			}
			//簡易ポリゴンの更新
			targetColorEMesh.SetColor(color);
			//コールバック
			onDraw.Invoke(this);
		}

		/// <summary>
		/// 色の取得
		/// </summary>
		public Color GetPolygonColor() {
			return polygonColor;
		}

		/// <summary>
		/// 簡易メッシュを取得
		/// </summary>
		public EasyMesh GetPolygonEasyMesh() {
			return targetColorEMesh;
		}

		/// <summary>
		/// 包括矩形の取得
		/// </summary>
		public Rect GetInclusionRect() {
			return inclusionRect;
		}

		#endregion

		#region CloneFunction

		/// <summary>
		/// 複製を生成する
		/// </summary>
		public PolyPartsObject InstantiateClone(Vector2 point) {
			GameObject gObj = (GameObject)Instantiate(gameObject, Vector3.zero, transform.rotation);
			PolyPartsObject polyObj = gObj.GetComponent<PolyPartsObject>();
			polyObj.SetVertices(vertices);
			gObj.transform.localPosition = point;
			return polyObj;
		}

		#endregion

		#region ICollisionEventHadler

		public void OnPointerEnter(RaycastHit hit) {
			lerpOutlineColor.SetTarget(overOutlineColor);
			overed = true;
		}

		public void OnPointerExit(RaycastHit hit) {
			lerpOutlineColor.SetTarget(normalOutlineColor);
			overed = false;
		}

		public void OnPointerDown(RaycastHit hit) {
			lerpOutlineColor.SetTarget(clickOutlineColor);
			onDown.Invoke(this);
		}

		public void OnPointerUp(RaycastHit hit) {
			if(overed) {
				lerpOutlineColor.SetTarget(overOutlineColor);
			} else {
				lerpOutlineColor.SetTarget(normalOutlineColor);
			}
			onUp.Invoke(this);
		}

		public void OnPointerClick(RaycastHit hit) {
			onClick.Invoke(this);
		}

		#endregion
	}
}