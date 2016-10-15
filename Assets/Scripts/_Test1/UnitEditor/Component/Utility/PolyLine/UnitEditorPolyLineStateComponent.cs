using Seiro.Scripts.FSM;
using System;

namespace Scripts._Test1.UnitEditor.Component.Utility.PolyLine {
	
	/// <summary>
	/// ユニットエディタのポリライン状態コンポーネント
	/// </summary>
	public abstract class UnitEditorPolyLineStateComponent : UnitEditorPolyLineComponent, IState {

		protected bool active = false;

		#region IState

		public virtual void Enter() {
			active = true;
		}

		public virtual void Exit() {
			active = false;
		}

		#endregion
	}
}