using UnityEngine;
using System;
using System.Collections.Generic;

namespace Scripts._Test1.UnitEditor.Component.Control {

	/// <summary>
	/// ユニットエディタの操作系コンポーネントの管理
	/// </summary>
	public class UnitEditorControl : UnitEditorComponent {

		[Header("ControlComponents")]
		public UnitEditorControlComponent defaultComponent;	//標準コンポーネント
		public List<UnitEditorControlComponent> components;	//登録コンポーネント
		private Dictionary<string, UnitEditorControlComponent> componentTable;	//コンポーネントテーブル
		private UnitEditorControlComponent activeComponent;	//有効コンポーネント

		#region VirtualFunction

		/// <summary>
		/// 初期化
		/// </summary>
		public override void Initialize(UnitEditor unitEditor) {
			base.Initialize(unitEditor);
			
			//コンポーネントの初期化
			InitializeControlComponents(components);

			//標準コンポーネントの有効化
			ActivateDefault();
		}

		/// <summary>
		/// 後初期化
		/// </summary>
		public override void LateInitialize() {
			base.LateInitialize();

			//コンポーネントの後初期化
			LateInitializeControlComponents(components);
		}

		#endregion

		#region ComponentFunction

		/// <summary>
		/// コンポーネントの初期化
		/// </summary>
		private void InitializeControlComponents(List<UnitEditorControlComponent> coms) {
			if (coms == null) return;

			//テーブルに追加
			componentTable = new Dictionary<string,UnitEditorControlComponent>();
			for (int i = 0; i < coms.Count; ++i ) {
				componentTable.Add(coms[i].name, coms[i]);
			}

			//初期化
			for (int i = 0; i < coms.Count; ++i) {
				coms[i].Initialize(unitEditor, this);
			}
		}

		/// <summary>
		/// コンポーネントの後初期化
		/// </summary>
		private void LateInitializeControlComponents(List<UnitEditorControlComponent> coms) {
			if (coms == null) return;

			//後初期化
			for (int i = 0; i < coms.Count; ++i) {
				coms[i].LateInitialize();
			}
		}

		/// <summary>
		/// 指定したコンポーネントの有効化
		/// </summary>
		public void ActivateControlComponent(string name) {
			//現在有効化されているコンポーネントの無効化
			DisactivateControlComponent();
			
			//有効化するコンポーネントの取得
			activeComponent = GetControlComponent(name);
			if (activeComponent) {
				activeComponent.Enter();
			}
		}

		/// <summary>
		/// 標準コンポーネントの有効化
		/// </summary>
		public void ActivateDefault() {
			if (!defaultComponent) return;
			ActivateControlComponent(defaultComponent.name);
		}

		/// <summary>
		/// 有効化されているコンポーネントの無効化
		/// </summary>
		public void DisactivateControlComponent() {
			if (activeComponent == null) return;
			activeComponent.Exit();
			activeComponent = null;
		}

		/// <summary>
		/// 指定したコンポーネントの取得
		/// </summary>
		public UnitEditorControlComponent GetControlComponent(string name) {
			if (!componentTable.ContainsKey(name)) return null;
			return componentTable[name];
		}

		#endregion

	}
}