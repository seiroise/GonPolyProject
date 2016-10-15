using UnityEngine;
using System;
using System.Collections.Generic;

namespace Scripts._Test1.UnitEditor.Component.Control {

	/// <summary>
	/// ユニットエディタの操作系コンポーネントの管理
	/// </summary>
	public class UnitEditorControl : UnitEditorComponent {

		[Header("ControlComponents")]
		public List<UnitEditorControlComponent> components;	//登録コンポーネント
		public UnitEditorControlComponent defalutComponent;	//標準コンポーネント
		private Dictionary<Type, UnitEditorControlComponent> componentTable;	//コンポーネントテーブル
		private UnitEditorControlComponent activeComponent;	//有効コンポーネント

		#region VirtualFunction

		/// <summary>
		/// 初期化
		/// </summary>
		public override void Initialize(UnitEditor unitEditor) {
			base.Initialize(unitEditor);
			
			//コンポーネントの初期化
			InitializeControlComponents(components);
		}

		#endregion

		#region ComponentFunction

		/// <summary>
		/// コンポーネントの初期化
		/// </summary>
		private void InitializeControlComponents(List<UnitEditorControlComponent> coms) {
			if (coms == null) return;

			//テーブル追加
			componentTable = new Dictionary<Type,UnitEditorControlComponent>();
			for (int i = 0; i < coms.Count; ++i ) {
				Type t = coms[i].GetType();
				componentTable.Add(t, coms[i]);
			}

			//初期化
			for (int i = 0; i < coms.Count; ++i) {
				coms[i].Initialize(unitEditor, this);
			}
		}

		/// <summary>
		/// 指定したコンポーネントの有効化
		/// </summary>
		public T ActivateControlComponent<T>() where T : UnitEditorControlComponent {
			//現在有効化されているコンポーネントの無効化
			DisactivateControlComponent();
			
			//有効化するコンポーネントの取得
			activeComponent = GetComponent<T>();
			if (activeComponent) {
				activeComponent.Enter();
			}

			return (T)activeComponent;
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
		public T GetControlComponent<T>() where T : UnitEditorControlComponent {
			Type t = typeof(T);
			if (!componentTable.ContainsKey(t)) return null;
			return (T)componentTable[t];
		}

		#endregion

	}
}