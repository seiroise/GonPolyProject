using UnityEngine;
using Scripts._Test.PolyPartsEditor.Common;
using Scripts._Test.PolyPartsEditor.UI.ColorEditor;

namespace Scripts._Test.PolyPartsEditor.Component.Adjuster {

	/// <summary>
	/// ポリゴンパーツエディタの色調整
	/// </summary>
	class PolyPartsColorAdjuster : PolyPartsAdjusterComponent {

		private ColorEditPopup popup;
		private PolyPartsObject selected;
		private const string POPUP_NAME = "ColorEditPopup";

		#region VirtualFunction

		/// <summary>
		/// 開始
		/// </summary>
		public override void Initialize(PolyPartsEditor editor, PolyPartsAdjuster adjuster) {
			base.Initialize(editor, adjuster);

			//UIコールバックの設定
			adjustMenu.colorBtn.onClick.RemoveListener(OnColorButtonClicked);
			adjustMenu.colorBtn.onClick.AddListener(OnColorButtonClicked);
		}

		/// <summary>
		/// 開始
		/// </summary>
		public override void Enter() {
			base.Enter();
			//活性化
			adjuster.ActivateComponent(this);
			//選択ポリゴンの取得
			selected = adjuster.GetSelected();
			//表示ポップアップの取得
			if(popup == null) {
				popup = (ColorEditPopup)editor.ui.popup.GetUI(POPUP_NAME);
				if (popup == null) {
					//結果的にPopupが取得できないなら非活性化
					adjuster.DisactivateComponent();
					return;
				}
			}
			//コールバックの設定
			popup.colorEditor.onColorChanged.AddListener(OnColorChanged);
			//ポップアップの表示
			editor.ui.popup.IndicateUI(POPUP_NAME);
			//編集する色を設定
			popup.colorEditor.SetColor(selected.GetPolygonColor());
		}

		/// <summary>
		/// 終了
		/// </summary>
		public override void Exit() {
			base.Exit();
			//コールバックの設定
			popup.colorEditor.onColorChanged.RemoveListener(OnColorChanged);
			//ポップアップの非表示
			editor.ui.popup.HideUI();
		}

		#endregion

		#region UICallback

		/// <summary>
		/// 色調整ボタンのクリック
		/// </summary>
		private void OnColorButtonClicked() {
			Enter();
		}

		/// <summary>
		/// 色の変更
		/// </summary>
		private void OnColorChanged(Color color) {
			if (selected) {
				selected.SetPolygonColor(color);
			}
		}

		#endregion
	}
}