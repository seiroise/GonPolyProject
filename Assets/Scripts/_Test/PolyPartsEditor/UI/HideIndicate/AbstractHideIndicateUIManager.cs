using UnityEngine;
using System.Collections.Generic;

namespace Scripts._Test.PolyPartsEditor.UI.HideIndicate {

	/// <summary>
	/// IHideIndicateUIの管理
	/// </summary>
	public abstract class AbstractHideIndicateUIManager<T> : MonoBehaviour where T : MonoBehaviour, IHideIndicateUI<T> {

		[Header("HideIndicateUI")]
		public T defaultUI;	//標準
		public T[] uis;		//登録UI
		private T nowUI;		//表示中
		private Dictionary<string, T> uiTable;

		#region UnityEvent
		
		private void Start() {
			Initialize();
			IndicateDefaultUI();
		}

		#endregion

		#region Function

		/// <summary>
		/// 初期化
		/// </summary>
		private void Initialize() {
			
			//テーブル初期化
			if (uiTable != null) {
				uiTable.Clear();
			} else {
				uiTable = new Dictionary<string, T>();
			}
			
			//テーブル登録
			foreach (var e in uis) {
				uiTable.Add(e.name, e);
				e.Hide();
			}
		}

		/// <summary>
		/// デフォルトUIの表示
		/// </summary>
		public void IndicateDefaultUI() {
			IndicateUI(defaultUI);
		}

		/// <summary>
		/// UIの表示
		/// </summary>
		public void IndicateUI(T ui) {
			//既存のモノを非表示
			HideUI();

			//表示
			if (ui != null) {
				ui.Indicate();
				nowUI = ui;
			}
		}

		/// <summary>
		/// UIの表示
		/// </summary>
		public void IndicateUI(string id) {
			if (!uiTable.ContainsKey(id)) return;
			IndicateUI(uiTable[id]);
		}

		/// <summary>
		/// UIの非表示
		/// </summary>
		public void HideUI() {
			if (nowUI != null) {
				nowUI.Hide();
				nowUI = null;
			}
		}

		/// <summary>
		/// UIの取得
		/// </summary>
		public T GetUI(string id) {
			if (!uiTable.ContainsKey(id)) return null;
			return uiTable[id];
		}

		#endregion
	}
}