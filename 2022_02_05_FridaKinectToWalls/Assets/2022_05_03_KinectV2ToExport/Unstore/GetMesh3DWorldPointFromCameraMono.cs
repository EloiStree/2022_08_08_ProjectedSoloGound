using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GetMesh3DWorldPointFromCameraMono : MonoBehaviour
{

	[Tooltip("Camera used to convert the screen point to world point.")]
	public Camera foregroundCamera;
	public KinectManager kinectManager;
	private int colorWidth = 0;
	private int colorHeight = 0;
	private Rect backgroundRect;



	public long m_timeToFetchInMs;
	void Start()
	{
		if (foregroundCamera == null)
		{
			foregroundCamera = Camera.main;
		}

		// get the singleton instance of KMif
		if (kinectManager == null)
			kinectManager = KinectManager.Instance;

		// get color-image resolution
		if (kinectManager && kinectManager.IsInitialized())
		{
			colorWidth = kinectManager.GetColorImageWidth();
			colorHeight = kinectManager.GetColorImageHeight();
		}

		// estimate the background rectangle
		backgroundRect = foregroundCamera ? foregroundCamera.pixelRect : new Rect(0, 0, Screen.width, Screen.height);
	}

	public void GetKinectLocalSpaceFromDepth(int depthX, int depthY, out Vector3 kinectLocalPosition)
    {
		if (kinectManager != null && kinectManager.GetSensorData() != null)
			kinectLocalPosition = kinectManager.GetSensorData().depth2SpaceCoords[depthY * kinectManager.GetDepthImageWidth() + depthX];
		else kinectLocalPosition = Vector3.zero;

	}

    public void GetMeshWorldPointOfScreenPointPercent(in float xPercent, in int yPercent, out Vector3 worldPointOfMesh)
	{
		GetMeshWorldPointOfScreenPoint((int)(xPercent * Screen.width), (int)(xPercent * Screen.height), out worldPointOfMesh);
	}

	public void GetAllWorldPoints(out Vector3[] points)
	{
		int depthWidth = kinectManager.GetDepthImageWidth();
		int dephtHeight = kinectManager.GetDepthImageHeight();
		throw new NotImplementedException();
		//Stopwatch watch = new Stopwatch();
		//watch.Start();

		//// get mouse button state and position

		//if (kinectManager && kinectManager.IsInitialized())
		//{
		//	for (int x = 0; x < depthWidth; x++)
		//	{
		//		for (int y = 0; y < dephtHeight; y++)
		//		{

		//			// screen position
		//			Vector2 screenPos = new Vector2(x, y);
		//			// update the background rectangle with the portrait background, if available
		//			PortraitBackground portraitBack = PortraitBackground.Instance;
		//			if (portraitBack && portraitBack.enabled)
		//			{
		//				backgroundRect = portraitBack.GetBackgroundRect();
		//			}
		//			// convert to color image rectangle
		//			float colorX = (screenPos.x - backgroundRect.x) * (float)colorWidth / backgroundRect.width;
		//			float colorY = (backgroundRect.y + (backgroundRect.height - screenPos.y)) * (float)colorHeight / backgroundRect.height;
		//			Vector2 colorPos = new Vector2(colorX, colorY);

		//			// get the respective depth image pos
		//			Vector2 depthPos = kinectManager.MapColorPointToDepthCoords(colorPos, true);
		//			if (depthPos != Vector2.zero)
		//			{
		//				// get the depth in mm
		//				ushort depthValue = kinectManager.GetDepthForPixel((int)depthPos.x, (int)depthPos.y);

		//				// get the space position in world coordinates
		//				Vector3 worldPos = foregroundCamera ? foregroundCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, (float)depthValue / 1000f)) :
		//					kinectManager.MapDepthPointToSpaceCoords(depthPos, depthValue, true);

		//				// set the overlay object's position
		//				if (!float.IsNaN(worldPos.x) && !float.IsNaN(worldPos.y) && !float.IsNaN(worldPos.z))
		//				{
		//					worldPointOfMesh = worldPos;
		//				}
		//			}

		//		}
		//	}
		//}

		//watch.Stop();
		//m_timeToFetchInMs = watch.ElapsedMilliseconds;

	}
	public void GetMeshWorldPointOfScreenPoint(in int x, in int y, out Vector3 worldPointOfMesh)
	{

		// get mouse button state and position

		worldPointOfMesh = Vector3.zero;
		if (kinectManager && kinectManager.IsInitialized() && x >= 0 && y >= 0 && x < Screen.width && y < Screen.height
			)
		{
			// screen position
			Vector2 screenPos = new Vector2(x, y);
			// update the background rectangle with the portrait background, if available
			PortraitBackground portraitBack = PortraitBackground.Instance;
			if (portraitBack && portraitBack.enabled)
			{
				backgroundRect = portraitBack.GetBackgroundRect();
			}
			// convert to color image rectangle
			float colorX = (screenPos.x - backgroundRect.x) * (float)colorWidth / backgroundRect.width;
			float colorY = (backgroundRect.y + (backgroundRect.height - screenPos.y)) * (float)colorHeight / backgroundRect.height;
			Vector2 colorPos = new Vector2(colorX, colorY);

			// get the respective depth image pos
			Vector2 depthPos = kinectManager.MapColorPointToDepthCoords(colorPos, true);
			if (depthPos != Vector2.zero)
			{
				// get the depth in mm
				ushort depthValue = kinectManager.GetDepthForPixel((int)depthPos.x, (int)depthPos.y);

				// get the space position in world coordinates
				Vector3 worldPos = foregroundCamera ? foregroundCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, (float)depthValue / 1000f)) :
					kinectManager.MapDepthPointToSpaceCoords(depthPos, depthValue, true);

				// set the overlay object's position
				if (!float.IsNaN(worldPos.x) && !float.IsNaN(worldPos.y) && !float.IsNaN(worldPos.z))
				{
					worldPointOfMesh = worldPos;
				}
			}

		}
	}


		public void GetDepthCoordinateOfScreenPixel(in Vector2 screenPoint, out int xr, out int yr)
	{
		GetDepthCoordinateOfScreenPixel((int)screenPoint.x, (int)screenPoint.y,out  xr,out  yr);

	}
		public void GetDepthCoordinateOfScreenPixel(in int x,in  int y, out int xr, out int yr)
	{
		xr = -1;
		yr = -1;


		if (kinectManager && kinectManager.IsInitialized() && x >= 0 && y >= 0 && x < Screen.width && y < Screen.height
			)
		{
			// screen position
			Vector2 screenPos = new Vector2(x, y);
			// update the background rectangle with the portrait background, if available
			PortraitBackground portraitBack = PortraitBackground.Instance;
			if (portraitBack && portraitBack.enabled)
			{
				backgroundRect = portraitBack.GetBackgroundRect();
			}
			// convert to color image rectangle
			float colorX = (screenPos.x - backgroundRect.x) * (float)colorWidth / backgroundRect.width;
			float colorY = (backgroundRect.y + (backgroundRect.height - screenPos.y)) * (float)colorHeight / backgroundRect.height;
			Vector2 colorPos = new Vector2(colorX, colorY);

			// get the respective depth image pos
			Vector2 depthPos = kinectManager.MapColorPointToDepthCoords(colorPos, true);
			xr = (int)depthPos.x;
			yr = (int)depthPos.y;
		}
	}

	public void GetScreenPixelMaxInfo(Vector2 screenPosition, out int x, out int y, out ushort depthFound, out Vector3 worldPositionFound)
	{

		worldPositionFound = Vector3.zero;
		depthFound = 0;
		x = -1;
		y = -1;

		if (kinectManager && kinectManager.IsInitialized()
			 && screenPosition.x >= 0 && screenPosition.y >= 0 && screenPosition.x < Screen.width && screenPosition.y < Screen.height
			)
		{

			Vector2 screenPos = screenPosition;

			// update the background rectangle with the portrait background, if available
			PortraitBackground portraitBack = PortraitBackground.Instance;
			if (portraitBack && portraitBack.enabled)
			{
				backgroundRect = portraitBack.GetBackgroundRect();
			}

			// convert to color image rectangle
			float colorX = (screenPos.x - backgroundRect.x) * (float)colorWidth / backgroundRect.width;
			float colorY = (backgroundRect.y + (backgroundRect.height - screenPos.y)) * (float)colorHeight / backgroundRect.height;
			Vector2 colorPos = new Vector2(colorX, colorY);

			// get the respective depth image pos
			Vector2 depthPos = kinectManager.MapColorPointToDepthCoords(colorPos, true);

			if (depthPos != Vector2.zero)
			{
				x = (int)depthPos.x;
				y = (int)depthPos.y;
				// get the depth in mm
				ushort depthValue = kinectManager.GetDepthForPixel(x, y);
				depthFound = depthValue;
				// get the space position in world coordinates
				Vector3 worldPos = foregroundCamera ? foregroundCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, (float)depthValue / 1000f)) :
					kinectManager.MapDepthPointToSpaceCoords(depthPos, depthValue, true);

				// set the overlay object's position
				if (!float.IsNaN(worldPos.x) && !float.IsNaN(worldPos.y) && !float.IsNaN(worldPos.z))
				{
					worldPositionFound = worldPos;

				}
				else worldPositionFound = Vector3.zero;
			}

		}
	}

}










