using UnityEngine;
using System.Collections.Generic;
using Scripts._Test.PolyPartsEditor.Common;

namespace Scripts._Test.PolyPartsEditor.Component.Marker {

	/// <summary>
	/// ポリゴンパーツエディタのマーカー管理
	/// </summary>
	public class PolyPartsMarker : PolyPartsEditorComponent{

		public SpritePool[] markerPool;
		private Dictionary<string, SpritePool> markerPoolTable;

		#region VirtualFunction

		public override void Initialize(PolyPartsEditor editor) {
			base.Initialize(editor);
			//マーカープールの初期化
			InitializeMarkerPool(markerPool);
		}

		#endregion

		#region Function

		/// <summary>
		/// マーカープールの初期化
		/// </summary>
		private void InitializeMarkerPool(SpritePool[] markerPool) {
			markerPoolTable = new Dictionary<string, SpritePool>();
			foreach(var e in markerPool) {
				markerPoolTable.Add(e.name, e);
			}
		}

		/// <summary>
		/// マーカーの取得
		/// </summary>
		public SpritePoolItem PopMarker(string name, Vector2 point) {
			if(!markerPoolTable.ContainsKey(name)) return null;
			return markerPoolTable[name].PopItem(point);
		}

		/// <summary>
		/// マーカーの取得
		/// </summary>
		public List<SpritePoolItem> PopMarkers(string name, int num, Vector2 point) {
			if(!markerPoolTable.ContainsKey(name)) return null;
			return markerPoolTable[name].PopItems(num, point);
		}

		#endregion
	}
}