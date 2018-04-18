using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for PageDataGrid.xaml
    /// </summary>
    public partial class PageDataGrid : UserControl
    {
        private List<string> originalData;

        private int MaxPage => (originalData.Count() / PageSize) + 1;

        public ObservableCollection<string> Items
        {
            get { return (ObservableCollection<string>)GetValue(ItemsProperty); }
            set
            {
                //ova linija koda se dogada uvijek
                SetValue(ItemsProperty, value);

                //zato samo postavljamo ovo prvi put i vise nikad; ovo je cijela lista
                if (originalData == null)
                {
                    originalData = value?.ToList();
                    CurrentPage = 1;
                }
            }
        }
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<string>), typeof(PageDataGrid), new PropertyMetadata(null));
        
        public string PageNumberInfo
        {
            get { return (string)GetValue(PageNumberInfoProperty); }
            set { SetValue(PageNumberInfoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageNumberInfo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageNumberInfoProperty =
            DependencyProperty.Register("PageNumberInfo", typeof(string), typeof(PageDataGrid), new PropertyMetadata(null));
        
        private int currentPage;

        public int CurrentPage
        {
            get { return currentPage; }
            private set
            {
                if (value != currentPage && value >= 1 && value <= MaxPage)
                {
                    currentPage = value;
                    DisplayPage();
                }
            }
        }


        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(PageDataGrid), new PropertyMetadata(10));



        private void DisplayPage()
        {
            var startingPoint = originalData.Skip(PageSize * (currentPage - 1));

            if (startingPoint == null || startingPoint.Count() == 0)
            {
                return;
            }

            Items = new ObservableCollection<string>();

            for (int i = 0; i < PageSize; i++)
            {
                if (i >= startingPoint.Count())
                {
                    break;
                }

                var element = startingPoint.ElementAt(i);

                Items.Add(element);

            }

            PageNumberInfo = $"Page {CurrentPage} of {MaxPage}";
        }



        #region Commands
        public ICommand FirstCmd
        {
            get { return (ICommand)GetValue(FirstCmdProperty); }
            set { SetValue(FirstCmdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FirstCmd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FirstCmdProperty =
            DependencyProperty.Register("FirstCmd", typeof(ICommand), typeof(PageDataGrid), new PropertyMetadata(null));

        public ICommand NextCmd
        {
            get { return (ICommand)GetValue(NextCmdProperty); }
            set { SetValue(NextCmdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NextCmd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NextCmdProperty =
            DependencyProperty.Register("NextCmd", typeof(ICommand), typeof(PageDataGrid), new PropertyMetadata(null));

        public ICommand PreviousCmd
        {
            get { return (ICommand)GetValue(PreviousCmdProperty); }
            set { SetValue(PreviousCmdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PreviousCmd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PreviousCmdProperty =
            DependencyProperty.Register("PreviousCmd", typeof(ICommand), typeof(PageDataGrid), new PropertyMetadata(null));

        public ICommand LastCmd
        {
            get { return (ICommand)GetValue(LastCmdProperty); }
            set { SetValue(LastCmdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LastCmd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LastCmdProperty =
            DependencyProperty.Register("LastCmd", typeof(ICommand), typeof(PageDataGrid), new PropertyMetadata(null));

        #endregion


        public PageDataGrid()
        {
            InitializeComponent();
            
            DataContext = this;

            FirstCmd = new RelayCommand(obj => CurrentPage = 1);
            PreviousCmd = new RelayCommand(obj => CurrentPage = (CurrentPage - 1));
            NextCmd = new RelayCommand(obj => CurrentPage = (CurrentPage + 1));
            LastCmd = new RelayCommand(obj => CurrentPage = MaxPage);

        }
    }
}
