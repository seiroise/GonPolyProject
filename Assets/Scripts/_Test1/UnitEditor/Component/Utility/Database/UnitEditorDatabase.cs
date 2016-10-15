using UnityEngine;
using System.Collections.Generic;
using Scripts._Test.PolyPartsEditor.Common;

namespace Scripts._Test1.UnitEditor.Component.Utility.Database {
	
	/// <summary>
	/// ユニットエディタのデータベース
	/// </summary>
	public class UnitEditorDatabase : UnitEditorComponent {

		[Header("Prefab")]
		public PolyPartsObject prefab;

		//内部パラメータ
		private List<PolyPartsObject> polyObjs;

		#region VirtualFunction

		public override void Initialize(UnitEditor unitEditor) {
			base.Initialize(unitEditor);

			polyObjs = new List<PolyPartsObject>();
		}

		#endregion

		#region Function

		/// <summary>
		/// ポリゴンオブジェクトの初期化
		/// </summary>
		private void InitializePolyObj(PolyPartsObject polyObj) {
			//親の設定
			polyObj.transform.SetParent(transform);

			//追加
			polyObjs.Add(polyObj);
		}

		/// <summary>
		/// ポリゴンオブジェクトの生成
		/// </summary>
		public PolyPartsObject InstantiatePolyObj(List<Vector2> vertices) {
			//生成と頂点の設定
			PolyPartsObject polyObj = Instantiate<PolyPartsObject>(prefab);
			polyObj.SetVertices(vertices);

			//初期化
			InitializePolyObj(polyObj);

			return polyObj;
		}

		#endregion
	}
}