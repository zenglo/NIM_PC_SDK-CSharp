using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NIM
{
    delegate int ExceptionFilter(ref long code);
    public class NimUnhandledExceptionFilter
    {
        [DllImport("kernel32")]
        private static extern int SetUnhandledExceptionFilter(ExceptionFilter filter);

        public static void RegisterFilter()
        {
            //SetUnhandledExceptionFilter(ProcessException);
        }

        static int ProcessException(ref long code)
        {
            NimUtility.NimLogManager.NimCoreLog.InfoFormat("NimUnhandledExceptionFilter  {0}", code);
            return 1;
        } 
    }
}
