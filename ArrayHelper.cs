using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public delegate SKey Handler<SKey>(Student s);
/// <summary>
/// 选择委托：从某个类型选择某个属性 返回这个属性的值
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
/// <typeparam name="TKey">属性的类型</typeparam>
/// <param name="t">数据类型的对象</param>
/// <returns> 返回属性的值  张三年龄 20</returns>
public delegate TKey SelectHandler<T,TKey>(T t);

/// <summary>
/// 查找委托：表示查找条件 如 bool b=obj.age==20&&obj.tall>160
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
/// <param name="t">数据类型的对象</param>
/// <returns>bool值：true，false</returns>
public delegate bool FindHandler<T>(T t);

/// <summary>
/// 数组助手类 
/// </summary>
public class ArrayHelper
{
    public static void OrderBy1(int[] array)
    {
        //冒泡法
        for (int i = 0; i < array.Length-1; i++)
        {
            for (int j = i+1; j < array.Length; j++)
            {
                if (array[i].CompareTo(array[j]) > 0)
                {
                    int temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }
        }
    
    }

    public static void OrderBy2(string[] array)
    {
        //冒泡法
        for (int i = 0; i < array.Length - 1; i++)
        {
            for (int j = i + 1; j < array.Length; j++)
            {
                if (array[i].CompareTo(array[j]) > 0)
                {
                    var temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }
        }

    }

    //public static void OrderBy3(Student[] array)
    //{
    //    //冒泡法
    //    for (int i = 0; i < array.Length - 1; i++)
    //    {
    //        for (int j = i + 1; j < array.Length; j++)
    //        {
    //            if (array[i].CompareTo(array[j]) > 0)
    //            {
    //                var temp = array[i];
    //                array[i] = array[j];
    //                array[j] = temp;
    //            }
    //        }
    //    }
    //}

    //如果实现比较接口，代码的膨胀！[复习比较接口练习]
    //引入委托：让委托指定比较哪个属性，让调用端确定=灵活性，复用性
    public static void OrderBy4(Student[] array)
    {
        //冒泡法
        for (int i = 0; i < array.Length - 1; i++)
        {
            for (int j = i + 1; j < array.Length; j++)
            {
                if (array[i].Id.CompareTo(array[j].Id) > 0)//Age
                {
                    var temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }
        }
    }
    public static void OrderBy44<SKey>(Student[] array, 
        Handler<SKey> handler)
        where SKey : IComparable<SKey>
    {
        //冒泡法
        for (int i = 0; i < array.Length - 1; i++)
        {
            for (int j = i + 1; j < array.Length; j++)
            {
                //if (array[i].??.CompareTo(array[j].??) > 0)//
                //array[i].??==handler 返回某个对象的某个属性的值
                //handler(array[i])
                //if (array[i].Tall.CompareTo(array[j].Tall) > 0)//
                if (handler(array[i]).CompareTo(handler(array[j])) > 0)//
                {
                    var temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }
        }
    }
    /// <summary>
    /// 1 升序排序
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <typeparam name="TKey">属性的类型</typeparam>
    /// <param name="array">数据类型的对象数组</param>
    /// <param name="handler">选择委托</param>
    public static void OrderBy<T,TKey>(T[] array,
       SelectHandler<T, TKey> handler)
       where TKey : IComparable<TKey>
    {
        //冒泡法
        for (int i = 0; i < array.Length - 1; i++)
        {
            for (int j = i + 1; j < array.Length; j++)
            {               
                if (handler(array[i]).CompareTo(handler(array[j])) > 0)
                {
                    var temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }
        }
    }
    /// <summary>
    /// 2 降序排序
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <typeparam name="TKey">属性的类型</typeparam>
    /// <param name="array">数据类型的对象数组</param>
    /// <param name="handler">选择委托</param>
    public static void OrderByDescending<T, TKey>(T[] array,
       SelectHandler<T, TKey> handler)
       where TKey : IComparable<TKey>
    {
        //冒泡法
        for (int i = 0; i < array.Length - 1; i++)
        {
            for (int j = i + 1; j < array.Length; j++)
            {
                if (handler(array[i]).CompareTo(handler(array[j])) < 0)
                {
                    var temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }
        }
    }

    /// <summary>
    /// 3 最大
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <typeparam name="TKey">属性的类型</typeparam>
    /// <param name="array">数据类型的对象数组</param>
    /// <param name="handler">选择委托</param>
    /// <returns>返回最大的对象</returns>
    public static T Max<T, TKey>(T[] array,
       SelectHandler<T, TKey> handler)
       where TKey : IComparable<TKey>
    {
        T max = array[0];//
        for (int i = 1; i < array.Length; i++)
        {

            if (handler(max).CompareTo(handler(array[i])) < 0)
            {
                max = array[i];               
            }           
        }
        return max;
    }

    /// <summary>
    /// 4 最小
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <typeparam name="TKey">属性的类型</typeparam>
    /// <param name="array">数据类型的对象数组</param>
    /// <param name="handler">选择委托</param>
    /// <returns>返回最小的对象</returns>
    public static T Min<T, TKey>(T[] array,
       SelectHandler<T, TKey> handler)
       where TKey : IComparable<TKey>
    {
        T min = array[0];//
        for (int i = 1; i < array.Length; i++)
        {

            if (handler(min).CompareTo(handler(array[i])) > 0)
            {
                min = array[i];
            }
        }
        return min;
    }

    /// <summary>
    /// 5 查找单个，给定一条件，返回满足条件的一个 复用性！
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="array">数据类型的对象数组</param>
    /// <param name="handler">查找委托 bool</param>
    /// <returns>满足条件的一个对象</returns>
    public static T Find<T>(T[] array, FindHandler<T> handler)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (handler(array[i]))
            {
                return array[i];
            }
        }
        return default(T);
    }
    /// <summary>
    ///  6 查找，给定一条件，返回满足条件的所有对象！
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    public static T[] FindAll<T>(T[] array, FindHandler<T> handler)
    {
        List<T> list = new List<T>();
        for (int i = 0; i < array.Length; i++)
        {
            if (handler(array[i]))
            {
                list.Add(array[i]);
            }
        }
        return list.ToArray();
    }
    /// <summary>
    /// 7 选择：选取数组中对象的某些成员形成一个独立的数组
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <typeparam name="TKey">数据类型的某个成员的类型</typeparam>
    /// <param name="array">数据类型对象数组</param>
    /// <param name="handler">选择委托</param>
    /// <returns>新的数组</returns>
    public static TKey[] Select<T,TKey>(T[] array,SelectHandler<T,TKey>
        handler) 
    {
        TKey[] arr = new TKey[array.Length];
        for (int i=0; i < array.Length; i++)
        {
           arr[i]=handler(array[i]); 
        }
        return arr;
    }
}

