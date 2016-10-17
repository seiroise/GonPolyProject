using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Seiro.Scripts.Graphics;
using Seiro.Scripts.Geometric;

namespace Scripts._Test.PolyPartsEditor.Common {
	
	/// <summary>
	/// メッシュ形状のイメージ
	/// </summary>
	class MeshImage : MaskableGraphic {

		private EasyMesh eMesh;
		private List<UIVertex> uiVertices;

		#region UnityEvent

		protected override void OnPopulateMesh(VertexHelper vh) {
			if (eMesh == null) {
				vh.Clear();
				return;
			}
			if (uiVertices == null) {
				uiVertices = new List<UIVertex>();
			} else {
				uiVertices.Clear();
			}

			Vector3[] vertices = eMesh.verts;
			Color[] colors = eMesh.colors;
			int[] indices = eMesh.indices;

			for (int i = 0; i < indices.Length; ++i) {
				UIVertex vert = new UIVertex();
				vert.position = vertices[indices[i]];
				vert.color = colors[indices[i]];
				uiVertices.Add(vert);
			}

			vh.Clear();
			vh.AddUIVertexTriangleStream(uiVertices);
		}

		#endregion

		#region Function

		/// <summary>
		/// 簡易メッシュの設定
		/// </summary>
		public void SetEasyMesh(EasyMesh src) {
			//サイズの調整
			List<Vector2> vertices = new List<Vector2>();
			foreach (var e in src.verts) vertices.Add(e);
			Rect meshRect = GeomUtil.CalculateRect(vertices);
			//拡大率の計算
			Rect rect = rectTransform.rect;
			float xScale = rect.width / meshRect.width;
			float yScale = rect.height / meshRect.height;
			eMesh = new EasyMesh(src);
			eMesh.Scaling(Mathf.Abs(xScale < yScale ? xScale : yScale));

			SetVerticesDirty();
		}

		/// <summary>
		/// 色の設定
		/// </summary>
		public void SetColor(Color color) {
			if (eMesh == null) return;
			eMesh.SetColor(color);
			SetVerticesDirty();
		}

		#endregion
	}
}