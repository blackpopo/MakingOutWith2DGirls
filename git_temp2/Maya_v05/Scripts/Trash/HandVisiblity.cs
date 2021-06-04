using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandVisibility : MonoBehaviour
{
	[SerializeField] private SkinnedMeshRenderer _renderer;


	private OVRMeshRenderer.IOVRMeshRendererDataProvider _dataProvider;





	private void Awake()
	{
		_dataProvider = GetComponent<OVRMeshRenderer.IOVRMeshRendererDataProvider>();
	}



	private void Update()
	{
		OVRMeshRenderer.MeshRendererData data = _dataProvider.GetMeshRendererData();

		bool isDataValid = data.IsDataValid;
		bool isDataHighConfidence = data.IsDataHighConfidence;

		bool shouldRender = isDataValid && isDataHighConfidence;

		if (_renderer.enabled != shouldRender)
		{
			_renderer.enabled = shouldRender;
		}
	}
}