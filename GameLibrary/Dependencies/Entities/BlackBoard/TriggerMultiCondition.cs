using System;

namespace GameLibrary.Dependencies.Entities
{
    public class TriggerMultiCondition : Trigger
    {
        private Func<BlackBoard, TriggerState, bool> Condition;
        private Action<TriggerState> onFire;

        public TriggerMultiCondition(Func<BlackBoard, TriggerState, bool> Condition, params string[] Names)
            : this(Condition, null, Names) { }

        public TriggerMultiCondition(Func<BlackBoard, TriggerState, bool> Condition, Action<TriggerState> onFire, params String[] Names)
        {
            this.WorldPropertiesMonitored.AddRange(Names);
            this.Condition = Condition;
            this.onFire = onFire;
        }

        public override void RemoveThisTrigger()
        {
            BlackBoard.RemoveTrigger(this);
        }

        protected override bool CheckConditionToFire()
        {
            return Condition(BlackBoard, TriggerState);
        }

        protected override void CalledOnFire(TriggerState TriggerState)
        {
            if (onFire != null)
                onFire(TriggerState);
        }
    }
}