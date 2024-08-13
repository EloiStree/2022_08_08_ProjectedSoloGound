using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Eloi.Array2D
{

    public interface IIsStoreAs1DArray<T> { void GetArray(out T[] array); }

    public interface IIsStoreAs2DArray<T> { void GetArray(out T[,] array); }

    public interface IArray2DSize
    {
        public void GetWidth(out int width);
        public void GetHeight(out int height);
        public void Get1DLenght(out int lenght);
    }

    public interface IIndexToPercentHorizontal : IArray2DSize
    {
        public void GetXLeftRightPercent(in float percent, out int index);
        public void GetXRightLeftPercent(in float percent, out int index);
    }
    public interface IPercentToIndexHorizontal : IArray2DSize
    {
        public void GetXLeftRightPercent(in float percent, out int index);
        public void GetXRightLeftPercent(in float percent, out int index);
    }
    public interface IIndexToPercentVertical : IArray2DSize
    {
        public void GetYTopDownPercent(in float percent, out int index);
        public void GetYDownTopPercent(in float percent, out int index);
    }

    public interface IPercentToIndexVertical : IArray2DSize
    {
        public void GetYTopDownPercent(in float percent, out int index);
        public void GetYDownTopPercent(in float percent, out int index);
    }
    public interface IArray2DWithDirectionIndex : IArray2DSize
    {
        public void GetXY_LRTDIndex1D(in int x, in int y, out int index1D);
        public void GetXY_LRDTIndex1D(in int x, in int y, out int index1D);
        public void GetXY_RLTDIndex1D(in int x, in int y, out int index1D);
        public void GetXY_RLDTIndex1D(in int x, in int y, out int index1D);
    }
    public interface IArray2DWithDirectionSet<T> : IArray2DSize, IArray2DWithDirectionIndex
    {
        public void SetXY_LRTD(in int x, in int y, T value);
        public void SetXY_LRDT(in int x, in int y, T value);
        public void SetXY_RLTD(in int x, in int y, T value);
        public void SetXY_RLDT(in int x, in int y, T value);
    }
    public interface IArray2DWithDirectionGet<T> : IArray2DSize, IArray2DWithDirectionIndex
    {
        public void GetXY_LRTD(in int x, in int y, out T value);
        public void GetXY_LRDT(in int x, in int y, out T value);
        public void GetXY_RLTD(in int x, in int y, out T value);
        public void GetXY_RLDT(in int x, in int y, out T value);
    }



    public interface IArrayClassicSet<T> : IArray2DSize
    {
        public void SetClassic2D(in int x, in int y, T value);
        public void SetClassic1D(in int index1D, T value);

    }

    public interface IArrayClassic2DGet<T> : IArray2DSize
    {
        public void GetClassic2D(in int x, in int y, out T value);
        public void GetClassic1D(in int index1D, out T value);
    }

    public interface IComputeBufferableGet<T>
    {

        public void CreateComputeBuffer(out ComputeBuffer buffer);
        public void RetractValueFrom(ref ComputeBuffer target);
    }
    public interface IComputeBufferableSet<T>
    {

        public void PushValueIn(ref ComputeBuffer target);
    }


    public abstract class Abstract1DArrayOfValueGet<T> : IArray2DSize, IIsStoreAs1DArray<T>, IArrayClassic2DGet<T>, IArray2DWithDirectionGet<T>,
         IIndexToPercentHorizontal, IIndexToPercentVertical, IPercentToIndexHorizontal, IPercentToIndexVertical, IComputeBufferableGet<T>
    {
        [SerializeField] protected readonly int m_width;
        [SerializeField] protected readonly int m_height;
        [SerializeField] protected readonly int m_1DLenght;
        [SerializeField] protected readonly T[] m_valueArray;
        [SerializeField] protected readonly int[,] m_valueArrayIndex;

        protected Abstract1DArrayOfValueGet(int width, int height, T[] valueArray = null)
        {
            m_width = width;
            m_height = height;
            if (valueArray == null)
                m_valueArray = new T[m_width * m_height];
            else
                m_valueArray = valueArray;
            m_1DLenght = m_width * m_height;
            for (int x = 0; x < m_width; x++)
            {
                for (int y = 0; y < m_height; y++)
                {
                    m_valueArrayIndex[x, y] = y * width + x;
                }
            }
        }

        public void GetArray(out T[] array) => array = m_valueArray;
        public void GetClassic1D(in int index1D, out T value) => value = m_valueArray[index1D];
        public void GetClassic2D(in int x, in int y, out T value) => value = m_valueArray[Get1DIndexOf2D(in y, in x)];
        public int Get1DIndexOf2D(in int x, in int y) { return m_valueArrayIndex[x, y]; }
        public void GetHeight(out int height) => height = m_height;
        public void GetWidth(out int width) => width = m_width;

        public void GetXLeftRightPercent(in float percent, out int index) => throw new System.NotImplementedException();
        public void GetXRightLeftPercent(in float percent, out int index) => throw new System.NotImplementedException();
        public void GetYTopDownPercent(in float percent, out int index) => throw new System.NotImplementedException();
        public void GetYDownTopPercent(in float percent, out int index) => throw new System.NotImplementedException();
        public void Get1DLenght(out int lenght) => lenght = m_width;

        public void GetXY_LRTD(in int x, in int y, out T value)
        {
            GetXY_LRTDIndex1D(in x, in y, out int index);
            GetClassic1D(in index, out value);
        }

        public void GetXY_LRDT(in int x, in int y, out T value)
        {
            GetXY_LRDTIndex1D(in x, in y, out int index);
            GetClassic1D(in index, out value);
        }

        public void GetXY_RLTD(in int x, in int y, out T value)
        {
            GetXY_RLTDIndex1D(in x, in y, out int index);
            GetClassic1D(in index, out value);
        }

        public void GetXY_RLDT(in int x, in int y, out T value)
        {
            GetXY_RLDTIndex1D(in x, in y, out int index);
            GetClassic1D(in index, out value);
        }

        public void GetXY_LRTDIndex1D(in int x, in int y, out int index1D)
        {
            index1D = Get1DIndexOf2D(in x, in y);
        }

        public void GetXY_LRDTIndex1D(in int x, in int y, out int index1D)
        {
            index1D = Get1DIndexOf2D(in x, m_height - 1 - y);
        }

        public void GetXY_RLTDIndex1D(in int x, in int y, out int index1D)
        {
            index1D = Get1DIndexOf2D(m_width - 1 - x, in y);
        }

        public void GetXY_RLDTIndex1D(in int x, in int y, out int index1D)
        {
            index1D = Get1DIndexOf2D(m_width - 1 - x, m_height - 1 - y);
        }

        public void CreateComputeBuffer(out ComputeBuffer buffer)
        {
            buffer = new ComputeBuffer(m_1DLenght, Marshal.SizeOf<T>(), ComputeBufferType.Default);
        }
      

        public void RetractValueFrom(ref ComputeBuffer target)
        {
            target.GetData(m_valueArray);
        }
    }
    public class Abstract1DArrayOfValueGetSet<T> : Abstract1DArrayOfValueGet<T>, IArrayClassicSet<T>, IArray2DWithDirectionSet<T>, IComputeBufferableSet<T>
    {
        public Abstract1DArrayOfValueGetSet(int width, int height, T[] valueArray = null) : base(width, height, valueArray)
        {
        }
        public void SetClassic1D(in int index1D, T value)
        {
            m_valueArray[index1D] = value;
        }

        public void SetClassic2D(in int x, in int y, T value)
        {
            m_valueArray[Get1DIndexOf2D(in x, in y)] = value;
        }

        public void SetXY_LRDT(in int x, in int y, T value)
        {
            GetXY_LRDTIndex1D(in x, in y, out int index);
            SetClassic1D(in index, value);
        }

        public void SetXY_LRTD(in int x, in int y, T value)
        {
            GetXY_LRTDIndex1D(in x, in y, out int index);
            SetClassic1D(in index, value);
        }

        public void SetXY_RLDT(in int x, in int y, T value)
        {
            GetXY_RLDTIndex1D(in x, in y, out int index);
            SetClassic1D(in index, value);
        }

        public void SetXY_RLTD(in int x, in int y, T value)
        {
            GetXY_RLTDIndex1D(in x, in y, out int index);
            SetClassic1D(in index, value);
        }
        public void PushValueIn(ref ComputeBuffer target)
        {
            target.SetData(m_valueArray);
        }
    }

}