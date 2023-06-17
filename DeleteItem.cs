﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using AForge.Video.DirectShow;
using ZXing;


namespace Final_Project
{
    public partial class DeleteItem : Form
    {
        public DeleteItem()
        {
            InitializeComponent();
        }


        private void BTNSUpplierDash_Click(object sender, EventArgs e)
        {
            Itemmaster im = new Itemmaster();
            im.Show();
            this.Hide();
        }

        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            string query = "";
            Item it = new Item();

            if (txtBarcode.Text == "")
            {

            }
            else
                it.Barcode = txtBarcode.Text;

            query = "Select * from Item_t where Barcode ='" + it.Barcode+ "';";
            DAL.SupplierDataGrid(dataGridView1, it, query, "Item_t");
              }


        
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;
        private void btnConnectCamera_Click(object sender, EventArgs e)
        {
            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[comboCamera.SelectedIndex].MonikerString);
            videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            videoCaptureDevice.Start();
        }

        private void VideoCaptureDevice_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();

            BarcodeReader reader = new BarcodeReader();
            var result = reader.Decode(bitmap);
            if (result != null)
            {
                txtBarcode.Invoke(new MethodInvoker(delegate()
                {
                    txtBarcode.Text = result.ToString();
                }));
            }

            pictureBox1.Image = bitmap;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (videoCaptureDevice != null)
            {
                if (videoCaptureDevice.IsRunning)
                    videoCaptureDevice.Stop();
            }
        }

        private void DeleteItem_Load(object sender, EventArgs e)
        {
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in filterInfoCollection)
                comboCamera.Items.Add(device.Name);
            comboCamera.SelectedIndex = 0;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtBarcode.Text = "";
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            Item item = new Item();
            item.Barcode = txtBarcode.Text;
            string query = "Delete from Item_t where Barcode = '" + item.Barcode + "'";
            DAL.DeleteSupplierData(query);
            txtBarcode.Text = "";
        }
    }
}
