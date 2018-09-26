using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleHunt2018.ViewModels
{
    public class CodeEntryViewModel : BaseViewModel
    {
        private bool isBit1Active;
        private bool isBit2Active;
        private bool isBit3Active;
        private bool isBit4Active;

        public bool IsBit1Active
        {
            get => isBit1Active;
            set
            {
                if (value != isBit1Active)
                {
                    isBit1Active = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsBit2Active
        {
            get => isBit2Active;
            set
            {
                if (value != isBit2Active)
                {
                    isBit2Active = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsBit3Active
        {
            get => isBit3Active;
            set
            {
                if (value != isBit3Active)
                {
                    isBit3Active = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsBit4Active
        {
            get => isBit4Active;
            set
            {
                if (value != isBit4Active)
                {
                    isBit4Active = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public CodeEntryViewModel CloneAndReset()
        {
            var copy = new CodeEntryViewModel
            {
                IsBit1Active = isBit1Active,
                IsBit2Active = isBit2Active,
                IsBit3Active = isBit3Active,
                IsBit4Active = isBit4Active
            };

            IsBit1Active = false;
            IsBit2Active = false; 
            IsBit3Active = false;
            IsBit4Active = false;

            return copy;
        }

        public override bool Equals(object obj)
        {
            var other = obj as CodeEntryViewModel;

            if (other == null) return false;

            return IsBit1Active == other.IsBit1Active &&
                   IsBit2Active == other.IsBit2Active &&
                   IsBit3Active == other.IsBit3Active &&
                   IsBit4Active == other.IsBit4Active;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
