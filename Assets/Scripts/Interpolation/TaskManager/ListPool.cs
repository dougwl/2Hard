using System;
using System.Collections.Generic;
using UnityEngine;

public static class ListPool<T>{

    private static readonly ObjectPool<List<T>> s_ListPool = new ObjectPool<List<T>>();

    public static List<T> Get()
    {
        return s_ListPool.Get();
    }

    public static void Release(List<T> toRelease)
    {
        if(toRelease.Count > 0 && toRelease[0] is IFlushable)
        {
            for (int i = 0; i < toRelease.Count; i++)
            {
                IFlushable obj = (IFlushable)toRelease[i];
                (obj).Flush();
            }
        }
        toRelease.Clear();
        s_ListPool.Release(toRelease);
    }
    public static void ReleaseNoFlush(List<T> toRelease)
    {
        toRelease.Clear();
        s_ListPool.Release(toRelease);
    }
}

internal static class ObjPool<T> where T : class, IFlushable, new()
{
    private static readonly ObjectPool<T> s_ObjPool = new ObjectPool<T>();

    public static T Get()
    {
        T obj = s_ObjPool.Get();
        obj.SetFlushed(false);
        return obj;
    }

    public static void Release(T toRelease)
    {
        if(toRelease.GetFlushed())
            return;
        toRelease.SetFlushed(true);
        s_ObjPool.Release(toRelease);
    }
}

internal interface IFlushable
{
    bool GetFlushed();

    void SetFlushed(bool flushed);

    void Flush();
}
