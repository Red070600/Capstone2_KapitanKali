using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JRLB
{

    public class CharacterManager : MonoBehaviour
    {
        [Header("Lock On Transform")]
        public Transform lockOnTransform;

        [Header("Movement Flags")]
        public bool canRotate;
    }
}
