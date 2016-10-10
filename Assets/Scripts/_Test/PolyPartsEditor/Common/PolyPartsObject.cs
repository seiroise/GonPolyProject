﻿using UnityEngine;
using System.Collections.Generic;
using Seiro.Scripts.Geometric.Polygon.Concave;
using Seiro.Scripts.Graphics;
using Seiro.Scripts.EventSystems;
using Seiro.Scripts.Utility;

namespace Scripts._Test.PolyPartsEditor.Common {

	/// <summary>
	/// ポリゴンパーツオブジェクト
	/// </summary>
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
	public class PolyPartsObject : MonoBehaviour, ICollisionEventHandler {

		[Header("Polygon")]
		public Color polygonColor = Color.white;
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

		//内部パラメータ
		private MeshFilter mf;
		private MeshCollider mc;

		private ConcavePolygon polygon; //元データ
		private EasyMesh eMesh;         //描画簡易メッシュ
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
			this.vertices = vertices;
			polygon = new ConcavePolygon(vertices);
			draw = true;
		}

		/// <summary>
		/// 頂点の取得
		/// </summary>
		public List<Vector2> GetVertices() {
			List<Vector2> vertices = new List<Vector2>();
			for(int i = 0; i < this.vertices.Count; ++i) {
				vertices.Add(this.vertices[i]);
			}
			return vertices;
		}

		/// <summary>
		/// 有効化
		/// </summary>
		public void Enable() {
			if(!disabled) return;
			lerpPolygonColor.SetTarget(polygonColor);
			disabled = !disabled;
		}

		/// <summary>
		/// 無効化
		/// </summary>
		public void Disable() {
			if(disabled) return;
			lerpPolygonColor.SetTarget(polygonColor * disableColorScale);
			disabled = !disabled;
		}

		/// <summary>
		/// 描画
		/// </summary>
		private void Draw() {
			if(!draw) return;
			//描画用簡易ポリゴンの作成
			EasyMesh[] eMeshes = new EasyMesh[2];
			eMeshes[0] = polygon.ToEasyMesh(lerpPolygonColor.Value);
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