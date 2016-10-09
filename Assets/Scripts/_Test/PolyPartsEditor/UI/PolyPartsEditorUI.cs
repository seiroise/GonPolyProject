using UnityEngine;
using System.Collections.Generic;

namespace Scripts._Test.PolyPartsEditor.UI {

	/// <summary>
	/// ポリゴンパーツ作成エディタのUI
	/// </summary>
	public class PolyPartsEditorUI : MonoBehaviour {

		[Header("SideMenu")]
		public SideMenuUI defaultSideMenu;	//デフォルト表示
		public SideMenuUI[] sideMenus;		//登録サイドメニュー
		private SideMenuUI nowSideMenu;		//表示サイドメニュー
		private Dictionary<string, SideMenuUI> sideMenuTable;

		#region UnityEvent

		private void Start() {

			//サイドメニュー関連
			InitializeSideMenu();
			IndicateDefaultSideMenu();
		}

		#endregion

		#region SideMenuFunction

		/// <summary>
		/// サイドメニュー関連の初期化
		/// </summary>
		private void InitializeSideMenu() {
			//テーブルの作成
			sideMenuTable = new Dictionary<string, SideMenuUI>();
			foreach(var e in sideMenus) {
				sideMenuTable.Add(e.name, e);
				e.Hide();
			}
		}

		/// <summary>
		/// デフォルトサイドメニューの表示
		/// </summary>
		public void IndicateDefaultSideMenu() {
			IndicateSideMenu(defaultSideMenu);
		}

		/// <summary>
		/// サイドメニューの表示
		/// </summary>
		public void IndicateSideMenu(SideMenuUI sideMenu) {
			if(nowSideMenu) {
				nowSideMenu.Hide();
			}
			sideMenu.Indicate();
			nowSideMenu = sideMenu;
		}

		/// <summary>
		/// サイドメニューの表示
		/// </summary>
		public void IndicateSideMenu(string name) {
			if(!sideMenuTable.ContainsKey(name)) return;
			IndicateSideMenu(sideMenuTable[name]);
		}

		/// <summary>
		/// サイドメニューの取得
		/// </summary>
		public SideMenuUI GetSideMenu(string name) {
			if(!sideMenuTable.ContainsKey(name)) return null;
			return sideMenuTable[name];
		}
		#endregion
	}
}