using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JRLB
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Idle Animations")]
        public string right_hand_idle;
        public string left_hand_idle;


        [Header("Attack Animation")]
        public string LightAttack_1;
        public string LightAttack_2;
        public string HeavyAttack_1;

        [Header("Stamina Cost")]
        public int baseStamina;
        public float lightAttackMultiplier;
        public float heavyAttackMultiplier;
    }
}

