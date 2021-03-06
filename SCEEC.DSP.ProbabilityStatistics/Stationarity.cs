﻿using System;
using System.Collections.Generic;

namespace SCEEC.DSP.ProbabilityStatistics
{
    public class Stationarity
    {
        private readonly int minLength;
        private readonly int maxLength;
        private readonly double[] momentsThreshold;
        private List<double> data;
        private List<double> moments;
        /// <summary>
        /// 创建平稳性分析类
        /// </summary>
        /// <param name="minLength">最小分析长度</param>
        /// <param name="unitLength">分析最大长度</param>
        /// <param name="momentsThreshold">矩上限</param>
        public Stationarity(int minLength, int unitLength, double[] momentsThreshold)
        {
            if (minLength < 3) throw new ArgumentException();
            if (unitLength < 3) throw new ArgumentException();
            if (momentsThreshold.Length < 1) throw new ArgumentException();
            this.maxLength = unitLength;
            this.minLength = minLength;
            this.momentsThreshold = momentsThreshold;
            this.data = new List<double>();
        }

        public void Insert(double value)
        {
            data.Add(value);
            if (data.Count > (this.maxLength))
                data.RemoveAt(0);
        }

        public bool IsStable
        {
            get
            {
                if (data.Count < minLength) return false;
                double[] xn = data.ToArray();
                moments = new List<double>();
                int i;
                for (i = 0; i < momentsThreshold.Length; i++)
                {
                    double m = 0.0;
                    double ax = (i != 0) ? moments[i - 1] : 0;
                    foreach (double x in xn)
                    {
                        m += Math.Pow(x - ax, i + 1);
                    }
                    m /= xn.Length;
                    m = Math.Pow(Math.Abs(m), 1.0 / (i + 1));
                    moments.Add(m);
                    if (i > 1)
                    {
                        if (m < momentsThreshold[i] * moments[0] * (1 - momentsThreshold[0])) return false;
                        if (m > momentsThreshold[i] * moments[0] * (1 + momentsThreshold[0])) return false;
                    }
                    //if ((i != 0) && (m > ax)) return false;
                }
                return true;
            }
        }
    }
}
