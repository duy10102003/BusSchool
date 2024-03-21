using Microsoft.VisualBasic.ApplicationServices;

namespace Test2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<Cal> data = new List<Cal>();

        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {

                string fileName = txtFile.Text;
                

                string[] lines = File.ReadAllLines(fileName);
                if(lines.Length ==0) 
                {
                    Cal cal = new Cal()
                    {
                        Sum = 0,
                        Aver = 0


                    };

                    listCal.Items.Add(cal);
                }
                foreach (string line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {

                        string[] txt = line.Split(" ");
                        txtFileText.Text = line;
                        double sum = 0;
                        double aver = 0;
                        for (int i = 0; i < txt.Length; i++)
                        {
                            sum += double.Parse(txt[i]);

                        }

                        aver = sum / (txt.Length + 1);
                        Cal cal = new Cal()
                        {
                            Sum = sum,
                            Aver = aver
                            

                        };

                        listCal.Items.Add(cal);

                    } 
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Load File Fail!!!!!!!!");
            }
        }
    }
}
