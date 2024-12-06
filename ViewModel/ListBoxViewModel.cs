using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TimeTrack_Pro.Model;

namespace TimeTrack_Pro.ViewModel
{
    public class ListBoxViewModel : ViewModelBase
    {
        private ObservableCollection<LBDataModel> dataList;
        public ObservableCollection<LBDataModel> DataList
        {
            get => dataList;
            set => Set(ref dataList, value);
        }

        public ListBoxViewModel()
        {
            DataList = GetDataList();
        }

        private ObservableCollection<LBDataModel> GetDataList()
        {
            return new ObservableCollection<LBDataModel>
            {
                new LBDataModel{ Name = "备份表操作"},
                new LBDataModel{ Name = "原始表操作"},
                new LBDataModel{ Name = "部门管理"},
                new LBDataModel{ Name = "考勤规则"},                
            };
        }
    }
}
