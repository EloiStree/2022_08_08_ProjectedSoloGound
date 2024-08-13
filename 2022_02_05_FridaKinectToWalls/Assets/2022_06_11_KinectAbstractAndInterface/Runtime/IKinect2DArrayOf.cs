using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eloi.Array2D;



public class KinectFromTransformWoldVector3Array : Abstract1DArrayOfValueGet<Vector3>
{
    public KinectFromTransformWoldVector3Array(int width, int height, Vector3[] arrayByRef = null) : base(width, height, arrayByRef)
    {  }
}
public class KinectLocalSpaceCoordinate3Array : Abstract1DArrayOfValueGet<Vector3>
{
    public KinectLocalSpaceCoordinate3Array(int width, int height, Vector3[] arrayByRef = null) : base(width, height, arrayByRef)
    {  }
}
public class KinectUshortDepthInMMArray : Abstract1DArrayOfValueGet<ushort>
{
    public KinectUshortDepthInMMArray(int width, int height, ushort[] arrayByRef = null) : base(width, height, arrayByRef)
    { }
}
public class KinectBooleanIntArray : Abstract1DArrayOfValueGet<int>
{
    public KinectBooleanIntArray(int width, int height, int[] arrayByRef = null) : base(width, height, arrayByRef)
    { }
}

public interface ITextCompressable
{
    public void GetAsText(out string text);
    public void SetFromText(in string text);
}
public interface IByteCompressable
{
    public void GetAsText(out byte[] text);
    public void SetFromText(in byte[] text);
}