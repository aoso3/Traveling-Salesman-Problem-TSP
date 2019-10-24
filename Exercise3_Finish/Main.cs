using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Windows.Forms.DataVisualization.Charting;
namespace Exercise3_Finish
{
    public partial class Main : Telerik.WinControls.UI.RadForm
    {

        Series A = new Series() { ChartType = SeriesChartType.Line };
        Series B = new Series() { ChartType = SeriesChartType.Point };
        static List<int> T = new List<int>();
        static int n;
        static double min ;
        int[] ok;

        double[] a;
        double[] b;
        public Main()
        {
            InitializeComponent();
            chart1.Series.Add(B);
            chart1.Series.Add(A);
         //   comboBox2.Hide();
           // radLabel8.Hide();
        }



        private void radButton_Add_Click_1(object sender, EventArgs e)
        {
            
            if ((radTextBox1.Text != string.Empty) && (radTextBox2.Text != string.Empty))
            {

                double x = Convert.ToDouble(radTextBox1.Text);
                double y = Convert.ToDouble(radTextBox2.Text);
                comboBox1.Items.Add(x.ToString() + "," + y.ToString());
                chart1.Series[0].Points.Add(new DataPoint(x, y));
                chart1.Invoke(new Action(Refresh));
                radTextBox1.Text = "";
                radTextBox2.Text = "";

            }

        }



        private void radButton_Delete_Click_1(object sender, EventArgs e)
        {
            if (comboBox1.Text != string.Empty)
            {
                if (comboBox1.Items.Contains(comboBox1.Text))
                {
                    string[] t = comboBox1.Text.Split(',');
                    comboBox1.Items.Remove(comboBox1.Text);
                    double x = Convert.ToDouble(t[0]);
                    double y = Convert.ToDouble(t[1]);

                    for (int i = 0; i < B.Points.Count; i++)
                        if ((B.Points[i].XValue == x) && (B.Points[i].YValues[0] == y))
                            chart1.Series[0].Points.RemoveAt(i);
                    chart1.Invoke(new Action(Refresh));


                }
                comboBox1.Text = "";

            }

        }
        private void radButton_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button_Draw_Click(object sender, EventArgs e)
        {
            if (radRadioButton1.CheckState != 0)
                Do1();
            else
                if (radRadioButton2.CheckState != 0)
                    Do2();
        }

        private void radButton_Back_Click(object sender, EventArgs e)
        {
            A.Points.Clear();
            B.Points.Clear();
            for (int r = 2; r < chart1.Series.Count; r++)
            {
                chart1.Series.RemoveAt(r); 
            }
            chart1.Invoke(new Action(Refresh));
            comboBox2.Items.Clear();
            comboBox1.Items.Clear();
            comboBox2.Text = ""; 
            radPanel2.Show();
            radPanel1.Show();
            radTextBox3.Text = "";
            radCheckBox1.Checked = false;
         //   comboBox2.Hide();
            radRadioButton1.CheckState = 0;
            radRadioButton2.CheckState = 0;
            radTextBox4.Text = "";
          //  radLabel8.Hide();
        }


        private void radButton_Go_Click(object sender, EventArgs e)
        {

            if ((radCheckBox1.Checked) && (radTextBox3.Text != ""))
            {
                chart1.Series[0].Points.Clear();
                Random r = new Random();
                for (int i = 0; i < Convert.ToInt32(radTextBox3.Text); i++)
                    chart1.Series[0].Points.Add(new DataPoint(r.Next(100), r.Next(100)));

            }

            if (chart1.Series[0].Points.Count != 0)
                if ((radRadioButton1.CheckState != 0) || (radRadioButton2.CheckState != 0))
                {
                    radPanel2.Hide();
                    radPanel1.Hide();
                    comboBox2.Show();
                    radLabel8.Show();
                    if (radRadioButton1.CheckState != 0)
                        radLabel8.Text = "Backtracking";
                    else
                        radLabel8.Text = "Greedy";

                    this.Button_Draw.Enabled = true;
                    intilize();
                }
        }
            

        


        //after Go
        private void intilize()
        {
            n = B.Points.Count;
           
            ok = new int[n];
            a = new double[n];
            b = new double[n];
            min = 0;
            for (var e = 0; e < n; e++)
            { ok[e] = 0; }
            for (int i = 0; i < n; i++)
            {
                a[i] = B.Points[i].XValue;
                b[i] = B.Points[i].YValues[0];
                comboBox2.Items.Add(a[i].ToString() + ',' + b[i].ToString());
            }

        }
        

       

        private void Do1()
        {
            if ((comboBox2.Items.Contains(comboBox2.Text)) || (comboBox2.Text == string.Empty))
            {

                double x, y;
                if (comboBox2.Text == "")
                {
                    x = a[0];
                    y = b[0];
                    ok[0] = 1;
                }
                else
                {
                    string[] str = comboBox2.Text.Split(',');
                    x = Convert.ToDouble(str[0]);
                    y = Convert.ToDouble(str[1]);
                    for (int j = 0; j < n; j++)
                        if ((a[j] == x) && (b[j] == y))
                            ok[j] = 1;
                }
                chart1.Series[1].Points.Add(new DataPoint(x, y));
                Try(2, x, y, 0);
                radTextBox4.Text = min.ToString();
                this.Button_Draw.Enabled = false;
                while (T.Count != 0)
                {
                    Series s = new Series() { ChartType = SeriesChartType.Line };
                    for (int k = 0; k < n; k++)
                    {
                        ok[k] = T[0];
                        T.RemoveAt(0);
                    }

                    for (int k = 1; k <= n; k++)
                        for (int j = 0; j < n; j++)
                            if (k == ok[j])
                            {
                                s.Points.Add(new DataPoint(a[j], b[j]));
                                if (k==1)
                                    comboBox2.Text=a[j].ToString() + "," + b[j].ToString();
                            }
                    chart1.Series.Add(s);
                }
                chart1.Invoke(new Action(Refresh));
            }
        }
   



        private void Try(int i, double x, double y, double C)
        {
            int k = -1;
            do
            {
                k++;
                double x0 = a[k];
                double y0 = b[k];
                if (ok[k] == 0)
                {
                    double m = System.Math.Sqrt((x - x0) * (x - x0) + (y - y0) * (y - y0));
                    C += m;
                    ok[k] = i;
                    chart1.Series[1].Points.Add(new DataPoint(x0, y0));
                    chart1.Invoke(new Action(Refresh));
                    if (i == n)
                    {
                        if ((min == 0) || (C == min))
                        {
                            min = C;
                            for (int j = 0; j < n; j++)
                                T.Add(ok[j]);
                        }
                        else
                            if (C < min)
                            {
                                min = C;
                                T.Clear();
                                for (int j = 0; j < n; j++)
                                    T.Add(ok[j]);
                            }
                    }
                    else
                        Try(i + 1, x0, y0, C);
                    ok[k] = 0;
                    chart1.Series[1].Points.RemoveAt(chart1.Series[1].Points.Count - 1);
                    chart1.Invoke(new Action(Refresh));
                    C -= m;
                }
            }
            while (k < n - 1);
        }



       

      

        private void radCheckBox1_CheckStateChanged(object sender, EventArgs e)
        {
            if (radCheckBox1.Checked)
            {
                radPanel2.Enabled = false;

            }
            else
                radPanel2.Enabled = true;
        }






        private void Do2()
        {

            if ((comboBox2.Items.Contains(comboBox2.Text)) || (comboBox2.Text == string.Empty))
            {
                double x, y;
                int i = 0;
                if (comboBox2.Text == "")
                {
                    x = a[0];
                    y = b[0];
                    i = 1;
                }
                else
                {
                    string[] str = comboBox2.Text.Split(',');
                    x = Convert.ToDouble(str[0]);
                    y = Convert.ToDouble(str[1]);
                    for (int j = 0; j < n; j++)
                        if ((a[j] == x) && (b[j] == y))
                        {
                            ok[j] = 1;
                            i = j;
                        }
                }
             
              Greedy(i);

                this.Button_Draw.Enabled = false;
                radTextBox4.Text = min.ToString();


            }
        }



        public void Greedy(int r)
        {
            int i = 1;
            double x0 = a[r];
            double y0 = b[r];
            chart1.Series[1].Points.Add(new DataPoint(x0, y0));
            ok[r] = i;
            int j = 2;
            while (j <= n)
            {
                int z = 0;
                double m = 0;
                for (int k = 0; k < n; k++)
                    if (ok[k] == 0)
                    {
                        chart1.Series[1].Points.Add(new DataPoint(a[k], b[k]));
                        chart1.Invoke(new Action(Refresh));
                        double e = System.Math.Sqrt((a[k] - x0) * (a[k] - x0) + (b[k] - y0) * (b[k] - y0));
                        if ((e < m) || (m == 0))
                        {
                            m = e;
                            z = k;
                        }
                        chart1.Series[1].Points.RemoveAt(chart1.Series[1].Points.Count - 1);
                        chart1.Invoke(new Action(Refresh));
                    }
                min += m;
                ok[z] = ++i;
                x0 = a[z];
                y0 = b[z];
                chart1.Series[1].Points.Add(new DataPoint(x0, y0));
                chart1.Invoke(new Action(Refresh));
                j++;
            }
        }

    

        
   
    }
}