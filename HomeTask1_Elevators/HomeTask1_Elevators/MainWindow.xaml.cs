using HomeTask1_Elevators.Models;
using HomeTask1_Elevators.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HomeTask1_Elevators
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			this.DataContext = new MainWindowViewModel();

			var viewModel = (MainWindowViewModel)DataContext;

			viewModel.Elevator1.PropertyChanged += Elevator1_CurrentFloorChanged;
			viewModel.Elevator2.PropertyChanged += Elevator2_CurrentFloorChanged;
			viewModel.Elevator3.PropertyChanged += Elevator3_CurrentFloorChanged;
		}

		private void Elevator1_CurrentFloorChanged(object sender, EventArgs e)
		{
			AnimateSlider(slider1, ((Elevator)sender).CurrentFloor);
		}

		private void Elevator2_CurrentFloorChanged(object sender, EventArgs e)
		{
			AnimateSlider(slider2, ((Elevator)sender).CurrentFloor);
		}

		private void Elevator3_CurrentFloorChanged(object sender, EventArgs e)
		{
			AnimateSlider(slider3, ((Elevator)sender).CurrentFloor);
		}

		private void AnimateSlider(Slider slider, double toValue)
		{
			var animation = new DoubleAnimation
			{
				To = toValue,
				Duration = TimeSpan.FromSeconds(2),
				EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
			};

			slider.BeginAnimation(Slider.ValueProperty, animation);
		}
	

	}
}
