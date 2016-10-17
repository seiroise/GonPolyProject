using Scripts._Test1.UnitEditor.Common.Lerp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts._Test1.UnitEditor.Component.UI.ShowHide.Sidemenu {

	/// <summary>
	/// パーツ選択時に表示するメニュー
	/// </summary>
	public class UnitEditorUIPartsMenu : UnitEditorUIShowHideComponent {

		[Header("LerpPosition")]
		public UILerpRectPosition lerpPosition;

		[Header("Buttons")]
		public Button adjustBtn;	//調整ボタン
		public Button moveBtn;		//移動ボタン
		public Button rotateBtn;	//回転ボタン
		public Button mirrorBtn;	//反転ボタン
		public Button copyBtn;		//コピーボタン
		public Button deleteBtn;	//削除ボタン

		public Button exitBtn;		//戻るボタン

		#region VirtualFunction

		/// <summary>
		/// 表示
		/// </summary>
		public override void Show() {
			base.Show();
			lerpPosition.ToAfter();
		}

		/// <summary>
		/// 非表示
		/// </summary>
		public override void Hide() {
			base.Show();
			lerpPosition.ToBefore();
		}

		#endregion
	}
}