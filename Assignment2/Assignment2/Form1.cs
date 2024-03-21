using System.Diagnostics.Eventing.Reader;

namespace Assignment2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<User> data = new List<User>();
        Dictionary<string, string> dic = new Dictionary<string, string>();

        private void btnShow_Click(object sender, EventArgs e)
        {
            try
            {

                string fileName = txtFile.Text.Trim();

                string line;
                using (StreamReader sw = new StreamReader(fileName)) {
                    line = sw.ReadLine();
                    while (line != null)
                    {

                        string[] txt = line.Split("|");

                        User user = new User()
                        {
                            Code = txt[0],
                            Name = txt[1],
                            Salary = double.Parse(txt[2]),
                            Bonus = double.Parse(txt[3]),

                        };
                        dic.Add(txt[0], txt[1]);
                        listUser.Items.Add(user);
                        data.Add(user);
                        line = sw.ReadLine();
                    }

                }


            }

            catch (Exception ex)
            { 
                listUser.Items.Clear();
                data.Clear();
                MessageBox.Show("Load File Fail!!!!!!!!");
                return;
            }

           

        }

        public bool checkFormat(string line)
        {
            string[] txt = line.Split("|");
            if (txt.Length == 4)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        int count = 0;

        private void btnSort_Click(object sender, EventArgs e)
        {
            
            UserSort userSort = new UserSort();
            data.Sort(userSort);
            listUser.Items.Clear();
            if (count % 2 == 0)
            {
                listUser.Items.Clear();
                foreach (User item in data)
                {
                    listUser.Items.Add(item);
                }
            } else
            {
                listUser.Items.Clear();
                for(int i = (data.Count-1); i >=0; i--)
                {
                    listUser.Items.Add(data[i]);
                }
            }

            count++;

        }

        public class UserSort : IComparer<User>
        {
            public int Compare(User? x, User? y)
            {
                return x.Code.CompareTo(y.Code);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Do you want to exit?", "Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Environment.Exit(0);
            }
        }
    }

}


