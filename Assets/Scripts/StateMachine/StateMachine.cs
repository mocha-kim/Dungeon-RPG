using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.AI
{

    public abstract class State<T>
    {
        protected StateMachine<T> stateMachine;
        protected T context;

        public State()
        {

        }

        internal void SetStateMachineAndContext(StateMachine<T> stateMachine, T context)
        {
            this.stateMachine = stateMachine;
            this.context = context;

            OnInitialized();
        }

        public virtual void OnInitialized()
        { }

        public virtual void OnEnter()
        { }

        public abstract void Update(float deltaTime);

        public virtual void OnExit()
        { }
    }

    public sealed class StateMachine<T>
    {
        private T context;

        private State<T> currentState;
        public State<T> Currentstate => currentState;

        private State<T> priviousState;
        public State<T> PriviousState => priviousState;

        private float elapsedTimeInState = 0.0f;
        public float ElapsedTimeInState => elapsedTimeInState;

        private Dictionary<System.Type, State<T>> states = new Dictionary<System.Type, State<T>>();

        // Constructor
        public StateMachine(T context, State<T> initialState)
        {
            this.context = context;

            // Set initial state
            AddState(initialState);
            currentState = initialState;
            currentState.OnEnter();
        }

        public void AddState(State<T> state)
        {
            state.SetStateMachineAndContext(this, context);
            states[state.GetType()] = state;
        }

        public void Update(float deltaTime)
        {
            elapsedTimeInState += deltaTime;

            currentState.Update(deltaTime);
        }

        public R ChangeState<R>() where R : State<T>
        {
            var newType = typeof(R);

            if (currentState.GetType() == newType)
                return currentState as R;

            if (currentState != null)
                currentState.OnExit();

            priviousState = currentState;
            currentState = states[newType];
            currentState.OnEnter();

            elapsedTimeInState = 0.0f;

            return currentState as R;
        }
    }
}
