using UnityEngine;
using UnityEngine.UI;
using Scripts._Test1.UnitEditor.Common.Lerp;

namespace Scripts._Test1.UnitEditor.Component.UI.ShowHide.Sidemenu {

	/// <summary>
	/// デフォルトで表示するメニュー
	/// </summary>
	class UnitEditorUIMainmenu : UnitEditorUIShowHideComponent {

		[Header("LerpPosition")]
		public UILerpRectPosition lerpPosition;

		[Header("Buttons")]
		public Button makeBtn;		//作成ボタン

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