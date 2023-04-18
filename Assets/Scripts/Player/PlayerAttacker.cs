using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JRLB
{
    public class PlayerAttacker : MonoBehaviour
    {
        WeaponSlotManager weaponSlotManager;
        AnimatorHandler animatorHandler;
        InputHandler inputHandler;
        PlayerStats playerStats;
        public string lastAttack;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            inputHandler = GetComponent<InputHandler>();
            playerStats = GetComponent<PlayerStats>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
                return;

            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);
                if (lastAttack == weapon.LightAttack_1)
                {
                    animatorHandler.PlayTargetAnimation(weapon.LightAttack_2, true);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
                return;

            weaponSlotManager.attackingWeapon = weapon;

            animatorHandler.PlayTargetAnimation(weapon.LightAttack_1, true);
            lastAttack = weapon.LightAttack_1;
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
                return;

            weaponSlotManager.attackingWeapon = weapon;
            animatorHandler.PlayTargetAnimation(weapon.HeavyAttack_1, true);
            lastAttack = weapon.HeavyAttack_1;
        }
    }
}
