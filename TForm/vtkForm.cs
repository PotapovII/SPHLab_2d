using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//using Kitware.VTK; 

namespace TForm
{
    public partial class vtkForm : Form
    {
        public vtkForm()
        {
            InitializeComponent();
            //var sphere = vtkSphereSource.New();
            //sphere.SetThetaResolution(8);
            //sphere.SetPhiResolution(16);
            //if (sphere != null) 
            //{ 
            //    sphere.Dispose(); 
            //} 
        }

        private void vtkForm_Load(object sender, EventArgs e)
        {

        }

    }
}
