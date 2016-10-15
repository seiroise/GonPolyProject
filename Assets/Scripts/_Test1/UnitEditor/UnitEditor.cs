using UnityEngine;
using Scripts._Test1.UnitEditor.Component;
using Scripts._Test1.UnitEditor.Component.Control;
using Scripts._Test1.UnitEditor.Component.UI;

namespace Scripts._Test1.UnitEditor {

	/// <summary>
	/// ユニットエディタ本体
	/// </summary>
	public class UnitEditor : UnitEditorComponent {

		[Header("EditorComponents")]
		public UnitEditorControl controller;
		public UnitEditorUI ui;

		#region UnityEvent

		private void Start() {
			Initialize(this);
			LateInitialize();
		}

		#endregion

		#region VirtualFunction

		public override void Initialize(UnitEditor unitEditor) {
			base.Initialize(unitEditor);

			//コンポーネントの初期化
			if (controller) {
				controller.Initialize(this);
			}
			if (ui) {
				ui.Initialize(this);
			}
		}

		public override void LateInitialize() {
			base.LateInitialize();

			//コンポーネントの後初期化
			if (controller) {
				controller.LateInitialize();
			}
			if (ui) {
				ui.LateInitialize();
			}
		}

		#endregion

	}
}