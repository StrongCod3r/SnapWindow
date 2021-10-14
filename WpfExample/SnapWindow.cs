using System;
using System.Windows;
using System.Windows.Interop;

namespace WpfExample
{
    public class SnapWindow
    {
        private Window window;
        private IntPtr handle;
        public uint SnapDist = 50;

        public SnapWindow(Window window)
        {
            this.window = window;
            this.handle = new WindowInteropHelper(window).Handle;
            HwndSource source = HwndSource.FromHwnd(this.handle);
            source.AddHook(new HwndSourceHook(WndProc));
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WinApi.WM_EXITSIZEMOVE)
            {
                OnResizeEnd();
            }

            return IntPtr.Zero;
        }

        private bool DoSnap(int pos, int edge)
        {
            int delta = pos - edge;
            return delta > 0 && delta <= SnapDist;
        }

        private void OnResizeEnd()
        {
            var winRect = WinApi.GetWindowRectangle(this.handle);
            Rect screenRect = SystemParameters.WorkArea;

            int widthReal = winRect.Right - winRect.Left;
            int heightReal = winRect.Bottom - winRect.Top;

            if (DoSnap(winRect.Left, (int)screenRect.Left))
                this.window.Left = widthReal - this.window.Width;
            if (DoSnap(winRect.Top, (int)screenRect.Top))
                this.window.Top = (int)screenRect.Top;
            if (DoSnap((int)screenRect.Right, winRect.Right))
                this.window.Left = (int)screenRect.Right - widthReal;
            if (DoSnap((int)screenRect.Bottom, winRect.Bottom))
                this.window.Top = screenRect.Bottom - heightReal;
        }
    }
}
