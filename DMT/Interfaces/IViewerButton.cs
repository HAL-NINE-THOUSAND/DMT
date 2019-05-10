using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DMT.Interfaces
{
    public interface IViewerButton
    {
        void Action(object sender, EventArgs args);
        string GetName();

        void AlterAppearance(Button button);
    }
}
