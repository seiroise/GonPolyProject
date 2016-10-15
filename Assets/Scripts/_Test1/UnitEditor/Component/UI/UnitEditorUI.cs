using UnityEngine;
using Scripts._Test1.UnitEditor.Component.UI.ShowHide;

namespace Scripts._Test1.UnitEditor.Component.UI {

	/// <summary>
	/// ユニットエディターのUI管理
	/// </summary>
	public class UnitEditorUI : UnitEditorComponent {

		[Header("Sidemenu")]
		public UnitEditorUIShowHide sidemenu;

		#region VirtualFunction

		public override void Initialize(UnitEditor unitEditor) {
			base.Initialize(unitEditor);

			//コンポーネントの初期化
			if (sidemenu) {
				sidemenu.Initialize(unitEditor, this);
			}
		}

		public override void LateInitialize() {
			base.LateInitialize();

			//コンポーネントの後初期化
			if (sidemenu) {
				sidemenu.LateInitialize();
			}
		}

		#endregion
	}
}