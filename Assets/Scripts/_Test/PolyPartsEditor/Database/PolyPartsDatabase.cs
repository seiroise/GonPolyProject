using UnityEngine;
using System.Collections.Generic;
using Scripts._Test.PolyPartsEditor.Common;

namespace Scripts._Test.PolyPartsEditor.Database {

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
		public PolyPartsObject InstantiatePolygon(List<Vector2> vertices) {
			PolyPartsObject polyObj = Instantiate<PolyPartsObject>(prefab);
			polyObj.transform.SetParent(transform);
			polyObj.SetVertices(vertices);
			polyObj.onClick.AddListener(OnPolyObjClicked);
			//追加
			polyObjs.Add(polyObj);

			return polyObj;
		}

		/// <summary>
		/// ポリゴンオブジェクトの複製を作成する
		/// </summary>
		public PolyPartsObject InstantiateClone(PolyPartsObject polyObj, Vector2 point) {
			PolyPartsObject clone = polyObj.InstantiateClone(point);
			clone.onClick.AddListener(OnPolyObjClicked);
			//追加
			polyObjs.Add(clone);

			return clone;
		}

		/// <summary>
		/// ポリゴンオブジェクトの削除
		/// </summary>
		public void DeletePolyPartsObject(PolyPartsObject polyObj) {
			if(polyObjs.Remove(polyObj)) {
				Destroy(polyObj.gameObject);
			}
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
		/// 指定したポリゴン以外を有効化する
		/// </summary>
		public void EnablePolygons(PolyPartsObject ignore) {
			EnablePolygons();
			ignore.Disable();
		}

		/// <summary>
		/// 全てのポリゴンを無効化する
		/// </summary>
		public void DisablePolygons() {
			for(int i = 0; i < polyObjs.Count; ++i) {
				polyObjs[i].Disable();
			}
		}

		/// <summary>
		/// 指定したポリゴン以外を無効化する
		/// </summary>
		public void DisablePolygons(PolyPartsObject ignore) {
			DisablePolygons();
			ignore.Enable();
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