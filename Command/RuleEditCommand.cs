using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TimeTrack_Pro.Command
{
    public class RuleEditCommand
    {
        private static readonly RoutedUICommand ruleEdit;
        static RuleEditCommand()
        {
            InputGestureCollection inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.R, ModifierKeys.Control, "Ctrl+R"));
            ruleEdit = new RoutedUICommand(
              "RuleEdit", "RuleEdit", typeof(RuleEditCommand), inputs);
        }

        public static RoutedUICommand RuleEdit
        {
            get { return ruleEdit; }
        }

    }
}
