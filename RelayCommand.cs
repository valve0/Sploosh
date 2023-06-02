﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sploosh
{
    class RelayCommand:ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private Action<object> _Execute { get; set; }
        private Predicate<object> _CanExecute { get; set; }


        public RelayCommand(Action<object> ExecuteMethod, Predicate<object> CanExecuteMethod)
        {
            _Execute = ExecuteMethod;
            _CanExecute = CanExecuteMethod;

        }

        public RelayCommand(Action<object> ExecuteMethod)
        {
            _Execute = ExecuteMethod;

        }


        public bool CanExecute(object? parameter)
        {
            return _CanExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _Execute(parameter);
        }

    }
}
