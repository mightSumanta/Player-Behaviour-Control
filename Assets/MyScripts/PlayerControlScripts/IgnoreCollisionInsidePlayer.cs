using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Example
{
	public class IgnoreCollisionInsidePlayer : MonoBehaviour 
	{

		public Collider mainCollider;
		public Collider[] ragdollColliders;

		void OnEnable()
		{
			initiate ();
		}

		void initiate()
		{
			foreach (Collider col in ragdollColliders) 
			{
                if (col)
				    Physics.IgnoreCollision (mainCollider, col);
			}
		}
	}

}
