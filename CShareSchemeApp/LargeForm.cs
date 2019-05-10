using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CShareSchemeApp
{
    public partial class LargeForm : Form
    {
        public LargeForm()
        {
            this.Font = new System.Drawing.Font("Microsoft YaHei", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Width = 900;
            this.Height = 556;
            this.Padding = new System.Windows.Forms.Padding(10, 11, 10, 11);
        }
    }
}
