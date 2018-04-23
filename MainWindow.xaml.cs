using System;
using System.Collections.Generic;
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

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            using(var model = new MojDAL.Model65())
            {
                MojPaginator.Items = new System.Collections.ObjectModel.ObservableCollection<object>(model.Kupac.Select(k=> new { k.Ime,k.Prezime,k.Telefon,k.Email }));
                MojPaginator.PageSize = 40;
            }

            
        }
    }
}
