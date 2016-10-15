using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Seiro.Scripts.Utility;

namespace Scripts._Test1.UnitEditor.Component.Utility.PolyLine {

	/// <summary>
	/// ユニットエディタ用ポリライン描画
	/// </summary>
	public class UnitEditorPolyLine : UnitEditorComponent {

		[Header("Component")]
		public UnitEditorPolyLineStateComponent defaultComponent;	//標準コンポーネント
		public List<UnitEditorPolyLineStateComponent> components;	//状態コンポーネント
		private Dictionary<string, UnitEditorPolyLineStateComponent> componentTable;	//コンポーネントテーブル
		private UnitEditorPolyLineStateComponent activeComponent;	//有効コンポーネント

		[Header("Utility")]
		public UnitEditorPolyLineRenderer renderer;
		public UnitEditorPolyLineSnapper snapper;

		[Header("Input")]
		public EventSystem uiEventSystem;
		public Camera cam;

		#region VirtualFunction

		/// <summary>
		/// 初期化
		/// </summary>
		public override void Initialize(UnitEditor unitEditor) {
			base.Initialize(unitEditor);

			//コンポーネントの初期化
			InitializeStateComponents(components);

			//標準コンポーネントの有効化
			ActivateDefault();
		}

		/// <summary>
		/// 後初期化
		/// </summary>
		public override void LateInitialize() {
			base.LateInitialize();

			//コンポーネントの後初期化
			LateInitializeStateComponents(components);
		}

		#endregion

		#region ComponentFunction

		/// <summary>
		/// コンポーネントの初期化
		/// </summary>
		private void InitializeStateComponents(List<UnitEditorPolyLineStateComponent> coms) {
			if (coms == null) return;

			//テーブルに追加
			componentTable = new Dictionary<string, UnitEditorPolyLineStateComponent>();
			for (int i = 0; i < coms.Count; ++i) {
				componentTable.Add(coms[i].name, coms[i]);
			}

			//Componentの初期化
			for (int i = 0; i < coms.Count; ++i) {
				coms[i].Initialize(unitEditor, this);
			}
			renderer.Initialize(unitEditor, this);
			snapper.Initialize(unitEditor, this);
		}

		/// <summary>
		/// コンポーネントの後初期化
		/// </summary>
		private void LateInitializeStateComponents(List<UnitEditorPolyLineStateComponent> coms) {
			if (coms == null) return;

			//後初期化
			for (int i = 0; i < coms.Count; ++i) {
				coms[i].LateInitialize();
			}
			renderer.LateInitialize();
			snapper.LateInitialize();
		}

		/// <summary>
		/// 指定したコンポーネントの有効化
		/// </summary>
		public void ActivateStateComponent(string name) {
			//現在有効化されているコンポーネントの無効化
			DisactivateStateComponent();

			//有効化するコンポーネントの取得
			activeComponent = GetStateComponent(name);
			if (activeComponent) {
				activeComponent.Enter();
			}
		}

		/// <summary>
		/// 標準コンポーネントの有効化
		/// </summary>
		public void ActivateDefault() {
			if (!defaultComponent) return;
			ActivateStateComponent(defaultComponent.name);
		}

		/// <summary>
		/// 有効化されているコンポーネントの無効化
		/// </summary>
		public void DisactivateStateComponent() {
			if (activeComponent == null) return;
			activeComponent.Exit();
			activeComponent = null;
		}

		/// <summary>
		/// 指定したコンポーネントの取得
		/// </summary>
		public UnitEditorPolyLineStateComponent GetStateComponent(string name) {
			if (!componentTable.ContainsKey(name)) return null;
			return componentTable[name];
		}

		#endregion

		#region Function

		/// <summary>
		/// マウス座標の取得。指定したEventStstemに被る場合はfalseを返す
		/// </summary>
		public bool GetMousePoint(out Vector2 point) {
			point = FuncBox.GetMousePosition(cam);
			if(!uiEventSystem) {
				return true;
			} else if(uiEventSystem.IsPointerOverGameObject()) {
				return false;
			} else {
				return true;
			}
		}

		#endregion
	}
}