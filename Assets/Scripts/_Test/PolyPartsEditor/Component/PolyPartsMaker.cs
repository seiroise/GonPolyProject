using UnityEngine;
using System.Collections.Generic;
using Seiro.Scripts.Graphics.PolyLine2D;
using Scripts._Test.PolyPartsEditor.UI;

namespace Scripts._Test.PolyPartsEditor.Component {
	
	/// <summary>
	/// ポリゴンパーツの作成コンポーネント
	/// </summary>
	public class PolyPartsMaker : PolyPartsEditorComponent {

		public PolyLine2DEditor polyLine;

		//内部パラメータ
		private MainMenuUI mainMenu;

		#region VirtualFunction

		public override void Initialize(PolyPartsEditor editor) {
			base.Initialize(editor);

			mainMenu = (MainMenuUI)editor.editorUI.GetSideMenu("MainMenu");
			if(mainMenu) {
				mainMenu.makeBtn.onClick.AddListener(OnMakeButtonCLicked);
			}
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

	}
}