using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PuzzleHunt2018.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private CodeEntryViewModel workingCodeEntry;
        private ObservableCollection<CodeEntryViewModel> enteredCodes;
        private DelegateCommand resetCommand;
        private DelegateCommand enterInactiveBitCommand;
        private DelegateCommand enterActiveBitCommand;
        private DelegateCommand submitCommand;
        private DateTime lastCommandExecution = DateTime.MinValue;
        private TimeSpan requiredTimespanBetweenCommandExecutions = TimeSpan.FromMilliseconds(300);

        public CodeEntryViewModel WorkingCodeEntry
        {
            get => workingCodeEntry;
            private set
            {
                workingCodeEntry = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<CodeEntryViewModel> EnteredCodes => enteredCodes;

        public ICommand ResetCommand => resetCommand;

        public ICommand EnterInactiveBitCommand => enterInactiveBitCommand;

        public ICommand EnterActiveBitCommand => enterActiveBitCommand;

        public ICommand SubmitCommand => submitCommand;

        public MainViewModel()
        {
            Reset();
            resetCommand = new DelegateCommand(GetWrappedAction(Reset), HasBeenMinIntervalTimespanBetweenCommandExecutions);
            submitCommand = new DelegateCommand(GetWrappedAction(Submit), HasBeenMinIntervalTimespanBetweenCommandExecutions);
            enterInactiveBitCommand = new DelegateCommand(GetWrappedAction(AddInactiveBit), HasBeenMinIntervalTimespanBetweenCommandExecutions);
            enterActiveBitCommand = new DelegateCommand(GetWrappedAction(AddActiveBit), HasBeenMinIntervalTimespanBetweenCommandExecutions);
        }

        private bool HasBeenMinIntervalTimespanBetweenCommandExecutions() => DateTime.Now - lastCommandExecution > requiredTimespanBetweenCommandExecutions;

        private void SetLastCommandExecutionTime()
        {
            lastCommandExecution = DateTime.Now;
        }

        private Action GetWrappedAction(Action innerAction)
        {
            return () =>
            {
                innerAction();
                SetLastCommandExecutionTime();
            };
        }

        private void Reset()
        {
            workingCodeEntry = new CodeEntryViewModel();
            enteredCodes = new ObservableCollection<CodeEntryViewModel>();
        }

        private void Submit()
        {
            enteredCodes.Add(workingCodeEntry.CloneAndReset());
        }

        private void AddInactiveBit() => ShiftBitsAndAdd(false);

        private void AddActiveBit() => ShiftBitsAndAdd(true);

        private void ShiftBitsAndAdd(bool leastSignificantBit)
        {
            workingCodeEntry.IsBit1Active = workingCodeEntry.IsBit2Active;
            workingCodeEntry.IsBit2Active = workingCodeEntry.IsBit3Active;
            workingCodeEntry.IsBit3Active = workingCodeEntry.IsBit4Active;
            workingCodeEntry.IsBit4Active = leastSignificantBit;
        }
    }
}
