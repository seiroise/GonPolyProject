using UnityEngine;
using System.Collections.Generic;
using Scripts._Test.PolyPartsEditor.UI;
using Scripts._Test.PolyPartsEditor.Common;
using Scripts._Test.PolyPartsEditor.Component.Adjuster;

namespace Scripts._Test.PolyPartsEditor.Component {

	/// <summary>
	/// ポリゴンパーツの調整コンポーネント
	/// </summary>
	public class PolyPartsAdjuster : PolyPartsEditorComponent {

		[Header("Components")]
		public PolyPartsVertexAdjuster vertexAdjuster;
		private List<PolyPartsAdjusterComponent> components;

		//内部パラメータ
		private AdjustMenuUI adjustMenu;
		public AdjustMenuUI AdjustMenu { get { return adjustMenu; } }
		private const string MENU_NAME = "AdjustMenu";

		private PolyPartsObject selected;

		#region VirtualFunction

		public override void Initialize(PolyPartsEditor editor) {
			base.Initialize(editor);

			//メニュー設定
			adjustMenu = (AdjustMenuUI)editor.editorUI.GetSideMenu(MENU_NAME);
			if(adjustMenu) {
				adjustMenu.exitBtn.onClick.AddListener(OnExitbuttonClicked);
			}

			//コンポーネント初期化
			InitializeComponents();

			//コールバックの設定
			editor.database.onPolyObjClicked.AddListener(OnPolyObjClicked);
		}

		#endregion

		#region Function

		/// <summary>
		/// コンポーネントの初期化
		/// </summary>
		private void InitializeComponents() {
			components = new List<PolyPartsAdjusterComponent>();
			components.Add(vertexAdjuster);

			for(int i = 0; i < components.Count; ++i) {
				components[i].Initialize(editor, this);
			}
		}

		/// <summary>
		/// 選択ポリゴンオブジェクトの設定
		/// </summary>
		private void SetSelected(PolyPartsObject selected) {
			this.selected = selected;
		}

		/// <summary>
		/// 選択ポリゴンオブジェクトの取得
		/// </summary>
		public PolyPartsObject GetSelected() {
			return selected;
		}

		#endregion

		#region Callback

		/// <summary>
		/// ポリゴンパーツのクリック
		/// </summary>
		private void OnPolyObjClicked(PolyPartsObject polyObj) {

			//選択ポリゴンオブジェクトの設定
			SetSelected(polyObj);
			//調整メニューの表示
			editor.editorUI.IndicateSideMenu(MENU_NAME);
		}

		#endregion

		#region UICallback

		/// <summary>
		/// 戻るボタンのクリック
		/// </summary>
		private void OnExitbuttonClicked() {
			//デフォルトサイドメニューの表示
			editor.editorUI.IndicateDefaultSideMenu();
		}

		#endregion
	}
}