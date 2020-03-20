using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Punto_de_ventas.ModelClass
{
   public class Imprimir
    {
        public void printDocument(PrintPageEventArgs e, GroupBox groupBox)
        {
            Bitmap bm = new Bitmap(groupBox.Width, groupBox.Height);
            groupBox.DrawToBitmap(bm, new Rectangle(0, 0, groupBox.Width, groupBox.Height));
            e.Graphics.DrawImage(bm, 0 , 0);
        }
    }
}
