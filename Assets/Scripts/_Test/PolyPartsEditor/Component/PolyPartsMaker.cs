using UnityEngine;
using System.Collections.Generic;
using Seiro.Scripts.Graphics.PolyLine2D;

namespace Scripts._Test.PolyPartsEditor.Component {
	
	/// <summary>
	/// ポリゴンパーツの作成コンポーネント
	/// </summary>
	public class PolyPartsMaker : PolyPartsEditorComponent {

		public PolyLine2DEditor polyLine;

		#region VirtualFunction

		public override void Initialoze(PolyPartsEditor editor) {
			base.Initialoze(editor);

			editor.mainMenuUI.makeBtn.onClick.AddListener(OnMakeButtonCLicked);
		}

		#endregion

		#region UICallback

		/// <summary>
		/// Makeボタンのクリック
		/// </summary>
		private void OnMakeButtonCLicked() {
			Debug.Log("OnMakeButtonClicked");
			//ポリゴンを無効化
			editor.database.DisablePolygons();
			//作成モードを有効化
			polyLine.EnableMaker();
			polyLine.onMakerExit.RemoveAllListeners();
			polyLine.onMakerExit.AddListener(OnMakeEnd);
		}

		#endregion

		#region Callback

		/// <summary>
		/// 作成終了時
		/// </summary>
		private void OnMakeEnd(List<Vector2> vertices) {
			//ポリゴンを有効化
			editor.database.EnablePolygons();
			//ポリゴンの生成
			if(vertices != null) {
				editor.database.InstantiatePolygon(vertices);
			}
		}

		#endregion
	}
}