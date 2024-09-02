﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HomeTask1_Elevators.Converters
{
	public class StringToBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string colorName)
			{
				return (Brush)new BrushConverter().ConvertFromString(colorName);
			}
			return Brushes.Transparent;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
