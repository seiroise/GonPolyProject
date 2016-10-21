using UnityEngine;
using Scripts._Test1.UnitEditor.Common.ShowHide;
using System.Collections.Generic;
using System;

namespace Scripts._Test1.UnitEditor.Component.UI.ShowHide {

	/// <summary>
	/// ユニットエディタの表示非表示可能なUIの管理
	/// </summary>
	public class UnitEditorUIShowHide : UnitEditorUIComponent {

		[Header("UIComponents")]
		public UnitEditorUIShowHideComponent defaultUIComponent;	//標準コンポーネント
		public List<UnitEditorUIShowHideComponent> uiComponents;	//登録コンポーネント
		private Dictionary<string, UnitEditorUIShowHideComponent> uiComponentTable;	//コンポーネントテーブル
		private UnitEditorUIShowHideComponent activeUIComponent;	//有効コンポーネント
		private UnitEditorUIShowHideComponent prevActiveUIComponent;	//一つ前に有効だったコンポーネント

		#region VirtualFunction

		/// <summary>
		/// 初期化。こっちを使う
		/// </summary>
		public override void Initialize(UnitEditor unitEditor, UnitEditorUI editorUI) {
			base.Initialize(unitEditor, editorUI);

			//コンポーネントの初期化
			InitializeUIComponents(uiComponents);
		}

		/// <summary>
		/// 遅延初期化
		/// </summary>
		public override void LateInitialize() {
			base.LateInitialize();

			//遅延初期化
			LateInitializeUIComponents(uiComponents);

			//標準コンポーネントの表示
			ShowDefault();
		}

		#endregion

		#region UIComponentFunction

		/// <summary>
		/// UIコンポーネントの初期化
		/// </summary>
		private void InitializeUIComponents(List<UnitEditorUIShowHideComponent> uiComs) {
			if (uiComs == null) return;

			//テーブルに追加
			uiComponentTable = new Dictionary<string, UnitEditorUIShowHideComponent>();
			for (int i = 0; i < uiComs.Count; ++i) {
				uiComponentTable.Add(uiComs[i].name, uiComs[i]);
			}

			//初期化
			for (int i = 0; i < uiComs.Count; ++i) {
				uiComs[i].Initialize(unitEditor, editorUI, this);
			}
		}

		/// <summary>
		/// UIコンポーネントの後初期化
		/// </summary>
		private void LateInitializeUIComponents(List<UnitEditorUIShowHideComponent> uiComs) {
			if (uiComs == null) return;

			//後初期化
			for (int i = 0; i < uiComs.Count; ++i) {
				uiComs[i].LateInitialize();
			}
		}

		/// <summary>
		/// UIの表示
		/// </summary>
		public void ShowUI(string name) {

			//表示されているものを非表示
			HideUI();

			//表示
			activeUIComponent = GetUIComponent(name);
			if (activeUIComponent) {
				activeUIComponent.Show();
			}
		}

		/// <summary>
		/// 標準UIの表示
		/// </summary>
		public void ShowDefault() {
			if (!defaultUIComponent) return;
			ShowUI(defaultUIComponent.name);
		}

		/// <summary>
		/// 前回UIの表示
		/// </summary>
		public void ShowPrevUI() {
			if (prevActiveUIComponent == null) return;

			//表示
			ShowUI(prevActiveUIComponent.name);
		}

		/// <summary>
		/// UIの非表示
		/// </summary>
		public void HideUI() {
			if (activeUIComponent == null) return;
			activeUIComponent.Hide();
			prevActiveUIComponent = activeUIComponent;
			activeUIComponent = null;
		}

		/// <summary>
		/// 指定したUIコンポーネントの取得
		/// </summary>
		public UnitEditorUIShowHideComponent GetUIComponent(string name) {
			if (!uiComponentTable.ContainsKey(name)) return null;
			return uiComponentTable[name];
		}

		#endregion
	}
}