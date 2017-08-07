using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Recursion
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Add02(5));
            Console.ReadLine();
        }

        //数列1,1,2,3,5,8,13...第n位数是多少?
        public static int Add(int n)
        {
            if (n == 1 || n == 2) return 1;
            else return Add(n-2)+Add(n-1);
        }

        public static int Add02(int n)
        {
            int[] intArray = new int[n];//声明一个数组存储这个数列
            for (int i = 0; i < intArray.Length; i++)//用for循环遍历数列中每个数
            {
                if (i == 0 || i == 1) intArray[i] = 1;
                else
                    intArray[i] = intArray[i - 1] + intArray[i - 2];            
            }
            return intArray[n-1];//返回数列中最后一个元素
        }
    }
}
