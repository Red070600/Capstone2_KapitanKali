using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JRLB
{
    public class AttackState : State
    {
        public CombatStanceState combatStanceState;
        public PursueTargetState pursueTargetState;
        public EnemyAttackAction currentAttack;

        public bool hasPerformedAttack = false;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            RotateTowardsTargetWhileAttacking(enemyManager);

            if (distanceFromTarget > enemyManager.maximumAttackRange)
            {
                return pursueTargetState;
            }

           /* if (willDoComboOnNextAttack && enemyManager.canDoCombo)
            {
                //ATTACK WITH COMBO
                //SET COOL DOWN TIME
            }
           */

            if (!hasPerformedAttack)
            {
                AttackTarget(enemyAnimatorManager, enemyManager);
            }

            /*
            if (willDoComboOnNextAttack && hasPerformedAttack)
            {
                return this;
            }
            */

            if (hasPerformedAttack)
            {
                return this;
            }

            return combatStanceState;
        }
        private void AttackTarget(EnemyAnimatorManager enemyAnimatorManager, EnemyManager enemyManager)
        {
            enemyAnimatorManager.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            hasPerformedAttack = true;
        }

        private void RotateTowardsTargetWhileAttacking(EnemyManager enemyManager)
        {
            //Manually Rotation
            if (enemyManager.canRotate)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }
    }
}