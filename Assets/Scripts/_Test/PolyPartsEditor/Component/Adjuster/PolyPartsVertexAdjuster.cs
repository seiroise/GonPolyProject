using UnityEngine;
using System.Collections.Generic;
using Seiro.Scripts.Graphics.PolyLine2D;
using Scripts._Test.PolyPartsEditor.Common;

namespace Scripts._Test.PolyPartsEditor.Component.Adjuster {

	/// <summary>
	/// ポリゴンパーツエディタの頂点調整
	/// </summary>
	public class PolyPartsVertexAdjuster : PolyPartsAdjusterComponent {

		[Header("PolyLineEditor")]
		public PolyLine2DEditor polyLineEditor;

		private PolyPartsObject selected;
		private List<Vector2> endVertices;

		#region VirtualFunction

		/// <summary>
		/// 初期化
		/// </summary>
		public override void Initialize(PolyPartsEditor editor, PolyPartsAdjuster adjuster) {
			base.Initialize(editor, adjuster);

			//コールバックの設定
			adjustMenu.vertexAdjustBtn.onClick.RemoveListener(OnVertexAdjustButtonClicked);
			adjustMenu.vertexAdjustBtn.onClick.AddListener(OnVertexAdjustButtonClicked);
		} 

		/// <summary>
		/// 開始
		/// </summary>
		public override void Enter() {
			base.Enter();
			//コンポーネントを活性化
			adjuster.ActivateComponent(this);
			//選択ポリゴンの取得
			selected = adjuster.GetSelected();
			//選択ポリゴンの無効化
			selected.Disable();
			//ポリラインエディタの頂点調整モードを有効化
			List<Vector2> vertices = selected.GetVertices();
			polyLineEditor.EnableAdjuster(vertices, true);
			//コールバックの設定
			polyLineEditor.onAdjusterExit.RemoveListener(OnAdjustEnd);
			polyLineEditor.onAdjusterExit.AddListener(OnAdjustEnd);
		}

		/// <summary>
		/// 終了
		/// </summary>
		public override void Exit() {
			base.Exit();
			//選択ポリゴンの有効化
			selected.Enable();
			//ポリラインエディタの無効化
			polyLineEditor.DisableState();
			if (endVertices != null) {
				adjuster.GetSelected().SetVertices(endVertices);
			}
		}

		#endregion

		#region Callback

		/// <summary>
		/// 頂点の調整終了
		/// </summary>
		private void OnAdjustEnd(List<Vector2> vertices) {
			endVertices = vertices;
			//ディスる(Exit呼ばれる)
			adjuster.DisactivateComponent();
		}
		
		#endregion

		#region UICallback

		/// <summary>
		/// 頂点調整ボタンのクリック
		/// </summary>
		private void OnVertexAdjustButtonClicked() {
			Enter();	//開始
		}

		#endregion
	}
}