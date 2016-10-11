using UnityEngine;
using System;
using System.Collections.Generic;
using Scripts._Test.PolyPartsEditor.UI;
using Scripts._Test.PolyPartsEditor.Common;
using Scripts._Test.PolyPartsEditor.Component.Adjuster;

namespace Scripts._Test.PolyPartsEditor.Component {

	/// <summary>
	/// ポリゴンパーツの調整コンポーネント
	/// </summary>
	public class PolyPartsAdjuster : PolyPartsEditorComponent {

		[Header("Components")]
		public List<PolyPartsAdjusterComponent> components;						//取り扱うコンポーネント一覧
		private Dictionary<Type, PolyPartsAdjusterComponent> componentTable;	//コンポーネントテーブル
		private PolyPartsAdjusterComponent activeComponent;						//現在のアクティブコンポーネント

		//内部パラメータ
		private AdjustMenuUI adjustMenu;
		public AdjustMenuUI AdjustMenu { get { return adjustMenu; } }
		private const string MENU_NAME = "AdjustMenu";

		private PolyPartsObject selected;

		#region VirtualFunction

		public override void Initialize(PolyPartsEditor editor) {
			base.Initialize(editor);

			//メニュー設定
			adjustMenu = (AdjustMenuUI)editor.editorUI.GetSideMenu(MENU_NAME);
			if(adjustMenu) {
				adjustMenu.exitBtn.onClick.AddListener(OnExitbuttonClicked);
			}

			//コンポーネント初期化
			InitializeComponents();

			//コールバックの設定
			editor.database.onPolyObjClicked.AddListener(OnPolyObjClicked);
		}

		#endregion

		#region Function

		/// <summary>
		/// Adjusterの開始
		/// </summary>
		public void Enter(PolyPartsObject polyObj) {
			//選択ポリゴンオブジェクトの設定
			SetSelected(polyObj);
			//調整メニューの表示
			editor.editorUI.IndicateSideMenu(MENU_NAME);
			//選択以外を無効化
			editor.database.DisablePolygons(polyObj);
			//登録してあるコールバックの設定変更
			editor.database.onPolyObjClicked.RemoveListener(OnPolyObjClicked);
		}

		/// <summary>
		/// Adjusterの終了
		/// </summary>
		public void Exit() {
			//ディスる
			DisactivateComponent();
			//デフォルトサイドメニューの表示
			editor.editorUI.IndicateDefaultSideMenu();
			//選択以外を無効化
			editor.database.EnablePolygons();
			//登録してあるコールバックの設定変更
			editor.database.onPolyObjClicked.AddListener(OnPolyObjClicked);
		}

		/// <summary>
		/// 選択ポリゴンオブジェクトの設定
		/// </summary>
		private void SetSelected(PolyPartsObject selected) {
			this.selected = selected;
		}

		/// <summary>
		/// 選択ポリゴンオブジェクトの取得
		/// </summary>
		public PolyPartsObject GetSelected() {
			return selected;
		}
		
		#endregion

		#region ComponentFunction

		/// <summary>
		/// コンポーネントの初期化
		/// </summary>
		private void InitializeComponents() {
			componentTable = new Dictionary<Type, PolyPartsAdjusterComponent>();
			for(int i = 0; i < components.Count; ++i) {
				componentTable.Add(components[i].GetType(), components[i]);
			}

			for(int i = 0; i < components.Count; ++i) {
				components[i].Initialize(editor, this);
			}
		}

		/// <summary>
		/// コンポーネントのアクティベート
		/// </summary>
		public void ActivateComponent(PolyPartsAdjusterComponent nextComponent) {
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
		/// 調整用コンポーネントの取得
		/// </summary>
		public T GetAdjusterComponent<T>() where T : PolyPartsAdjusterComponent {
			Type t = typeof(T);
			if (!componentTable.ContainsKey(t)) return null;
			return (T)componentTable[t];
		}

		#endregion

		#region Callback

		/// <summary>
		/// ポリゴンパーツのクリック(Adjuster起動時は呼ばれない)
		/// </summary>
		private void OnPolyObjClicked(PolyPartsObject polyObj) {
			Enter(polyObj);
		}

		#endregion

		#region UICallback

		/// <summary>
		/// 戻るボタンのクリック
		/// </summary>
		private void OnExitbuttonClicked() {
			Exit();
		}

		#endregion
	}
}