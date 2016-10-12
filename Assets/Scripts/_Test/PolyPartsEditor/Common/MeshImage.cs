using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Seiro.Scripts.Graphics;

namespace Scripts._Test.PolyPartsEditor.Common {
	
	/// <summary>
	/// メッシュ形状のイメージ
	/// </summary>
	class MeshImage : Graphic {

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
		public void SetEasyMesh(EasyMesh eMesh) {
			this.eMesh = eMesh;
			SetVerticesDirty();
		}

		#endregion
	}
}