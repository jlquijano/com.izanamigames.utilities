using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.izanamigames.lib
{
    public class GizmoDirection : MonoBehaviour
    {

        public float longitud = 1;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            Debug.DrawLine(transform.position, transform.position + transform.right * longitud, Color.red);
        }
    }
}
