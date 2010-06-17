using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Esath.Pie.Helpers
{
    public static class WinFormsUtils
    {
        public static Control SearchRecursive(this Control c, String name)
        {
            if (c.Name == name) return c;
            var res = c.Controls.Cast<Control>().Select(c1 => SearchRecursive(c1, name));
            return res.FirstOrDefault(c2 => c2 != null);
        }

        public static bool Confirm(this object o)
        {
//            return MessageBox.Show(
//                Resources.Warning_IrreversibleChange, Resources.Warning_Caption,
//                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
//            == DialogResult.Yes;

            return true;
        }

        public static void EnsureSameSize(this Label l1, Label l2)
        {
            l1.AutoSize = l2.AutoSize = false;
            l1.Width = l2.Width = Math.Max(l1.Width, l2.Width);
        }

        public static IEnumerable<Control> Parents(this Control c)
        {
            for (var current = c.Parent; current != null; current = current.Parent)
                yield return current;
        }
    }
}
