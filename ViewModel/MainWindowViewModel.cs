using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrack_Pro.Model;

namespace TimeTrack_Pro.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<LBDataModel> dataList;
        public ObservableCollection<LBDataModel> DataList
        {
            get => dataList;
            set => Set(ref dataList, value);
        }

        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                RaisePropertyChanged("SelectedIndex");
            }
        }

        public MainWindowViewModel()
        {
            SelectedIndex = -1;
            DataList = GetDataList();
        }

        private ObservableCollection<LBDataModel> GetDataList()
        {
            return new ObservableCollection<LBDataModel>()
            {
                new LBDataModel{ Index = 0, ImgPath = "pack://application:,,,/Resource/Image/LeftMainContent/Brush_16x.png", Name = "备份表操作" },
                new LBDataModel{ Index = 1, ImgPath = "pack://application:,,,/Resource/Image/LeftMainContent/ButtonClick_16x.png", Name = "原始表操作"},
                new LBDataModel{ Index = 2, ImgPath = "pack://application:,,,/Resource/Image/LeftMainContent/ImageStack_16x.png", Name = "部门管理"},
                new LBDataModel{ Index = 3, ImgPath = "pack://application:,,,/Resource/Image/LeftMainContent/ImageBrowser_16x.png", Name = "考勤规则"},
            };
        }
    }
}
