using UnityEngine;
using System;
using System.Collections.Generic;

namespace Scripts._Test1.UnitEditor.Component.Utility.Marker {

	/// <summary>
	/// ユニットエディタで使うマーカーの管理
	/// </summary>
	public class UnitEditorMarker : UnitEditorComponent {

		[Header("MarkerPool")]
		public List<UnitEditorMarkerPool> markerPools;
		private Dictionary<string, UnitEditorMarkerPool> markerPoolTable;

		#region VirtualFunction

		public override void Initialize(UnitEditor unitEditor) {
			base.Initialize(unitEditor);

			//マーカープールの初期化
			InitializeMarkerPool(markerPools);
		}

		#endregion

		#region Function

		/// <summary>
		/// プールの初期化
		/// </summary>
		private void InitializeMarkerPool(List<UnitEditorMarkerPool> markerPools) {
			markerPoolTable = new Dictionary<string, UnitEditorMarkerPool>();
			for (int i = 0; i < markerPools.Count; ++i) {
				markerPoolTable.Add(markerPools[i].name, markerPools[i]);
			}
		}

		/// <summary>
		/// スプライトマーカーの取得
		/// </summary>
		public SpriteMarker PopMarker(string name, Vector2 point) {
			if (!markerPoolTable.ContainsKey(name)) return null;
			return markerPoolTable[name].PopItem(point);
		}

		/// <summary>
		/// 複数のスプライトマーカーの取得
		/// </summary>
		public List<SpriteMarker> PopMarkers(string name, int num, Vector2 point) {
			if (!markerPoolTable.ContainsKey(name)) return null;
			return markerPoolTable[name].PopItems(num, point);
		}

		#endregion

	}
}