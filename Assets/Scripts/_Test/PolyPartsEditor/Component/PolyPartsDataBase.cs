using UnityEngine;
using System.Collections.Generic;
using Seiro.Scripts.Geometric.Polygon.Concave;
using Scripts._Test.PolyPartsEditor.Polygon;

namespace Scripts._Test.PolyPartsEditor.Component {

	/// <summary>
	/// ポリゴンパーツのデータベース
	/// </summary>
	public class PolyPartsDatabase : MonoBehaviour {

		[Header("Prefab")]
		public PolyPartsObject prefab;

		[Header("Callback")]
		public PolyPartsObjectEvent onPolyObjClicked;

		//内部パラメータ
		private List<PolyPartsObject> polyObjs;

		#region UnityEvent

		private void Awake() {
			polyObjs = new List<PolyPartsObject>();
		}

		#endregion

		#region Function

		/// <summary>
		/// ポリゴンオブジェクトの生成
		/// </summary>
		public void InstantiatePolygon(List<Vector2> vertices) {
			PolyPartsObject polyObj = Instantiate<PolyPartsObject>(prefab);
			polyObj.transform.SetParent(transform);
			polyObj.SetVertices(vertices);
			polyObj.onClick.AddListener(OnPolyObjClicked);
			//追加
			polyObjs.Add(polyObj);
		}

		/// <summary>
		/// 全てのポリゴンを有効化する
		/// </summary>
		public void EnablePolygons() {
			for(int i = 0; i < polyObjs.Count; ++i) {
				polyObjs[i].Enable();
			}
		}

		/// <summary>
		/// 全てのポリゴンを無効化する
		/// </summary>
		public void DisablePolygons() {
			for(int i = 0; i < polyObjs.Count; ++i) {
				polyObjs[i].Disable();
			}
		}

		#endregion

		#region Callback

		/// <summary>
		/// ポリゴンオブジェクトのクリック
		/// </summary>
		private void OnPolyObjClicked(PolyPartsObject polyObj) {
			onPolyObjClicked.Invoke(polyObj);
		}

		#endregion
	}
}