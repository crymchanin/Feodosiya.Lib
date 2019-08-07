using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Feodosiya.Lib.InteropServices {
    /// <summary>
    /// Класс предоставлющий методы Win32 API
    /// </summary>
    public static class Win32ApiHelper {

        internal const string user32 = "user32.dll";
        internal const string kernel32 = "kernel32.dll";
        internal const string advapi32 = "advapi32.dll";

        /// <summary>
        /// Приложение отправляет сообщение WM_SETREDRAW окну для того, чтобы дать возможность изменениям в этом окне быть перерисованными или воспрепятствовать изменениям в этом окне быть перерисованными
        /// </summary>
        public const int WM_SETREDRAW = 11;

        /// <summary>
        /// Скрывает окно
        /// </summary>
        [Obsolete("Это поле будет удалено")]
        public const int SW_HIDE = 0;
        /// <summary>
        /// Показывает окно
        /// </summary>
        [Obsolete("Это поле будет удалено")]
        public const int SW_SHOW = 5;

        private const int ECM_FIRST = 0x1500;
        /// <summary>
        /// Устанавливает watermark элемента управления
        /// </summary>
        public const int EM_SETCUEBANNER = ECM_FIRST + 1;

        /// <summary>
        /// Сообщение WM_NCLBUTTONDOWN посылается тогда, если пользователь нажимает левую кнопку мыши, в то время, когда курсор находится в нерабочей области окна
        /// </summary>
        public const int WM_NCLBUTTONDOWN = 0xA1;

        /// <summary>
        /// Этот сигнал передается в окно для того, чтобы определить, какая часть окна соответствует определенному экрану координат
        /// </summary>
        public const int HT_CAPTION = 0x2;

        /// <summary>
        /// Нулевой дескриптор
        /// </summary>
        public static IntPtr Nullhandle = IntPtr.Zero;

        /// <summary>
        /// Отправляет заданное сообщение окну или окнам
        /// </summary>
        /// <param name="hWnd">Дескриптор окна</param>
        /// <param name="wMsg">Отправляемое сообщение</param>
        /// <param name="wParam">Определяет дополнительную конкретизирующую сообщение информацию</param>
        /// <param name="lParam">Определяет дополнительную конкретизирующую сообщение информацию</param>
        /// <returns>Результат обработки сообщения</returns>
        [DllImport(user32)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 wMsg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Отправляет заданное сообщение окну или окнам
        /// </summary>
        /// <param name="hWnd">Дескриптор окна</param>
        /// <param name="wMsg">Отправляемое сообщение</param>
        /// <param name="wParam">Определяет дополнительную конкретизирующую сообщение информацию</param>
        /// <param name="lParam">Определяет дополнительную конкретизирующую сообщение информацию</param>
        /// <returns>Результат обработки сообщения</returns>
        [DllImport(user32)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 wMsg, Int32 wParam, Int32 lParam);

        /// <summary>
        /// Отправляет заданное сообщение окну или окнам
        /// </summary>
        /// <param name="hWnd">Дескриптор окна</param>
        /// <param name="wMsg">Отправляемое сообщение</param>
        /// <param name="wParam">Определяет дополнительную конкретизирующую сообщение информацию</param>
        /// <param name="lParam">Определяет дополнительную конкретизирующую сообщение информацию</param>
        /// <returns>Результат обработки сообщения</returns>
        [DllImport(user32)]
        public static extern IntPtr SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        /// <summary>
        /// Отправляет заданное сообщение окну или окнам
        /// </summary>
        /// <param name="hWnd">Дескриптор окна</param>
        /// <param name="wMsg">Отправляемое сообщение</param>
        /// <param name="wParam">Определяет дополнительную конкретизирующую сообщение информацию</param>
        /// <param name="lParam">Определяет дополнительную конкретизирующую сообщение информацию</param>
        /// <returns>Результат обработки сообщения</returns>
        [DllImport(user32)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 wMsg, IntPtr wParam, StringBuilder lParam);

        /// <summary>
        /// Отправляет заданное сообщение окну или окнам
        /// </summary>
        /// <param name="hWnd">Дескриптор окна</param>
        /// <param name="wMsg">Отправляемое сообщение</param>
        /// <param name="wParam">Определяет дополнительную конкретизирующую сообщение информацию</param>
        /// <param name="lParam">Определяет дополнительную конкретизирующую сообщение информацию</param>
        /// <returns>Результат обработки сообщения</returns>
        [DllImport(user32)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 wMsg, IntPtr wParam, [MarshalAs(UnmanagedType.LPStr)] string lParam);

        /// <summary>
        /// Отправляет заданное сообщение окну или окнам. Версия для UNICODE
        /// </summary>
        /// <param name="hWnd">Дескриптор окна</param>
        /// <param name="wMsg">Отправляемое сообщение</param>
        /// <param name="wParam">Определяет дополнительную конкретизирующую сообщение информацию</param>
        /// <param name="lParam">Определяет дополнительную конкретизирующую сообщение информацию</param>
        /// <returns>Результат обработки сообщения</returns>
        [DllImport(user32)]
        public static extern IntPtr SendMessageW(IntPtr hWnd, UInt32 wMsg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        /// <summary>
        /// Завершает фиксацию мыши для указанного окна
        /// </summary>
        /// <returns></returns>
        [DllImport(user32)]
        public static extern bool ReleaseCapture();

        /// <summary>
        /// Получает дескриптор консольного окна в связанном процессе
        /// </summary>
        /// <returns></returns>
        [DllImport(kernel32)]
        public static extern IntPtr GetConsoleWindow();

        /// <summary>
        /// Устанавливает состояние отображения заданного окна окна
        /// </summary>
        /// <param name="hWnd">Дескриптор окна</param>
        /// <param name="nCmdShow">Параметр отображения окна</param>
        /// <returns></returns>
        [DllImport(user32)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// Переключает фокус на указанное окно и переносит ено на передний план
        /// </summary>
        /// <param name="hWnd">Дескриптор окна</param>
        /// <param name="fAltTab">Переключаться посредством комбинации клавиш Alt+Tab, если значение этого параметра установлено в true</param>
        [DllImport(user32, SetLastError = true)]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        /// <summary>
        /// Возвращает строку из указанной секции INI файла
        /// </summary>
        /// <param name="lpAppName">Название раздела содержащего имя ключа</param>
        /// <param name="lpKeyName">Имя ключа связанного с возвращаемой строкой</param>
        /// <param name="lpDefault">Строка по умолчанию</param>
        /// <param name="lpReturnedString">Буфер в который будет помещена получаемая строка</param>
        /// <param name="nSize">Размер буфера lpReturnedString</param>
        /// <param name="lpFileName">Путь к INI файлу</param>
        /// <returns>Количество символов в полученной строке</returns>
        [DllImport(kernel32)]
        public static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        /// <summary>
        /// Копирует строку в указанную секцию INI файла
        /// </summary>
        /// <param name="lpAppName">Имя раздела в который будет скопирована устанавливаемая строка</param>
        /// <param name="lpKeyName">Имя ключа связанного с устанавливаемой строкой</param>
        /// <param name="lpString">Устанавливаемое строковое значение</param>
        /// <param name="lpFileName">Путь к INI файлу</param>
        /// <returns>При успешном выполнении, возвращает true</returns>
        [DllImport(kernel32, CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        /// <summary>
        /// Logs off the interactive user, shuts down the system, or shuts down and restarts the system. It sends the WM_QUERYENDSESSION message to all applications to determine if they can be terminated
        /// </summary>
        /// <param name="uFlags">The shutdown type</param>
        /// <param name="dwReason">The reason for initiating the shutdown. This parameter must be one of the system shutdown reason codes</param>
        /// <returns>If the function succeeds, the return value is nonzero</returns>
        [DllImport(user32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        /// <summary>
        /// The OpenProcessToken function opens the access token associated with a process
        /// </summary>
        /// <param name="ProcessHandle">A handle to the process whose access token is opened. The process must have the PROCESS_QUERY_INFORMATION access permission</param>
        /// <param name="DesiredAccess">Specifies an access mask that specifies the requested types of access to the access token. These requested access types are compared with the discretionary access control list (DACL) of the token to determine which accesses are granted or denied</param>
        /// <param name="TokenHandle">A pointer to a handle that identifies the newly opened access token when the function returns</param>
        /// <returns>If the function succeeds, the return value is nonzero</returns>
        [DllImport(advapi32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool OpenProcessToken(IntPtr ProcessHandle, UInt32 DesiredAccess, out IntPtr TokenHandle);

        /// <summary>
        /// An LUID is a 64-bit value guaranteed to be unique only on the system on which it was generated. The uniqueness of a locally unique identifier (LUID) is guaranteed only until the system is restarted
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct LUID {
            /// <summary>
            /// Low-order bits
            /// </summary>
            public uint LowPart;
            /// <summary>
            /// High-order bits
            /// </summary>
            public int HighPart;
        }

        /// <summary>
        /// The LookupPrivilegeValue function retrieves the locally unique identifier (LUID) used on a specified system to locally represent the specified privilege name
        /// </summary>
        /// <param name="lpSystemName">A pointer to a null-terminated string that specifies the name of the system on which the privilege name is retrieved. If a null string is specified, the function attempts to find the privilege name on the local system</param>
        /// <param name="lpName">A pointer to a null-terminated string that specifies the name of the privilege, as defined in the Winnt.h header file. For example, this parameter could specify the constant, SE_SECURITY_NAME, or its corresponding string, "SeSecurityPrivilege"</param>
        /// <param name="lpLuid">A pointer to a variable that receives the LUID by which the privilege is known on the system specified by the lpSystemName parameter</param>
        /// <returns></returns>
        [DllImport(advapi32, SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, out LUID lpLuid);

        /// <summary>
        /// Closes an open object handle
        /// </summary>
        /// <param name="hObject">A valid handle to an open object</param>
        /// <returns></returns>
        [DllImport(kernel32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        /// <summary>
        /// The LUID_AND_ATTRIBUTES structure represents a locally unique identifier (LUID) and its attributes
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct LUID_AND_ATTRIBUTES {
            /// <summary>
            /// Specifies an LUID value
            /// </summary>
            public LUID Luid;
            /// <summary>
            /// Specifies attributes of the LUID. This value contains up to 32 one-bit flags. Its meaning is dependent on the definition and use of the LUID
            /// </summary>
            public UInt32 Attributes;
        }

        /// <summary>
        /// The TOKEN_PRIVILEGES structure contains information about a set of privileges for an access token
        /// </summary>
        public struct TOKEN_PRIVILEGES {
            /// <summary>
            /// This must be set to the number of entries in the Privileges array
            /// </summary>
            public UInt32 PrivilegeCount;
            /// <summary>
            /// Specifies an array of LUID_AND_ATTRIBUTES structures. Each structure contains the LUID and attributes of a privilege. To get the name of the privilege associated with a LUID, call the LookupPrivilegeName function, passing the address of the LUID as the value of the lpLuid parameter
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public LUID_AND_ATTRIBUTES[] Privileges;
        }

        /// <summary>
        /// The AdjustTokenPrivileges function enables or disables privileges in the specified access token. Enabling or disabling privileges in an access token requires TOKEN_ADJUST_PRIVILEGES access
        /// </summary>
        /// <param name="TokenHandle">A handle to the access token that contains the privileges to be modified. The handle must have TOKEN_ADJUST_PRIVILEGES access to the token. If the PreviousState parameter is not NULL, the handle must also have TOKEN_QUERY access</param>
        /// <param name="DisableAllPrivileges">Specifies whether the function disables all of the token's privileges. If this value is TRUE, the function disables all privileges and ignores the NewState parameter. If it is FALSE, the function modifies privileges based on the information pointed to by the NewState parameter</param>
        /// <param name="NewState">A pointer to a TOKEN_PRIVILEGES structure that specifies an array of privileges and their attributes. If the DisableAllPrivileges parameter is FALSE, the AdjustTokenPrivileges function enables, disables, or removes these privileges for the token. The following table describes the action taken by the AdjustTokenPrivileges function, based on the privilege attribute</param>
        /// <param name="BufferLength">Specifies the size, in bytes, of the buffer pointed to by the PreviousState parameter. This parameter can be zero if the PreviousState parameter is NULL</param>
        /// <param name="PreviousState">A pointer to a buffer that the function fills with a TOKEN_PRIVILEGES structure that contains the previous state of any privileges that the function modifies. That is, if a privilege has been modified by this function, the privilege and its previous state are contained in the TOKEN_PRIVILEGES structure referenced by PreviousState. If the PrivilegeCount member of TOKEN_PRIVILEGES is zero, then no privileges have been changed by this function. This parameter can be NULL</param>
        /// <param name="ReturnLength">A pointer to a variable that receives the required size, in bytes, of the buffer pointed to by the PreviousState parameter. This parameter can be NULL if PreviousState is NULL</param>
        /// <returns></returns>
        [DllImport(advapi32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AdjustTokenPrivileges(IntPtr TokenHandle,
            [MarshalAs(UnmanagedType.Bool)]bool DisableAllPrivileges,
            ref TOKEN_PRIVILEGES NewState,
            UInt32 BufferLength,
            IntPtr PreviousState,
            IntPtr ReturnLength);

        /// <summary>
        /// System Shutdown Reason Codes
        /// </summary>
        [Flags]
        public enum ShutdownReasonCodes : uint {
            /// <summary>
            /// Application issue
            /// </summary>
            SHTDN_REASON_MAJOR_APPLICATION = 0x00040000,
            /// <summary>
            /// Hardware issue
            /// </summary>
            SHTDN_REASON_MAJOR_HARDWARE = 0x00010000,
            /// <summary>
            /// The InitiateSystemShutdown function was used instead of InitiateSystemShutdownEx.
            /// </summary>
            SHTDN_REASON_MAJOR_LEGACY_API = 0x00070000,
            /// <summary>
            /// Operating system issue
            /// </summary>
            SHTDN_REASON_MAJOR_OPERATINGSYSTEM = 0x00020000,
            /// <summary>
            /// Other issue
            /// </summary>
            SHTDN_REASON_MAJOR_OTHER = 0x00000000,
            /// <summary>
            /// Power failure
            /// </summary>
            SHTDN_REASON_MAJOR_POWER = 0x00060000,
            /// <summary>
            /// Software issue
            /// </summary>
            SHTDN_REASON_MAJOR_SOFTWARE = 0x00030000,
            /// <summary>
            /// System failure
            /// </summary>
            SHTDN_REASON_MAJOR_SYSTEM = 0x00050000,

            /// <summary>
            /// Blue screen crash event
            /// </summary>
            SHTDN_REASON_MINOR_BLUESCREEN = 0x0000000F,
            /// <summary>
            /// Unplugged
            /// </summary>
            SHTDN_REASON_MINOR_CORDUNPLUGGED = 0x0000000b,
            /// <summary>
            /// Disk
            /// </summary>
            SHTDN_REASON_MINOR_DISK = 0x00000007,
            /// <summary>
            /// Environment
            /// </summary>
            SHTDN_REASON_MINOR_ENVIRONMENT = 0x0000000c,
            /// <summary>
            /// Driver
            /// </summary>
            SHTDN_REASON_MINOR_HARDWARE_DRIVER = 0x0000000d,
            /// <summary>
            /// Hot fix
            /// </summary>
            SHTDN_REASON_MINOR_HOTFIX = 0x00000011,
            /// <summary>
            /// Hot fix uninstallation
            /// </summary>
            SHTDN_REASON_MINOR_HOTFIX_UNINSTALL = 0x00000017,
            /// <summary>
            /// Unresponsive
            /// </summary>
            SHTDN_REASON_MINOR_HUNG = 0x00000005,
            /// <summary>
            /// Installation
            /// </summary>
            SHTDN_REASON_MINOR_INSTALLATION = 0x00000002,
            /// <summary>
            /// Maintenance
            /// </summary>
            SHTDN_REASON_MINOR_MAINTENANCE = 0x00000001,
            /// <summary>
            /// MMC issue
            /// </summary>
            SHTDN_REASON_MINOR_MMC = 0x00000019,
            /// <summary>
            /// Network connectivity
            /// </summary>
            SHTDN_REASON_MINOR_NETWORK_CONNECTIVITY = 0x00000014,
            /// <summary>
            /// Network card
            /// </summary>
            SHTDN_REASON_MINOR_NETWORKCARD = 0x00000009,
            /// <summary>
            /// Other issue
            /// </summary>
            SHTDN_REASON_MINOR_OTHER = 0x00000000,
            /// <summary>
            /// Other driver event
            /// </summary>
            SHTDN_REASON_MINOR_OTHERDRIVER = 0x0000000e,
            /// <summary>
            /// Power supply
            /// </summary>
            SHTDN_REASON_MINOR_POWER_SUPPLY = 0x0000000a,
            /// <summary>
            /// Processor
            /// </summary>
            SHTDN_REASON_MINOR_PROCESSOR = 0x00000008,
            /// <summary>
            /// Reconfigure
            /// </summary>
            SHTDN_REASON_MINOR_RECONFIG = 0x00000004,
            /// <summary>
            /// Security issue
            /// </summary>
            SHTDN_REASON_MINOR_SECURITY = 0x00000013,
            /// <summary>
            /// Security patch
            /// </summary>
            SHTDN_REASON_MINOR_SECURITYFIX = 0x00000012,
            /// <summary>
            /// Security patch uninstallation
            /// </summary>
            SHTDN_REASON_MINOR_SECURITYFIX_UNINSTALL = 0x00000018,
            /// <summary>
            /// Service pack
            /// </summary>
            SHTDN_REASON_MINOR_SERVICEPACK = 0x00000010,
            /// <summary>
            /// Service pack uninstallation
            /// </summary>
            SHTDN_REASON_MINOR_SERVICEPACK_UNINSTALL = 0x00000016,
            /// <summary>
            /// Terminal Services
            /// </summary>
            SHTDN_REASON_MINOR_TERMSRV = 0x00000020,
            /// <summary>
            /// Unstable
            /// </summary>
            SHTDN_REASON_MINOR_UNSTABLE = 0x00000006,
            /// <summary>
            /// Upgrade
            /// </summary>
            SHTDN_REASON_MINOR_UPGRADE = 0x00000003,
            /// <summary>
            /// WMI issue
            /// </summary>
            SHTDN_REASON_MINOR_WMI = 0x00000015,

            /// <summary>
            /// The reason code is defined by the user. For more information, see Defining a Custom Reason Code.
            /// If this flag is not present, the reason code is defined by the system
            /// </summary>
            SHTDN_REASON_FLAG_USER_DEFINED = 0x40000000,
            /// <summary>
            /// The shutdown was planned. The system generates a System State Data (SSD) file. This file contains system state information such as the processes, threads, memory usage, and configuration. 
            /// If this flag is not present, the shutdown was unplanned. Notification and reporting options are controlled by a set of policies. For example, after logging in, the system displays a dialog box reporting the unplanned shutdown if the policy has been enabled. An SSD file is created only if the SSD policy is enabled on the system. The administrator can use Windows Error Reporting to send the SSD data to a central location, or to Microsoft.
            /// </summary>
            SHTDN_REASON_FLAG_PLANNED = 0x80000000
        }
    }
}
