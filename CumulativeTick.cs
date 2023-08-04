// Copyright QUANTOWER LLC. © 2017-2022. All rights reserved.

using System;
using System.Drawing;
using TradingPlatform.BusinessLayer;

namespace CumulativeTick
{
	public class CumulativeTick : Indicator
    {
        [InputParameter("First Hour")]
        public int firstHour = 9;

        [InputParameter("Last Hour")]
        public int lastHour = 16;

        public CumulativeTick()
            : base()
        {
            Name = "CumulativeTick";
            Description = "My indicator's annotation";
            AddLineSeries("line1", Color.Lime, 2, LineStyle.Histogramm);
            AddLineSeries("line2", Color.DarkGreen, 2, LineStyle.Histogramm);
            AddLineSeries("line3", Color.Red, 2, LineStyle.Histogramm);
            AddLineSeries("line4", Color.DarkRed, 2, LineStyle.Histogramm);
            AddLineLevel(0.0, "", Color.WhiteSmoke, 1, LineStyle.Dash);
            SeparateWindow = true;
        }

        protected override void OnInit()
        {
        }

        protected override void OnUpdate(UpdateArgs args)
        {
            double cumtick = 0.0;
            for (int i = this.Count - 1; i >= 0; i--)
            {
                DateTime leftTime = this.Time(i).ToSelectedTimeZone();
                double prevtick = cumtick;
                if ((leftTime.Hour == this.firstHour && leftTime.Minute >= 30))
                    cumtick = this.Close(i);
                else if (leftTime.Hour > this.firstHour && leftTime.Hour <= this.lastHour)
                    cumtick += this.Close(i);
                else
                    cumtick = 0.0;

                this.UnsetAtIndex(i);

                if (cumtick > 0.0 && cumtick > prevtick)
                    this.SetValue(cumtick, 0, i);
                else if (cumtick > 0.0 && cumtick <= prevtick)
                    this.SetValue(cumtick, 1, i);
                else if (cumtick < 0.0 && cumtick < prevtick)
                    this.SetValue(cumtick, 2, i);
                else
                    this.SetValue(cumtick, 3, i);
            }
        }

        private void UnsetAtIndex(int index)
        {
            this.SetValue(0, 0, index);
            this.SetValue(0, 1, index);
            this.SetValue(0, 2, index);
            this.SetValue(0, 3, index);
        }
    }
}
