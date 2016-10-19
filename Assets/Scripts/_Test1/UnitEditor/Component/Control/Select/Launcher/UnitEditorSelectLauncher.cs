using System;
using Scripts._Test1.UnitEditor.Common.Parts.Equip;
using Scripts._Test1.UnitEditor.Component.StateMachine;

namespace Scripts._Test1.UnitEditor.Component.Control.Select.Launcher {

	/// <summary>
	/// ユニットエディタのランチャー選択
	/// </summary>
	public class UnitEditorSelectLauncher : UnitEditorStateMachine {

		//選択ランチャー
		private Equipment select;
		public Equipment Select { get { return select; } }

		//定数
		private const string MENU = "LauncherMenu";
		private const string PREV_MENU = "MainMenu";

		#region VirtualFunction


		public override void LateInitialize() {
			base.LateInitialize();

			//UIコールバックの設定
			UnitEditorUI
		}

		#endregion
	}
}