using System;
using Unity.VisualScripting;
using UnityEngine;


namespace Misc
{
    public class RotateConstantly : MonoBehaviour
    {
        private Transform _parentTransform;
        public Vector3 rotation;
        private void Awake()
        {
            _parentTransform = gameObject.GetComponentInParent<Transform>();
        }

        // Update is called once per frame
        void Update()
        {
            _parentTransform.Rotate(rotation);
        }
    }
}
