using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HomeTask1_Elevators.Models;

namespace HomeTask1_Elevators.ViewModels
{
	
		public class MainWindowViewModel : INotifyPropertyChanged
		{
			public Elevator Elevator1 { get; set; }
			public Elevator Elevator2 { get; set; }
			public Elevator Elevator3 { get; set; }

			public Dictionary<int, string> FloorButtonColors { get; private set; }

			public ICommand ExternalFloorButtonCommand { get; }
			public ICommand Elevator1ButtonCommand { get; }
			public ICommand Elevator2ButtonCommand { get; }
			public ICommand Elevator3ButtonCommand { get; }

			public MainWindowViewModel()
			{
				Elevator1 = new Elevator();
				Elevator2 = new Elevator();
				Elevator3 = new Elevator();

				FloorButtonColors = new Dictionary<int, string>();
				for (int i = 0; i <= 10; i++)
				{
					FloorButtonColors[i] = "DodgerBlue"; // Initialize all buttons to DodgerBlue
				}

				// Subscribe to the FloorReached event for all elevators
				Elevator1.FloorReached += OnFloorReached;
				Elevator2.FloorReached += OnFloorReached;
				Elevator3.FloorReached += OnFloorReached;

				ExternalFloorButtonCommand = new RelayCommand<int>(OnExternalFloorButtonPressed);
				Elevator1ButtonCommand = new RelayCommand<int>(floor => OnElevatorButtonPressed(Elevator1, floor));
				Elevator2ButtonCommand = new RelayCommand<int>(floor => OnElevatorButtonPressed(Elevator2, floor));
				Elevator3ButtonCommand = new RelayCommand<int>(floor => OnElevatorButtonPressed(Elevator3, floor));
			}

			private void OnExternalFloorButtonPressed(int floor)
			{
				Elevator closestElevator = GetClosestElevator(floor);
				closestElevator.AddFloorRequest(floor);

				FloorButtonColors[floor] = "Red";
				OnPropertyChanged(nameof(FloorButtonColors));
			}

			private void OnElevatorButtonPressed(Elevator elevator, int floor)
			{
				elevator.AddFloorRequest(floor);
				FloorButtonColors[floor] = "Red";
				OnPropertyChanged(nameof(FloorButtonColors));
			}

			private void OnFloorReached(int floor)
			{
				// If all elevators have reset the button color, reset the floor button as well
				if (Elevator1.ButtonColors[floor] == "DodgerBlue" &&
					Elevator2.ButtonColors[floor] == "DodgerBlue" &&
					Elevator3.ButtonColors[floor] == "DodgerBlue")
				{
					FloorButtonColors[floor] = "DodgerBlue";
					OnPropertyChanged(nameof(FloorButtonColors));
				}
			}

			private Elevator GetClosestElevator(int floor)
			{
				var distances = new Dictionary<Elevator, int>
		{
			{ Elevator1, Math.Abs(Elevator1.CurrentFloor - floor) },
			{ Elevator2, Math.Abs(Elevator2.CurrentFloor - floor) },
			{ Elevator3, Math.Abs(Elevator3.CurrentFloor - floor) }
		};

				return distances.OrderBy(d => d.Value).First().Key;
			}

			public event PropertyChangedEventHandler PropertyChanged;
			protected void OnPropertyChanged([CallerMemberName] string name = null)
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
			}
		}


		// Implementation of RelayCommand
		public class RelayCommand<T> : ICommand
	{
		private readonly Action<T> _execute;
		private readonly Func<T, bool> _canExecute;

		public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute == null || _canExecute((T)parameter);
		}

		public void Execute(object parameter)
		{
			if (parameter is T validParameter)
			{
				_execute(validParameter);
			}
			else if (parameter != null)
			{
				_execute((T)Convert.ChangeType(parameter, typeof(T)));
			}
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
	}
}
