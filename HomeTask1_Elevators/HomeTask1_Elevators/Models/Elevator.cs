using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HomeTask1_Elevators.Models
{
	public class Elevator : INotifyPropertyChanged
	{
		private int _currentFloor;
		private Queue<int> _upwardRequests = new Queue<int>();
		private Queue<int> _downwardRequests = new Queue<int>();
		private bool _isMovingUp = true;

		public Dictionary<int, string> ButtonColors { get; private set; } = new Dictionary<int, string>();

		public event Action<int> FloorReached; // New event

		public int CurrentFloor
		{
			get => _currentFloor;
			set
			{
				_currentFloor = value;
				OnPropertyChanged();
			}
		}

		public Elevator()
		{
			for (int i = 0; i <= 10; i++)
			{
				ButtonColors[i] = "DodgerBlue"; // Initialize all buttons to DodgerBlue
			}
		}

		public void AddFloorRequest(int floor)
		{
			if (floor > CurrentFloor && !_upwardRequests.Contains(floor))
			{
				_upwardRequests.Enqueue(floor);
			}
			else if (floor < CurrentFloor && !_downwardRequests.Contains(floor))
			{
				_downwardRequests.Enqueue(floor);
			}

			ButtonColors[floor] = "Red"; // Change color to Red when pressed
			OnPropertyChanged(nameof(ButtonColors));
			ProcessNextFloor();
		}

		private async void ProcessNextFloor()
		{
			while (_upwardRequests.Count > 0 || _downwardRequests.Count > 0)
			{
				if (_isMovingUp)
				{
					while (_upwardRequests.Count > 0)
					{
						int targetFloor = _upwardRequests.Dequeue();
						while (CurrentFloor < targetFloor)
						{
							CurrentFloor++;
							await Task.Delay(2000); // Move one floor every 2 seconds
						}
						await Task.Delay(2000); // Wait 2 seconds on the floor
						ButtonColors[targetFloor] = "DodgerBlue"; // Reset color to DodgerBlue
						OnPropertyChanged(nameof(ButtonColors));
						FloorReached?.Invoke(targetFloor); // Notify that a floor has been reached
					}
					_isMovingUp = false;
				}
				else
				{
					while (_downwardRequests.Count > 0)
					{
						int targetFloor = _downwardRequests.Dequeue();
						while (CurrentFloor > targetFloor)
						{
							CurrentFloor--;
							await Task.Delay(2000); // Move one floor every 2 seconds
						}
						await Task.Delay(2000); // Wait 2 seconds on the floor
						ButtonColors[targetFloor] = "DodgerBlue"; // Reset color to DodgerBlue
						OnPropertyChanged(nameof(ButtonColors));
						FloorReached?.Invoke(targetFloor); // Notify that a floor has been reached
					}
					_isMovingUp = true;
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}


}
