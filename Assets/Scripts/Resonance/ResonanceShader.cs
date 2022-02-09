using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResonanceShader : MonoBehaviour {
	private Camera cam;
	
	void Start () {
		cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		bool d3d = SystemInfo.graphicsDeviceVersion.IndexOf("Direct3D") > -1;
		Matrix4x4 M = transform.localToWorldMatrix;
		Matrix4x4 V = cam.worldToCameraMatrix;
		Matrix4x4 P = cam.projectionMatrix;
		if (d3d) {
			// Invert Y for rendering to a render texture
			for (int i = 0; i < 4; i++) {
				P[1,i] = -P[1,i];
			}
			// Scale and bias from OpenGL -> D3D depth range
			for (int i = 0; i < 4; i++) {
				P[2,i] = P[2,i]*0.5f + P[3,i]*0.5f;
			}
		}
		Matrix4x4 MVP = M*V*P;
		
		Shader.SetGlobalMatrix("_resonanceMatrix", MVP);
	}
}
