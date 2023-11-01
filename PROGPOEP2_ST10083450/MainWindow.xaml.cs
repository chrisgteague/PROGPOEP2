using Microsoft.Data.SqlClient;
using PROG6212_POE_P1_Library_ST10083450_Christopher_Teague;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PROGPOEP2_ST10083450
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string modName;
        public string modCode;
        public double numCreds;
        public double numClassHrs;
        public static double numOfWeeks;
        public DateTime semDate;
        public double numSelfStudyHrs;
        public DateTime selfStudyDate;
        public String mCode;
        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;
        }

        public SqlConnection sqlConnection;

        private void btnAddSemester_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Semester semester = new Semester();
                numOfWeeks = Convert.ToDouble(tbNumberOfWeeks.Text);
                semDate = Convert.ToDateTime(dpSemesterStartDate.Text);

                ListUtil.semesters.Add(new Semester { numWeeks = numOfWeeks, semesterStartDate = semDate });
                MessageBox.Show("Semester Info recorded");





            }
            catch
            {
                MessageBox.Show("Please enter the values in correctly");
                tbNumberOfWeeks.Clear();
            }
        }

        private void btnAddModule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                modName = tbModuleName.Text;
                modCode = tbModuleCode.Text;
                numCreds = Convert.ToDouble(tbNumberOfCredits.Text);
                numClassHrs = Convert.ToDouble(tbClassHours.Text);


                ListUtil.modules.Add(new Module { moduleName = modName, moduleCode = modCode, numberCredits = numCreds, numClassHours = numClassHrs, remainingStudyHours = Calculations.requiredStudyHrs(numCreds, numClassHrs, numOfWeeks) });
                cbModuleCode.Items.Clear();
                List<Module> modList = ListUtil.modules;
                foreach (Module item in modList)
                {
                    cbModuleCode.Items.Add(item.moduleCode);
                }
                MessageBox.Show("Module Added!!");
                tbModuleName.Clear();
                tbModuleCode.Clear();
                tbClassHours.Clear();
                tbNumberOfCredits.Clear();
            }
            catch
            {
                MessageBox.Show("Failed to add module");
                tbModuleName.Clear();
                tbModuleCode.Clear();
                tbClassHours.Clear();
                tbNumberOfCredits.Clear();
            }
        }

        private void btnAddSelfStudyHours_Click(object sender, RoutedEventArgs e)
        {
            Calculations calculations = new Calculations();
            try
            {
                numSelfStudyHrs = Convert.ToDouble(tbNumberOfSelfStudyHours.Text);
                selfStudyDate = Convert.ToDateTime(dpDateOfSelfStudy.Text);
                mCode = cbModuleCode.Text;

                //code attribution
                //this method was taken from CodeLikeaDev
                //https://codelikeadev.com/blog/find-item-in-list-csharp
                //Sameer Saini

                //LINQ used to find Module Code
                var findModule = ListUtil.modules.Find(x => x.moduleCode == mCode);

                findModule.remainingStudyHours = Calculations.SelfStudyCalc(numSelfStudyHrs, findModule.remainingStudyHours);
                MessageBox.Show("Self Study Hours Recorded");
                tbNumberOfSelfStudyHours.Clear();
            }
            catch
            {
                MessageBox.Show("Please enter the values in correctly");

                tbNumberOfSelfStudyHours.Clear();
            }
        }
        //this method is used to update the datagrid when the button is pressed
        private void btnUpdateDisplay_Click(object sender, RoutedEventArgs e)
        {
            //code attribution
            //this method was taken from AppsLoveWorld
            //https://www.appsloveworld.com/csharp/100/1671/how-to-combine-multiple-lists-and-use-as-a-gridview-datasource?expand_article=1
            //Norberto Escobar
            //https://stackoverflow.com/users/4783509/norberto-escobar

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("moduleName");
            dataTable.Columns.Add("moduleCode");
            dataTable.Columns.Add("numberCredits");
            dataTable.Columns.Add("numClassHours");
            dataTable.Columns.Add("remainingStudyHours");

            foreach (Module module in ListUtil.modules)
            {
                dataTable.Rows.Add(module.moduleName, module.moduleCode, module.numberCredits, module.numClassHours, module.remainingStudyHours);
            }

            myDataGrid.ItemsSource = dataTable.DefaultView;

            using (SqlConnection connection = new SqlConnection(ConnectionString.connStr))
            {

            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
          Registration_and_Login registration_And_Login = new Registration_and_Login();
            this.Visibility = Visibility.Hidden;
            registration_And_Login.ShowDialog();
            this.Visibility = Visibility.Visible;
            
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
          
            Application.Current.Shutdown();
            
        }


    }
}
