using System;
using System.Windows.Forms;

namespace WinFormsExample
{
    public class SnapWindow
    {
        private Form form;
        private IntPtr handle;
        public uint SnapDist = 50;

        public SnapWindow(Form form)
        {
            this.form = form;
            this.handle = form.Handle;
            this.form.ResizeEnd += Form_ResizeEnd;
        }

        private bool DoSnap(int pos, int edge)
        {
            int delta = pos - edge;
            return delta > 0 && delta <= SnapDist;
        }

        private void Form_ResizeEnd(object sender, EventArgs e)
        {
            var winRect = WinApi.GetWindowRectangle(this.handle);
            Screen screen = Screen.FromPoint(this.form.Location);

            int widthReal = winRect.Right - winRect.Left;
            int heightReal = winRect.Bottom - winRect.Top;

            if (DoSnap(winRect.Left, screen.WorkingArea.Left))
                this.form.Left = widthReal - this.form.Width;
            if (DoSnap(winRect.Top, screen.WorkingArea.Top))
                this.form.Top = screen.WorkingArea.Top;
            if (DoSnap(screen.WorkingArea.Right, winRect.Right))
                this.form.Left = screen.WorkingArea.Right - widthReal;
            if (DoSnap(screen.WorkingArea.Bottom, winRect.Bottom))
                this.form.Top = screen.WorkingArea.Bottom - heightReal;
        }

    }
}
