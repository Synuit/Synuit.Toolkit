﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Example
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            propertyGrid1.SelectedObject = Example;
        }

        private ExampleClass Example = new ExampleClass();

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
