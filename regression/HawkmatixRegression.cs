/*
 * Hawkmatix Regression 1.0.0.2
 * Official project page: http://www.hawkmatix.com/regression.html
 *
 * Copyright (C) 2013 Andrew Hawkins
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

// Using declaration
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Data;
using NinjaTrader.Gui.Chart;

// User menu choices
public enum VisualAid
{
	None,
	IncreasingDecreasing,
	SupportResistance
}

// Begin indicator script
namespace NinjaTrader.Indicator
{
	// Description
	[Description("Hawkmatix Regression determines a second order polynomial that best fits the data.")]

	// Begin class script
	public class HawkmatixRegression : Indicator
	{
		// Declare class variables
		private int period                 = 55;
		private VisualAid visualAid        = VisualAid.None;
		private Color increasingSupport    = Color.Green;
		private Color decreasingResistance = Color.Red;

		// Override Initialize method
		protected override void Initialize()
		{
			// Add plot
			Add(new Plot(new Pen(Color.DeepSkyBlue, 3), PlotStyle.Line, "Regression"));

			// Default parameters
			CalculateOnBarClose = false;
			MaximumBarsLookBack = MaximumBarsLookBack.Infinite;
			Overlay             = true;
		}

		// Override OnBarUpdate method
		protected override void OnBarUpdate()
		{
			// Protect against too few bars
			if (CurrentBar < period)
			{
				Regression.Reset();
				return;
			}

			// Declare local variables
			double n      = period - 1;
			double sumX   = n * (n + 1) / 2;
			double sumX2  = n * (n + 1) * (2 * n + 1) / 6;
			double sumX3  = sumX * sumX;
			double sumX4  = Math.Pow(n, 5) / 5 + Math.Pow(n, 4) / 2 + Math.Pow(n, 3) / 3 - n / 30;
			double sumY   = 0;
			double sumXY  = 0;
			double sumX2Y = 0;

			// Sum price values
			for (int i = 0; i < period; i++)
			{
				sumY   += Input[i];
				sumXY  += (n - i) * Input[i];
				sumX2Y += (n - i) * (n - i) * Input[i];
			}

			// Determine first two multipliers
			double r1 = -sumX3 / sumX4;
			double r2 = -sumX2 / sumX4;

			// Apply first two matrix operations
			double z0 = r1 * sumX3 + sumX2;
			double z1 = r1 * sumX2 + sumX;
			double zy = r1 * sumX2Y + sumXY;
			double k0 = r2 * sumX3 + sumX;
			double k1 = r2 * sumX2 + period;
			double ky = r2 * sumX2Y + sumY;

			// Determine third multiplier
			double r3 = -k0 / z0;

			// Apply final matrix operation
			double f  = r3 * z1 + k1;
			double fy = r3 * zy + ky;

			// Back substitute to solve for a, b, and c
			double c = fy / f;
			double b = (zy - z1 * c) / z0;
			double a = (sumX2Y - b * sumX3 - c * sumX2) / sumX4;

			// Set the value to the newest price
			Regression.Set(a * n * n + b * n + c);

			// Set visual aid, if any
			if (visualAid == VisualAid.IncreasingDecreasing)
			{
				if (Rising(Regression))
				{
					PlotColors[0][0] = increasingSupport;
				}
				else
				{
					PlotColors[0][0] = decreasingResistance;
				}
			}
			else if (visualAid == VisualAid.SupportResistance)
			{
				if (Close[0] > Regression[0])
				{
					PlotColors[0][0] = increasingSupport;
				}
				else
				{
					PlotColors[0][0] = decreasingResistance;
				}
			}
		}

		// Properties of plots and inputs
		[Browsable(false)]
		[XmlIgnore()]
		public DataSeries Regression
		{
			get { return Values[0]; }
		}

		[Description("Amount of bars used for calculations.")]
		[Category("Parameters")]
		[Gui.Design.DisplayNameAttribute("Distribution")]
		public int Period
		{
			get { return period; }
			set { period = Math.Max(1, value); }
		}

		[Description("Method used to alter the line coloring.")]
		[Category("Parameters")]
		[Gui.Design.DisplayNameAttribute("Visual Aid")]
		public VisualAid VisualAid
		{
			get { return visualAid; }
			set { visualAid = value; }
		}

		[XmlIgnore()]
		[Description("Color for the increasing or support visual aid.")]
		[Category("Color")]
		[Gui.Design.DisplayNameAttribute("Increasing or Support")]
		public Color IncreasingSupport
		{
			get { return increasingSupport; }
			set { increasingSupport = value; }
		}
		[Browsable(false)]
		public string IncreasingSupportSerialize
		{
			get { return NinjaTrader.Gui.Design.SerializableColor.ToString(increasingSupport); }
			set { increasingSupport = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
		}

		[XmlIgnore()]
		[Description("Color for the decreasing or resistance visual aid.")]
		[Category("Color")]
		[Gui.Design.DisplayNameAttribute("Decreasing or Resistance")]
		public Color DecreasingResistance
		{
			get { return decreasingResistance; }
			set { decreasingResistance = value; }
		}
		[Browsable(false)]
		public string DecreasingResistanceSerialize
		{
			get { return NinjaTrader.Gui.Design.SerializableColor.ToString(decreasingResistance); }
			set { decreasingResistance = NinjaTrader.Gui.Design.SerializableColor.FromString(value); }
		}
	}
}
