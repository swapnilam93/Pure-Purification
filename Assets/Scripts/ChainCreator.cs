using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainCreator : MonoBehaviour {
	public GameObject knotPrepab;
	public Rigidbody StartBody, EndBody;
	public float knotLength = 0.3f;
	public int KnotCount = 10;
	List<GameObject> joints;
	// Use this for initialization
	void Start () {
		var StartPos = StartBody.transform.position;
		var EndPos = EndBody.transform.position;
		joints = new List<GameObject>();
		knotLength = Vector3.Distance(StartPos, EndPos) / KnotCount;
		for (int i = 0; i < KnotCount; i++) {
			float t = (float)i / KnotCount, mid_t = ((float)i + 0.5f) / KnotCount;
			var nowAnchor = StartPos * (1 - t) + EndPos * t;
			var scale = new Vector3(0.03f, knotLength, 0.03f);
			var nowObject = Instantiate(knotPrepab, StartPos * (1 - mid_t) + EndPos * mid_t, Quaternion.identity, transform);
			nowObject.name = "Knot " + i.ToString();
			nowObject.transform.localScale = scale;
			joints.Add(nowObject);
			var hingeJoint = nowObject.GetComponent<FixedJoint>();
			// hingeJoint.maxDistance = knotLength / 2;
			if (i > 0) {
				hingeJoint.connectedBody = joints[i - 1].GetComponent<Rigidbody>();
			} 
			else {
				hingeJoint.connectedBody = StartBody;
			}
			// hingeJoint.anchor = nowAnchor;
		}
		var endHinge = EndBody.gameObject.GetComponent<FixedJoint>();
		endHinge.connectedBody = joints[joints.Count - 1].GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
