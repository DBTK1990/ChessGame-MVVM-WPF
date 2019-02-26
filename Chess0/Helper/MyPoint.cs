﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess0.Helper
{

    public enum State { White = 1, Black = 2 }

    public class MyPoint
        {
            public double X { get; set; }
            public double Y { get; set; }


            public MyPoint(double x, double y)
            {
                X = x;
                Y = y;
            }


            public static MyPoint operator *(MyPoint a, double b)
            {
                return new MyPoint(a.X * b, a.Y * b);
            }


            public static MyPoint operator *(MyPoint a, MyPoint b)
            {
            return new MyPoint(a.X * b.X, a.Y * b.Y);
            }

        public static MyPoint operator +(MyPoint a, MyPoint b)
            {
                return new MyPoint(a.X + b.X, a.Y + b.Y);
            }

        public static bool operator ==(MyPoint a, MyPoint b)
        {
          
            return a.X == b.X && a.Y == b.Y ?true:false;

        }
        public static bool operator !=(MyPoint a, MyPoint b)
        {
            
            return a.X != b.X || a.Y != b.Y ? true : false;

        }
    }
    
}