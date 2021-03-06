﻿using System;
using System.Windows;

namespace SCEEC.TTM
{
    public static class ErrorReporter
    {
        private static string errorText(int errorCode)
        {
            string errorStr = "Error Code: " + errorCode.ToString();
            switch(errorCode % 10000)
            {
                case 1:
                    errorStr += "\n用户数据库加载错误。";
                    break;
                case 2:
                    errorStr += "\n无法连接本地数据库。";
                    break;
                case 3:
                    errorStr += "\n无法更改本地数据库。";
                    break;
                case 4:
                    errorStr += "\n无法获取数据表。";
                    break;
                case 5:
                    errorStr += "\n无法打开串口。";
                    break;
                default:
                    break;
            }
            return errorStr;
        }
        public static void ErrorReport(int errorCode, string ownerName, string detail)
        {
            MessageBox.Show(errorText(errorCode) + Environment.NewLine + detail, ownerName, MessageBoxButton.OK, MessageBoxImage.Error);
#if DEBUG
            throw new Exception();
#else
            Environment.Exit(errorCode);
#endif
        }
    }
}
