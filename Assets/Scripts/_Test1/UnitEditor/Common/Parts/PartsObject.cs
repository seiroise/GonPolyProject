using Seiro.Scripts.EventSystems;
using UnityEngine;
using System.Collections.Generic;
using Seiro.Scripts.Geometric;
using Seiro.Scripts.Geometric.Polygon.Concave;
using Seiro.Scripts.Graphics;
using Seiro.Scripts.Utility;
using Scripts._Test1.UnitEditor.Common.Parts.Equip;
using Scripts._Test1.UnitEditor.Component.Utility.Marker;
using System;
using UnityEngine.Events;

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

		[Header("Equipment")]
		public List<Equipment> launchers;		//砲台
		public List<Equipment> boosters;		//ブースタ

		[Header("Marker")]
		public string launcherMarker = "LauncherMarker";
		public string boosterMarker = "BoosterMarker";
		private UnitEditorMarker marker;					//マーカー管理クラス
		private List<SpriteMarker> showLauncherMarkers;		//表示している砲台マーカー
		private List<SpriteMarker> showBoosterMarkers;		//表示しているブースタマーカー

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

		/// <summary>
		/// 砲台の解析
		/// </summary>
		private List<Equipment> ParseLauncher(ConcavePolygon polygon, float tolerance = 1f) {
			List<PolygonVertex> vertices = polygon.GetPolygonVertices();
			int size = vertices.Count;
			List<Equipment> launchers = new List<Equipment>();

			float acutelity = 45f;	//鋭さ判定用

			//解析開始
			for (int i = 0; i < size; ++i) {
				PolygonVertex p1 = vertices[(i + 1) % size];
				PolygonVertex p2 = vertices[(i + 2) % size];

				//鋭角あるいは鈍角か判定
				if (p1.angle < (90f - acutelity) || (90f + acutelity) < p1.angle) continue;

				//角度の和を求める
				float sumAngle = p1.angle + p2.angle;

				//角度の和が180度近辺の場合
				if (180f - tolerance < sumAngle && sumAngle < 180f + tolerance) {
					PolygonVertex p0 = vertices[i];
					PolygonVertex p3 = vertices[(i + 3) % size];
					//座標
					Vector2 point = (p2.point - p1.point) * 0.5f + p1.point;
					//角度
					float angle = GeomUtil.TwoPointAngle(p0.point, p1.point);
					//砲身長
					float barrel0 = (p1.point - p0.point).magnitude;
					float barrel1 = (p3.point - p2.point).magnitude;
					float barrel = (barrel0 + barrel1) * 0.5f;
					//口径
					Line l1 = Line.FromPoints(p0.point, p1.point);
					Line l2 = Line.FromPoints(p2.point, GeomUtil.RotateVector2(p3.point - p2.point, 90f) + p2.point);	//l1の垂直線
					Vector2 intersection = Vector2.zero;
					l1.GetIntersectionPoint(l2, ref intersection);
					float caliber = (intersection - p2.point).magnitude;

					launchers.Add(new Equipment("Launcher", point, angle));
				}
			}
			return launchers;
		}

		#endregion

		#region PublicFunction

		/// <summary>
		/// 初期化
		/// </summary>
		public void Initialize(List<Vector2> vertices, Color color, UnitEditorMarker marker) {
			this.marker = marker;
			
			//頂点の設定
			SetVertices(vertices, color);
		}

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

			//Equipmentの解析
			launchers = ParseLauncher(polygon);

			//マーカーの非表示/表示
			HideMarkers(showLauncherMarkers);
			showLauncherMarkers = ShowEquipmentMarkers(launcherMarker, launchers, OnLauncherMarkerClicked);

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
			disabled = false;
			//ポリゴン本体の色を変更
			SetPolygonColor(polygonColor);
			//マーカーの有効化
			EnableMarkers(showLauncherMarkers, showBoosterMarkers);
		}

		/// <summary>
		/// 無効化
		/// </summary>
		public void Disable() {
			if (disabled) return;
			disabled = true;
			//ポリゴン本体の色を変更
			SetPolygonColor(polygonColor);
			//マーカーの無効化
			DisableMarkers(showLauncherMarkers, showBoosterMarkers);
		}

		#endregion

		#region MarkerFunction

		/// <summary>
		/// Equipmentマーカーの表示
		/// </summary>
		private List<SpriteMarker> ShowEquipmentMarkers(string markerName, List<Equipment> equipments, UnityAction<GameObject> callback) {
			if(equipments == null || equipments.Count <= 0) return null;
			List<SpriteMarker> markers = marker.PopMarkers(markerName, equipments.Count, transform.localPosition);
			for(int i = 0; i < markers.Count; ++i) {
				SpriteMarker m = markers[i];
				Debug.Log(m.GetInstanceID());
				m.name = i.ToString();
				m.transform.localPosition += (Vector3)equipments[i].point;
				m.transform.localEulerAngles = new Vector3(0f, 0f, equipments[i].angle);
				//コールバックの設定
				m.onClick.RemoveListener(callback);
				m.onClick.AddListener(callback);
			}
			//状態によって有効/無効化
			if(disabled) {
				DisableMarkers(markers);
			} else {
				EnableMarkers(markers);
			}

			return markers;
		}

		/// <summary>
		/// Equipmentマーカーの非表示
		/// </summary>
		private void HideMarkers(params List<SpriteMarker>[] markers) {
			for(int i = 0; i < markers.Length; ++i) {
				List<SpriteMarker> ms = markers[i];
				if (ms == null) continue;
				for (int j = 0; j < ms.Count; ++j) {
					ms[j].gameObject.SetActive(false);
				}
			}
		}

		/// <summary>
		/// Equipmentマーカーの有効化
		/// </summary>
		private void EnableMarkers(params List<SpriteMarker>[] markers) {
			for(int i = 0; i < markers.Length; ++i) {
				List<SpriteMarker> ms = markers[i];
				if (ms == null) continue;
				Debug.Log(ms);
				for (int j = 0; j < ms.Count; ++j) {
					ms[j].Enable();
				}
			}
		}

		/// <summary>
		/// Equipmentマーカーの無効化
		/// </summary>
		private void DisableMarkers(params List<SpriteMarker>[] markers) {
			for(int i = 0; i < markers.Length; ++i) {
				List<SpriteMarker> ms = markers[i];
				if (ms == null) continue;
				for (int j = 0; j < ms.Count; ++j) {
					ms[j].Disable();
				}
			}
		}

		#endregion

		#region Callback

		/// <summary>
		/// 砲台マーカーのクリック
		/// </summary>
		private void OnLauncherMarkerClicked(GameObject gObj) {
			int index = int.Parse(gObj.name);
			Debug.Log("Clicked Launcher [" + index + "]");
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