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
        private List<object> originalData;
        private List<object> filteredData;
        private bool filterMode;
        private const int NumPageButtons = 10;
        private const int NumOfPagesOnLeft = 5;
        private const int NumOfPagesOnRight = 4;
        private bool forceOriginalCollection;

        private ICollection<object> CurrentList
        {
            get
            {
                if (filterMode)
                {
                    return filteredData;
                }
                else
                {
                    return originalData;
                }
            }
            
        }

        public int MaxPage
        {
            get
            {
                if (CurrentList.Count % PageSize != 0)
                {
                    return (CurrentList.Count / PageSize) + 1;
                }
                else
                {
                    return CurrentList.Count / PageSize;
                }
            }
        }

        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set
            {
                //ova linija koda se dogada uvijek
                SetValue(ItemsProperty, value);

                //zato samo postavljamo ovo prvi put i vise nikad; ovo je cijela lista
                if (originalData == null)
                {
                    originalData = value?.ToList();
                    CurrentPage = 1;
                    CreateFilterMenu();
                }
            }
        }
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<object>), typeof(PageDataGrid), new PropertyMetadata(null));
        
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
                if (value != currentPage && PageExists(value))
                {
                    currentPage = value;
                    DisplayPage();
                    AddPageButtons();
                }
            }
        }


        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value);
                DisplayPage();
            }
        }

        // Using a DependencyProperty as the backing store for PageSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(PageDataGrid), new PropertyMetadata(10));



        private void DisplayPage()
        {
            var collection = forceOriginalCollection ? originalData : CurrentList;

            var startingPoint = collection.Skip(PageSize * (currentPage - 1));

            if (startingPoint == null || startingPoint.Count() == 0)
            {
                return;
            }

            Items = new ObservableCollection<object>();

            for (int i = 0; i < PageSize && i < startingPoint.Count(); i++)
            {
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

        public ICommand ClearFiltersCmd
        {
            get { return (ICommand)GetValue(ClearFiltersCmdProperty); }
            set { SetValue(ClearFiltersCmdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LastCmd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClearFiltersCmdProperty =
            DependencyProperty.Register("ClearFiltersCmd", typeof(ICommand), typeof(PageDataGrid), new PropertyMetadata(null));

        #endregion


        public PageDataGrid()
        {
            InitializeComponent();
            
            DataContext = this;

            FirstCmd = new RelayCommand(obj => CurrentPage = 1);
            PreviousCmd = new RelayCommand(obj => CurrentPage = (CurrentPage - 1));
            NextCmd = new RelayCommand(obj => CurrentPage = (CurrentPage + 1));
            LastCmd = new RelayCommand(obj => CurrentPage = MaxPage);
            ClearFiltersCmd = new RelayCommand(obj =>
            {
                if (filterMode)
                {
                    filterMode = false;
                    forceOriginalCollection = true;
                    currentPage = 2;
                    CurrentPage = 1;
                    forceOriginalCollection = false;
                }

            });
        }

        private bool PageExists(int page)
        {
            return page >= 1 && page <= MaxPage;
        }

        public void AddPageButtons()
        {
            PageButtons.Children.Clear();

            int startPage = currentPage - NumOfPagesOnLeft;
            int endPage = currentPage + NumOfPagesOnRight;

            if (startPage < 1)
            {
                startPage = 1;
                endPage = NumPageButtons;
            }

            if (endPage > MaxPage)
            {
                endPage = MaxPage;
            }

            if (MaxPage >= NumPageButtons && endPage - startPage != NumPageButtons - 1)
            {
                startPage = endPage - (NumPageButtons - 1);
            }

            for (int i = startPage; i <= endPage; i++)
            {
                int pageNum = i;
                if (PageExists(pageNum))
                {
                    PageButtons.Children.Add(new Button
                    {
                        Content = pageNum.ToString(),
                        Command = new RelayCommand(obj => CurrentPage = pageNum)
                    });
                }
            }
        }
        

        private void CreateFilterMenu()
        {

            var obj = originalData?.ElementAt(0);
            
            foreach (var prop in obj.GetType().GetProperties())
            {
                FilterMenuItem.Items.Add(new MenuItem
                {
                    Header = prop.Name,
                    Command = new RelayCommand(param => 
                    {
                        var popup = new PromptWindow();
                        
                        popup.lblUnos.Content = $"Insert {prop.Name}";
                        popup.Owner = Window.GetWindow(this);
                        if (popup.ShowDialog()!=true)
                        {
                            return;
                        }
                        var unos = popup.txtUnos.Text;
                        filterMode = true;

                        filteredData = originalData.Where(item => Equals(prop.GetValue(item),unos)).ToList();
                        currentPage = 2;
                        CurrentPage = 1;
                    })
                });
            }
        }
    }
}
