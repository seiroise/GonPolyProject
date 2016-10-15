using UnityEngine;
using Scripts._Test1.UnitEditor.Component;
using Scripts._Test1.UnitEditor.Component.Control;
using Scripts._Test1.UnitEditor.Component.UI;
using Scripts._Test1.UnitEditor.Component.Utility.PolyLine;
using Scripts._Test1.UnitEditor.Component.Utility.Database;

namespace Scripts._Test1.UnitEditor {

	/// <summary>
	/// ユニットエディタ本体
	/// </summary>
	public class UnitEditor : UnitEditorComponent {

		[Header("Control")]
		public UnitEditorControl controller;

		[Header("UI")]
		public UnitEditorUI ui;

		[Header("Utilitys")]
		public UnitEditorPolyLine polyLine;
		public UnitEditorDatabase database;

		#region UnityEvent

		private void Start() {
			Initialize(this);
			LateInitialize();
		}

		#endregion

		#region VirtualFunction

		public override void Initialize(UnitEditor unitEditor) {
			base.Initialize(unitEditor);

			//componentの初期化
			if (controller) {
				controller.Initialize(this);
			}
			if (ui) {
				ui.Initialize(this);
			}
			if (polyLine) {
				polyLine.Initialize(this);
			}
			if (database) {
				database.Initialize(this);
			}
		}

		public override void LateInitialize() {
			base.LateInitialize();

			//componentの遅延初期化
			if (controller) {
				controller.LateInitialize();
			}
			if (ui) {
				ui.LateInitialize();
			}
			if (polyLine) {
				polyLine.LateInitialize();
			}
			if (database) {
				database.LateInitialize();
			}
		}

		#endregion

	}
}