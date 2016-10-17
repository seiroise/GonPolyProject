using UnityEngine;
using System;
using System.Collections.Generic;
using Scripts._Test.PolyPartsEditor.UI;
using Scripts._Test.PolyPartsEditor.Component;
using Scripts._Test.PolyPartsEditor.Database;

namespace Scripts._Test.PolyPartsEditor {

	/// <summary>
	/// ポリゴンパーツエディター
	/// </summary>
	public class PolyPartsEditor : MonoBehaviour {

		[Header("Components")]
		public List<PolyPartsEditorComponent> components;
		private Dictionary<Type, PolyPartsEditorComponent> componentTable;
		private PolyPartsEditorComponent activeComponent;

		[Header("UI")]
		public PolyPartsEditorUI ui;

		[Header("DataBase")]
		public PolyPartsDatabase database;

		#region UnityEvent

		private void Start() {
			Initialize();
		}

		#endregion

		#region Function

		/// <summary>
		/// 初期化
		/// </summary>
		private void Initialize() {
			InitializeComponents();
		}

		#endregion

		#region ComponentFunction

		/// <summary>
		/// コンポーネントの初期化
		/// </summary>
		private void InitializeComponents() {
			componentTable = new Dictionary<Type, PolyPartsEditorComponent>();
			for (int i = 0; i < components.Count; ++i) {
				componentTable.Add(components[i].GetType(), components[i]);
			}

			for (int i = 0; i < components.Count; ++i) {
				components[i].Initialize(this);
			}
		}

		/// <summary>
		/// コンポーネントのアクティベート
		/// </summary>
		public void ActivateComponent(PolyPartsEditorComponent nextComponent) {
			if (activeComponent != null) {
				if (activeComponent != nextComponent) {
					DisactivateComponent();
				}
			}
			activeComponent = nextComponent;
		}

		/// <summary>
		/// アクティベートされたコンポーネントをディスる
		/// <para>コンポーネント側のExit()から呼ばないこと</para>>
		/// </summary>
		public void DisactivateComponent() {
			if (activeComponent == null) return;
			activeComponent.Exit();
			activeComponent = null;
		}

		/// <summary>
		/// コンポーネントの取得
		/// </summary>
		public T GetAdjusterComponent<T>() where T : PolyPartsEditorComponent {
			Type t = typeof(T);
			if (!componentTable.ContainsKey(t)) return null;
			return (T)componentTable[t];
		}

		#endregion
	}
}