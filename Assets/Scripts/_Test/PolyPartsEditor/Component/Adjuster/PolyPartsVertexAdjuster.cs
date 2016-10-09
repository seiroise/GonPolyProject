using UnityEngine;
using System.Collections.Generic;
using Seiro.Scripts.Graphics.PolyLine2D;

namespace Scripts._Test.PolyPartsEditor.Component.Adjuster {

	/// <summary>
	/// ポリゴンパーツエディタの頂点調整
	/// </summary>
	public class PolyPartsVertexAdjuster : PolyPartsAdjusterComponent {

		[Header("PolyLineEditor")]
		public PolyLine2DEditor polyLineEditor;

		#region VirtualFunction

		public override void Initialize(PolyPartsEditor editor, PolyPartsAdjuster adjuster) {
			base.Initialize(editor, adjuster);

			//コールバックの設定
			adjustMenu.vertexAdjustBtn.onClick.AddListener(OnVertexAdjustButtonClicked);
		}

		#endregion

		#region UICallback

		/// <summary>
		/// 頂点調整ボタンのクリック
		/// </summary>
		private void OnVertexAdjustButtonClicked() {
			Debug.Log("OnVertexAdjustButtonClicked");
			//ポリラインエディタの頂点調整モードを有効化
			List<Vector2> vertices = adjuster.GetSelected().GetVertices();
			vertices.Add(vertices[vertices.Count - 1]);	//末尾を追加
			polyLineEditor.EnableAdjuster(vertices, true);
		}

		#endregion
	}
}