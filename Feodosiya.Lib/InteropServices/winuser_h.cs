using System;

namespace Feodosiya.Lib.InteropServices {
    /// <summary>
    /// winuser.h
    /// </summary>
    public static class winuser_h {

        /// <summary>
        /// A window receives this message when the user chooses a command from the Window menu (formerly known as the system or control menu)
        /// or when the user chooses the maximize button, minimize button, restore button, or close button
        /// </summary>
        public const int WM_SYSCOMMAND = 0x0112;
        /// <summary>
        /// Closes the window
        /// </summary>
        public const int SC_CLOSE = 0xF060;
        /// <summary>
        /// Changes the cursor to a question mark with a pointer. If the user then clicks a control in the dialog box, the control receives a WM_HELP message
        /// </summary>
        public const int SC_CONTEXTHELP = 0xF180;
        /// <summary>
        /// Selects the default item; the user double-clicked the window menu
        /// </summary>
        public const int SC_DEFAULT = 0xF160;
        /// <summary>
        /// Activates the window associated with the application-specified hot key. The lParam parameter identifies the window to activate
        /// </summary>
        public const int SC_HOTKEY = 0xF150;
        /// <summary>
        /// Scrolls horizontally
        /// </summary>
        public const int SC_HSCROLL = 0xF080;
        /// <summary>
        /// Indicates whether the screen saver is secure
        /// </summary>
        public const int SCF_ISSECURE = 0x00000001;
        /// <summary>
        /// Retrieves the window menu as a result of a keystroke. For more information, see the Remarks section
        /// </summary>
        public const int SC_KEYMENU = 0xF100;
        /// <summary>
        /// Maximizes the window
        /// </summary>
        public const int SC_MAXIMIZE = 0xF030;
        /// <summary>
        /// Minimizes the window
        /// </summary>
        public const int SC_MINIMIZE = 0xF020;
        /// <summary>
        /// Sets the state of the display. This command supports devices that have power-saving features, such as a battery-powered personal computer. 
        /// The lParam parameter can have the following values:
        /// -1 (the display is powering on)
        /// 1 (the display is going to low power)
        /// 2 (the display is being shut off)
        /// </summary>
        public const int SC_MONITORPOWER = 0xF170;
        /// <summary>
        /// Retrieves the window menu as a result of a mouse click
        /// </summary>
        public const int SC_MOUSEMENU = 0xF090;
        /// <summary>
        /// Moves the window
        /// </summary>
        public const int SC_MOVE = 0xF010;
        /// <summary>
        /// Moves to the next window
        /// </summary>
        public const int SC_NEXTWINDOW = 0xF040;
        /// <summary>
        /// Moves to the previous window
        /// </summary>
        public const int SC_PREVWINDOW = 0xF050;
        /// <summary>
        /// Restores the window to its normal position and size
        /// </summary>
        public const int SC_RESTORE = 0xF120;
        /// <summary>
        /// Executes the screen saver application specified in the [boot] section of the System.ini file
        /// </summary>
        public const int SC_SCREENSAVE = 0xF140;
        /// <summary>
        /// Sizes the window
        /// </summary>
        public const int SC_SIZE = 0xF000;
        /// <summary>
        /// Activates the Start menu
        /// </summary>
        public const int SC_TASKLIST = 0xF130;
        /// <summary>
        /// Scrolls vertically
        /// </summary>
        public const int SC_VSCROLL = 0xF070;



        /// <summary>
        /// Minimizes a window, even if the thread that owns the window is not responding. This flag should only be used when minimizing windows from a different thread
        /// </summary>
        public const int SW_FORCEMINIMIZE = 11;
        /// <summary>
        /// Hides the window and activates another window
        /// </summary>
        public const int SW_HIDE = 0;
        /// <summary>
        /// Maximizes the specified window
        /// </summary>
        public const int SW_MAXIMIZE = 3;
        /// <summary>
        /// Minimizes the specified window and activates the next top-level window in the Z order
        /// </summary>
        public const int SW_MINIMIZE = 6;
        /// <summary>
        /// Activates and displays the window. If the window is minimized or maximized, the system restores it to its original size and position.
        /// An application should specify this flag when restoring a minimized window
        /// </summary>
        public const int SW_RESTORE = 9;
        /// <summary>
        /// Activates the window and displays it in its current size and position
        /// </summary>
        public const int SW_SHOW = 5;
        /// <summary>
        /// Sets the show state based on the SW_ value specified in the STARTUPINFO structure passed to the CreateProcess function by the program
        /// that started the application
        /// </summary>
        public const int SW_SHOWDEFAULT = 10;
        /// <summary>
        /// Activates the window and displays it as a maximized window
        /// </summary>
        public const int SW_SHOWMAXIMIZED = 3;
        /// <summary>
        /// Activates the window and displays it as a minimized window
        /// </summary>
        public const int SW_SHOWMINIMIZED = 2;
        /// <summary>
        /// Displays the window as a minimized window. This value is similar to SW_SHOWMINIMIZED, except the window is not activated
        /// </summary>
        public const int SW_SHOWMINNOACTIVE = 7;
        /// <summary>
        /// Displays the window in its current size and position. This value is similar to SW_SHOW, except that the window is not activated
        /// </summary>
        public const int SW_SHOWNA = 8;
        /// <summary>
        /// Displays a window in its most recent size and position. This value is similar to SW_SHOWNORMAL, except that the window is not activated
        /// </summary>
        public const int SW_SHOWNOACTIVATE = 4;
        /// <summary>
        /// Activates and displays a window. If the window is minimized or maximized, the system restores it to its original size and position.
        /// An application should specify this flag when displaying the window for the first time
        /// </summary>
        public const int SW_SHOWNORMAL = 1;

        // ExitWindowsEx flags
        //
        /// <summary>
        /// Beginning with Windows 8:  You can prepare the system for a faster startup by combining the EWX_HYBRID_SHUTDOWN flag with the EWX_SHUTDOWN flag
        /// </summary>
        public const uint EWX_HYBRID_SHUTDOWN = 0x00400000;
        /// <summary>
        /// Shuts down all processes running in the logon session of the process that called the ExitWindowsEx function. Then it logs the user off.
        /// This flag can be used only by processes running in an interactive user's logon session.
        /// </summary>
        public const uint EWX_LOGOFF = 0;
        /// <summary>
        /// Shuts down the system and turns off the power. The system must support the power-off feature.
        /// The calling process must have the SE_SHUTDOWN_NAME privilege. For more information, see the following Remarks section.
        /// </summary>
        public const uint EWX_POWEROFF = 0x00000008;
        /// <summary>
        /// Shuts down the system and then restarts the system.
        /// The calling process must have the SE_SHUTDOWN_NAME privilege. For more information, see the following Remarks section.
        /// </summary>
        public const uint EWX_REBOOT = 0x00000002;
        /// <summary>
        /// Shuts down the system and then restarts it, as well as any applications that have been registered for restart using the RegisterApplicationRestart function. These application receive the WM_QUERYENDSESSION message with lParam set to the ENDSESSION_CLOSEAPP value.
        /// </summary>
        public const uint EWX_RESTARTAPPS = 0x00000040;
        /// <summary>
        /// Shuts down the system to a point at which it is safe to turn off the power. All file buffers have been flushed to disk, and all running processes have stopped.
        /// The calling process must have the SE_SHUTDOWN_NAME privilege. For more information, see the following Remarks section.
        /// 
        /// Specifying this flag will not turn off the power even if the system supports the power-off feature. You must specify EWX_POWEROFF to do this.Windows XP with SP1:  If the system supports the power-off feature, specifying this flag turns off the power.
        /// </summary>
        public const uint EWX_SHUTDOWN = 0x00000001;

        // This parameter can optionally include one of the following values
        //
        /// <summary>
        /// This flag has no effect if terminal services is enabled. Otherwise, the system does not send the WM_QUERYENDSESSION message. This can cause applications to lose data. Therefore, you should only use this flag in an emergency.
        /// </summary>
        public const uint EWX_FORCE = 0x00000004;
        /// <summary>
        /// Forces processes to terminate if they do not respond to the WM_QUERYENDSESSION or WM_ENDSESSION message within the timeout interval.
        /// </summary>
        public const uint EWX_FORCEIFHUNG = 0x00000010;
    }
}
