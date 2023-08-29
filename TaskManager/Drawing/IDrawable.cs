using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing
{
    public interface IDrawable
    {
        bool Visible { get; }
        void Render();
        void Fill();
    }
}
