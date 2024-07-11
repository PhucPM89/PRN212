using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjectPRN2.Support
{
    public class CustomCommand : ICommand
    {
        private Action<object>  _execute;
        private Func<object,bool> _canExecute;
        public EventHandler CanExecuteChanged;

        public CustomCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        event EventHandler? ICommand.CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;

            remove => CommandManager.RequerySuggested -= value;
        }



        // Hàm kiểm tra xem một chức năng có hoạt động hay không
        public bool CanExecute(object parameter)
        {
            //Kiểm tra nếu tham số thứ 2 = true thì hàm sẽ được thực hiện
            return _canExecute!=null && _canExecute(parameter);
        }

        //Hàm thực hiện 1 chức năng 
        //Trước khi chạy hàm này thì sẽ chạy hàm CanExecute trước để kiểm tra có thỏa mãn không
        //nếu thỏa mãn thì mới thực hiện yêu cầu tiếp theo
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        //Gọi hàm này khi muốn khởi chạy 1 chức năng nào đó
        public void FireCanExecuteChanged()
        {
            if (CanExecuteChanged == null) return;
            CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
