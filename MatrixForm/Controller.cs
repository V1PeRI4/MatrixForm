using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixForm
{
    internal class Controller
    {
        delegate void ControllerHndl(int result);
        event ControllerHndl CtrlEvent;

        public Controller(Model model)
        {
            Model = model;
        }

        Model Model { get; set; }
    }
}
