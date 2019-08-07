using System;
using Feodosiya.Lib.InteropServices;
using System.ComponentModel;
using System.Runtime.InteropServices;


namespace Feodosiya.Lib.OS {
    /// <summary>
    /// Класс предоставляющий методы для работы с системой
    /// </summary>
    public static class SystemHelper {

        private const UInt32 TOKEN_QUERY = 0x0008;
        private const UInt32 TOKEN_ADJUST_PRIVILEGES = 0x0020;
        private const UInt32 SE_PRIVILEGE_ENABLED = 0x00000002;
        private const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";

        /// <summary>
        /// Выполняет выход из сеанса пользователя, зевершение работы системы или её перезагрузку
        /// </summary>
        /// <param name="procHandle">Дескриптор приложения выполняющего вызов данного метода</param>
        /// <param name="shutdownFlag">Вариант завершения работы</param>
        public static void ExitWindows(IntPtr procHandle, uint shutdownFlag) {
            IntPtr tokenHandle = IntPtr.Zero;

            try {
                if (!Win32ApiHelper.OpenProcessToken(procHandle, TOKEN_QUERY | TOKEN_ADJUST_PRIVILEGES, out tokenHandle)) {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "Не удалось открыть дескриптор токена процесса");
                }

                Win32ApiHelper.TOKEN_PRIVILEGES tokenPrivs = new Win32ApiHelper.TOKEN_PRIVILEGES();
                tokenPrivs.PrivilegeCount = 1;
                tokenPrivs.Privileges = new Win32ApiHelper.LUID_AND_ATTRIBUTES[1];
                tokenPrivs.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;

                if (!Win32ApiHelper.LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, out tokenPrivs.Privileges[0].Luid)) {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "Не удалось преобразовать имя привилегии выключения");
                }

                if (!Win32ApiHelper.AdjustTokenPrivileges(tokenHandle, false, ref tokenPrivs, 0, IntPtr.Zero, IntPtr.Zero)) {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "Не удалось настроить права токена процесса");
                }

                if (!Win32ApiHelper.ExitWindowsEx(shutdownFlag,
                        (uint)Win32ApiHelper.ShutdownReasonCodes.SHTDN_REASON_MAJOR_APPLICATION |
                        (uint)Win32ApiHelper.ShutdownReasonCodes.SHTDN_REASON_MINOR_INSTALLATION |
                        (uint)Win32ApiHelper.ShutdownReasonCodes.SHTDN_REASON_FLAG_PLANNED)) {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "Ошибка при выключении системы");
                }
            }
            finally {
                if (tokenHandle != IntPtr.Zero) {
                    Win32ApiHelper.CloseHandle(tokenHandle);
                }
            }
        }
    }
}
