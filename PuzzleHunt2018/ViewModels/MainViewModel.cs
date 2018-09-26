using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PuzzleHunt2018.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        // user entered data
        private CodeEntryViewModel workingCodeEntry = new CodeEntryViewModel();
        private ObservableCollection<CodeEntryViewModel> enteredCodes = new ObservableCollection<CodeEntryViewModel>();
        private DelegateCommand resetCommand;
        private DelegateCommand enterInactiveBitCommand;
        private DelegateCommand enterActiveBitCommand;
        private DelegateCommand submitCommand;
        // protection against sensative touch screens
        private DateTime lastCommandExecution = DateTime.MinValue;
        private readonly TimeSpan requiredTimespanBetweenCommandExecutions = TimeSpan.FromMilliseconds(300);
        // solution file
        private bool isUnlocked = false;
        private const string keyFileName = "lock_key.txt";
        private List<CodeEntryViewModel> key;

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

        public bool IsUnlocked
        {
            get => isUnlocked;
            set
            {
                isUnlocked = value;
                NotifyPropertyChanged();
            }
        }

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

            LoadSolution();
        }

        #region user command methods

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
            workingCodeEntry.CloneAndReset();
            enteredCodes.Clear();
            IsUnlocked = false;
        }

        private void Submit()
        {
            enteredCodes.Add(workingCodeEntry.CloneAndReset());
            CheckEnteredCodeMatchesAgainstKey();
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

        #endregion

        #region Solution

        private void CheckEnteredCodeMatchesAgainstKey()
        {
            if (key.Count != enteredCodes.Count) return;

            for (var i = 0; i < key.Count; i++)
            {
                var keyEntry = key[i];
                var codeEntry = enteredCodes[i];

                if (!keyEntry.Equals(codeEntry)) return;
            }

            IsUnlocked = true;
        }

        private void LoadSolution()
        {
            var permissibleLine = new Regex("^[01]{4}$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            key = new List<CodeEntryViewModel>();

            try
            {
                foreach (var line in File.ReadAllLines(keyFileName).Where(l => permissibleLine.Match(l).Success))
                {
                    var bit1 = line[0] == '1';
                    var bit2 = line[1] == '1';
                    var bit3 = line[2] == '1';
                    var bit4 = line[3] == '1';

                    var keyEntry = new CodeEntryViewModel
                    {
                        IsBit1Active = bit1,
                        IsBit2Active = bit2,
                        IsBit3Active = bit3,
                        IsBit4Active = bit4
                    };

                    key.Add(keyEntry);
                }
            }
            catch (IOException e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        #endregion
    }
}
