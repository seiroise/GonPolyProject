using Scripts._Test1.UnitEditor.Common.Lerp;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts._Test1.UnitEditor.Component.UI.ShowHide.Sidemenu {

	/// <summary>
	/// ユニットエディタのサイドメニュー用UI
	/// </summary>
	public class UnitEditorUISideMenu : UnitEditorUIShowHideComponent {

		[Header("LerpPosition")]
		public UILerpRectPosition lerpPosition;

		[Header("ExitButton")]
		public Button exitBtn;

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
			base.Hide();
			lerpPosition.ToBefore();
		}

		#endregion
	}
}