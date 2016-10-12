using UnityEngine;
using System.Collections.Generic;
using Scripts._Test.PolyPartsEditor.Common;

namespace Scripts._Test.PolyPartsEditor.UI.Hierarchy {

	/// <summary>
	/// ポリゴンパーツ階層のUI表示
	/// </summary>
	class HierarchyUIManager : MonoBehaviour {

		[Header("Content")]
		public RectTransform content;

		[Header("Prefab")]
		public HierarchyUIContent prefab;

		[Header("Spacing")]
		public float ySpacing = 5f;

		private List<HierarchyUIContent> contents;

		#region UnityEvent

		private void Awake() {
			contents = new List<HierarchyUIContent>();
		}

		#endregion

		#region Function

		/// <summary>
		/// Contentの生成
		/// </summary>
		private HierarchyUIContent InstantiateContent(PolyPartsObject polyObj) {
			HierarchyUIContent con = Instantiate(prefab);
			con.SetPolyObject(polyObj);
			return con;
		}

		/// <summary>
		/// コンテンツの追加
		/// </summary>
		private void AddContent(PolyPartsObject polyObj) {
			
		}

		#endregion
	}
}