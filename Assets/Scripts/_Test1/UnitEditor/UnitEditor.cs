using Scripts._Test1.UnitEditor.Component;
using Scripts._Test1.UnitEditor.Component.Control;
using Scripts._Test1.UnitEditor.Component.StateMachine;
using Scripts._Test1.UnitEditor.Component.UI;
using Scripts._Test1.UnitEditor.Component.Utility.Database;
using Scripts._Test1.UnitEditor.Component.Utility.Marker;
using Scripts._Test1.UnitEditor.Component.Utility.PolyLine;
using UnityEngine;

namespace Scripts._Test1.UnitEditor {

	/// <summary>
	/// ユニットエディタ本体
	/// </summary>
	public class UnitEditor : UnitEditorComponent {

		[Header("Control")]
		public UnitEditorStateMachine control;

		[Header("UI")]
		public UnitEditorUI ui;

		[Header("Utilitys")]
		public UnitEditorPolyLine polyLine;
		public UnitEditorDatabase database;
		public UnitEditorMarker marker;

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
			if (control) {
				control.Initialize(this, null);
			}
			if (ui) {
				ui.Initialize(this);
			}
			if (polyLine) {
				polyLine.Initialize(this, null);
			}
			if (database) {
				database.Initialize(this);
			}
			if (marker) {
				marker.Initialize(this);
			}
		}

		public override void LateInitialize() {
			base.LateInitialize();

			//componentの遅延初期化
			if (control) {
				control.LateInitialize();
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
			if (marker) {
				marker.LateInitialize();
			}
		}

		#endregion

	}
}