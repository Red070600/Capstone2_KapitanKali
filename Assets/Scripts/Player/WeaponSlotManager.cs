using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JRLB
{
	public class WeaponSlotManager : MonoBehaviour
	{
		PlayerManager playerManager;
		PlayerInventory playerInventory;
		public WeaponItem attackingWeapon;

		public WeaponHolderSlot leftHandSlot;
		public WeaponHolderSlot rightHandSlot;

		DamageCollider leftHandDamageCollider;
		DamageCollider rightHandDamageCollider;

		Animator animator;

		QuickSlotsUI quickSlotsUI;

		PlayerStats playerStats;

		private void Awake()
		{
			playerManager = GetComponentInParent<PlayerManager>();
			playerInventory = GetComponentInParent<PlayerInventory>();
			animator = GetComponent<Animator>();
			quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
			playerStats = GetComponentInParent<PlayerStats>();

			WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
			foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
			{
				if (weaponSlot.isLeftHandSlot)
				{
					leftHandSlot = weaponSlot;
				}
				else if (weaponSlot.isRightHandSlot)
				{
					rightHandSlot = weaponSlot;
				}
			}
		}

		public void LoadBothWeaponsOnSlots()
        {
			LoadWeaponOnSlot(playerInventory.rightWeapon, false);
			LoadWeaponOnSlot(playerInventory.leftWeapon, true);
		}

		public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
		{
			if (isLeft)
			{
				leftHandSlot.LoadWeaponModel(weaponItem);
				LoadLeftWeaponDamageCollider();
				quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                #region Handle Left Weapon Idle Animations
                if (weaponItem != null)
                {
					animator.CrossFade(weaponItem.left_hand_idle, 0.2f);
                }
				else
                {
					animator.CrossFade("Left Arm Empty", 0.2f);
                }

                #endregion
            }
            else
			{
				rightHandSlot.LoadWeaponModel(weaponItem);
				LoadRightWeaponDamageCollider();
				quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
				#region Handle Right Weapon Idle Animations
				if (weaponItem != null)
				{
					animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
				}
				else
				{
					animator.CrossFade("Right Arm Empty", 0.2f);
				}
				#endregion
			}
		}

        #region Handle Weapon's Damage Collider
        
        private void LoadLeftWeaponDamageCollider()
        {
			leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

		private void LoadRightWeaponDamageCollider()
		{
			rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
		}

		public void OpenDamageCollider()
        {
			if (playerManager.isUsingRightHand)
            {
				rightHandDamageCollider.EnableDamageCollider();
			}
			else if (playerManager.isUsingLeftHand)
            {
				leftHandDamageCollider.EnableDamageCollider();
            }
        }

		public void CloseDamageCollider()
		{
			rightHandDamageCollider.DisableDamageCollider();
			leftHandDamageCollider.DisableDamageCollider();
		}

        #endregion

        #region Handle Weapon's Stamina Drainage

        public void DrainStaminaLightAttack()
        {
			playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }

		public void DrainStaminaHeavyAttack()
		{
			playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
		}
       
		#endregion
    }
}