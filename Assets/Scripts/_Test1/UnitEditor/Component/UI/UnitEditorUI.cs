using UnityEngine;
using Scripts._Test1.UnitEditor.Component.UI.ShowHide;
using Scripts._Test.PolyPartsEditor.UI.ColorEditor;

namespace Scripts._Test1.UnitEditor.Component.UI {

	/// <summary>
	/// ユニットエディターのUI管理
	/// </summary>
	public class UnitEditorUI : UnitEditorComponent {

		[Header("Sidemenu")]
		public UnitEditorUIShowHide sidemenu;

		[Header("ColorEditField")]
		public ColorEditField colorEditField;	//仮

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