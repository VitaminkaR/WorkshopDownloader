using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace WorkshopDownloader
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();

            FileStream fs = File.Open(Environment.CurrentDirectory + "\\path.txt", FileMode.OpenOrCreate);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            steamcmd_field.Text = Encoding.Default.GetString(buffer);
            fs.Close();

            download_button.Click += Download_button_Click;
            steamcmd_field.TextChanged += Steamcmd_field_TextChanged;
        }

        private void Steamcmd_field_TextChanged(object sender, TextChangedEventArgs e)
        {
            FileStream fs = File.Create(Environment.CurrentDirectory + "\\path.txt");
            byte[] buffer = Encoding.Default.GetBytes(steamcmd_field.Text);
            fs.Write(buffer, 0, buffer.Length);
            fs.Close();
        }

        private void Download_button_Click(object sender, RoutedEventArgs e)
        {
            long game_id = Convert.ToInt64(game_field.Text);
            string item_ids = item_field.Text;
            string steamcmd_path = steamcmd_field.Text;

            Process process = new Process();
            process.StartInfo.FileName = steamcmd_path + @"\steamcmd.exe";
            process.StartInfo.Arguments = @"+login anonymous ";
            foreach (var item_id in item_ids.Split(';'))
            {
                process.StartInfo.Arguments += $"+workshop_download_item {game_id} {item_id} ";
            }
            process.StartInfo.Arguments += "+quit";
            process.Start();

            if ((bool)open_directory_field.IsChecked)
                Process.Start(@"C:\Windows\explorer.exe", steamcmd_path + @"\steamapps\workshop\content\" + game_id.ToString());
        }


    }
}
