using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace Calculator.ViewModels
{
	public class CalculatorPageViewModel : BindableBase
	{
        private double _amount;
        private bool _isOprClicked;
        private bool _isTotalCounted;
        private string _historyNum;
        private string _dynamicNum;

        public ImageSource DeleteImg
        {
            get { return ImageSource.FromResource("Calculator.Icons.delete.png"); }
        }

        public string HistoryNum
        {
            get { return _historyNum; }
            set { SetProperty(ref _historyNum, value); }
        }
        public string DynamicNum
        {
            get { return _dynamicNum; }
            set { SetProperty(ref _dynamicNum, value); }
        }

        public CalculatorPageViewModel()
        {
            HistoryNum = "";
            DynamicNum = "0";
        }

        private DelegateCommand _onBtnClearClickedCommand;
        public DelegateCommand OnBtnClearClickedCommand =>
            _onBtnClearClickedCommand ?? (_onBtnClearClickedCommand = new DelegateCommand(onBtnClearClicked));

        private DelegateCommand _onBtnDelClickedCommand;
        public DelegateCommand OnBtnDelClickedCommand =>
            _onBtnDelClickedCommand ?? (_onBtnDelClickedCommand = new DelegateCommand(onBtnDelClicked));

        private DelegateCommand _onBtnTotalClickedCommand;
        public DelegateCommand OnBtnTotalClickedCommand =>
            _onBtnTotalClickedCommand ?? (_onBtnTotalClickedCommand = new DelegateCommand(onBtnTotalClicked));

        private DelegateCommand<string> _onBtnOprClickedCommand;
        public DelegateCommand<string> OnBtnOprClickedCommand =>
            _onBtnOprClickedCommand ?? (_onBtnOprClickedCommand = new DelegateCommand<string>((opr) =>
            {
                onBtnOprClicked(opr);
            }));

        private DelegateCommand<string> _onBtnNumClickedCommand;
        public DelegateCommand<string> OnBtnNumClickedCommand =>
            _onBtnNumClickedCommand ?? (_onBtnNumClickedCommand = new DelegateCommand<string>((num) =>
            {
                OnBtnNumClicked(num);
            }));

        private DelegateCommand _onBtnClearAllClickedCommand;
        public DelegateCommand OnBtnClearAllClickedCommand =>
            _onBtnClearAllClickedCommand ?? (_onBtnClearAllClickedCommand = new DelegateCommand(OnBtnClearAllClicked));


        private async void onBtnClearClicked()
        {
            DynamicNum = "0";
        }

        private async void OnBtnClearAllClicked()
        {
            DynamicNum = "0";
            HistoryNum = "";
            _amount = 0;
        }

        private void clearTotal()
        {
            if (_isTotalCounted)
            {
                OnBtnClearAllClicked();
                _isTotalCounted = false;
            }
        }


        private async void onBtnDelClicked()
        {
            if (!_isTotalCounted && DynamicNum != "0")
            {
                if (DynamicNum.Length == 1)
                    DynamicNum = "0";
                else
                {
                    int len = DynamicNum.Length;
                    DynamicNum = DynamicNum.Substring(0, len - 1);
                }
            }
        }

        private void calculate()
        {
            string[] historyNumSplitted = HistoryNum.Split(' ');
            string lastOpr = historyNumSplitted[historyNumSplitted.Length - 1];
            double dynamicNum = double.Parse(DynamicNum);

            if (lastOpr == "+")
                _amount += dynamicNum;
            else if (lastOpr == "-")
                _amount -= dynamicNum;
            else if (lastOpr == "x")
                _amount *= dynamicNum;
            else if (lastOpr == "/")
                _amount /= dynamicNum;
        }

        private async void onBtnTotalClicked()
        {
            if (!_isTotalCounted && HistoryNum != "" && DynamicNum != "0")
            {
                calculate();
                HistoryNum = HistoryNum + " " + DynamicNum + " = " + _amount;
                DynamicNum = "= " + _amount;
                _isTotalCounted = true;
            }
        }

        private async void onBtnOprClicked(string opr)
        {
            clearTotal();

            if (HistoryNum == "")
            {
                if (DynamicNum != "0")
                {
                    HistoryNum = DynamicNum + " " + opr;
                    _amount = double.Parse(DynamicNum);
                }
            }
            else
            {
                calculate();
                HistoryNum = HistoryNum + " " + DynamicNum + " " + opr;
                DynamicNum = _amount + "";
            }
            _isOprClicked = true;
        }

        private async void OnBtnNumClicked(string num)
        {
            clearTotal();

            if (HistoryNum == "")
                updateDynamicNum(num);
            else
            {
                if (_isOprClicked)
                {
                    DynamicNum = num;
                    _isOprClicked = false;
                }
                else
                    updateDynamicNum(num);
            }
        }

        private void updateDynamicNum(string num)
        {
            if (DynamicNum == "0")
                DynamicNum = num;
            else
                DynamicNum = DynamicNum + num;
        }
    }
}
