// Cecconoid by Triple Eh? Ltd -- 2019
//
// Cecconoid_CC is distributed under a CC BY 4.0 license. You are free to:
//
// * Share copy and redistribute the material in any medium or format
// * Adapt remix, transform, and build upon the material for any purpose, even commercially.
//
// As long as you give credit to Gareth Noyce / Triple Eh? Ltd.
//
//

using UnityEngine;

public class Line : MonoBehaviour
{
	public Vector3 vStart, vEnd;
	public Vector3 GetStart() { return transform.position + vStart; }
	public Vector3 GetEnd() { return transform.position + vEnd; }
}
