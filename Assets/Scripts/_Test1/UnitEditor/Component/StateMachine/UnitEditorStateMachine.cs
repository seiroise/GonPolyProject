using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts._Test1.UnitEditor.Component.StateMachine {

	/// <summary>
	/// ユニットエディタの状態機械
	/// </summary>
	public class UnitEditorStateMachine : UnitEditorState {

		[Header("State")]
		public UnitEditorState defaultState;	//標準子状態
		public List<UnitEditorState> states;	//子状態リスト
		private Dictionary<string, UnitEditorState> stateDic;	//状態辞書
		private UnitEditorState activeState;	//有効な子状態

		#region VirtualFunction

		/// <summary>
		/// 使わない
		/// </summary>
		[Obsolete]
		public override void Initialize(UnitEditor unitEditor) {
			Initialize(unitEditor, null);
		}

		/// <summary>
		/// 初期化
		/// </summary>
		public override void Initialize(UnitEditor unitEditor, UnitEditorStateMachine owner) {
			base.Initialize(unitEditor, owner);

			//状態の初期化
			InitializeStates(states);
		}

		/// <summary>
		/// 後初期化
		/// </summary>
		public override void LateInitialize() {
			base.LateInitialize();

			//状態の後初期化
			LateInitializeStates(states);

			//標準状態の有効化
			ActivateDefault();

			//親がnullならそのまま開始
			if (owner == null) Enter();
		}

		/// <summary>
		/// 終了
		/// </summary>
		public override void Exit() {
			base.Exit();

			//有効化されている状態を無効化
			DisactivateState();
		}

		/// <summary>
		/// 子状態が有効になった時(どの子状態も有効でなかった場合)
		/// </summary>
		protected virtual void OnActivateState() { }

		/// <summary>
		/// 子状態が無効になった時(どの子状態も有効にならなかった場合)
		/// </summary>
		protected virtual void OnDisactivateState() { }

		#endregion

		#region StateFunction

		/// <summary>
		/// 状態の初期化
		/// </summary>
		private void InitializeStates(List<UnitEditorState> states) {
			if (states == null) return;

			//テーブルに追加
			stateDic = new Dictionary<string, UnitEditorState>();
			for (int i = 0; i < states.Count; ++i) {
				stateDic.Add(states[i].name, states[i]);
			}

			//初期化
			for (int i = 0; i < states.Count; ++i) {
				states[i].Initialize(unitEditor, this);
			}
		}

		/// <summary>
		/// 状態の後初期化
		/// </summary>
		private void LateInitializeStates(List<UnitEditorState> coms) {
			if (coms == null) return;

			//後初期化
			for (int i = 0; i < coms.Count; ++i) {
				coms[i].LateInitialize();
			}
		}

		/// <summary>
		/// 指定した状態の有効化
		/// </summary>
		public void ActivateState(string name) {
			//アクティブでなければ有効化しない
			if (!activated) return;

			//現在の状態
			bool prevNull = (activeState == null);

			//現在の状態と同じか確認
			if(!prevNull && activeState.name.Equals(name)) return;

			//現在有効化されている状態の無効化
			DisactivateState();

			//有効化する状態の取得
			activeState = GetState(name);
			if (activeState) {
				if (prevNull) OnActivateState();
				activeState.Enter();
			}
		}

		/// <summary>
		/// 標準状態の有効化
		/// </summary>
		public void ActivateDefault() {
			if (!defaultState) return;
			ActivateState(defaultState.name);
		}

		/// <summary>
		/// 有効化されている状態の無効化
		/// </summary>
		public void DisactivateState() {
			if (activeState == null) return;
			OnDisactivateState();
			activeState.Exit();
			activeState = null;
		}

		/// <summary>
		/// この状態の無効化
		/// </summary>
		public void DisactivateThis() {
			if (!owner) return;
			owner.DisactivateState();
		}

		/// <summary>
		/// 指定した状態の取得
		/// </summary>
		public UnitEditorState GetState(string name) {
			if (!stateDic.ContainsKey(name)) return null;
			return stateDic[name];
		}

		#endregion
	}
}