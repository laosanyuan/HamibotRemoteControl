using Autofac;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HamibotRemoteControl.DataBase;
using HamibotRemoteControl.Enums;
using HamibotRemoteControl.Tools;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace HamibotRemoteControl.ViewModels
{
    [ObservableObject]
    partial class DataStatisticsPageViewModel
    {
        private ApiCallCountDb _db;

        private SKColor _lineColor = ResourcesHelper.GetResource<Color>("Foreground.Secondary").ToSKColor();
        private SKColor _textColor = ResourcesHelper.GetResource<Color>("Foreground.Primary").ToSKColor();


        #region [Properties]
        /// <summary>
        /// 图表横坐标设置
        /// </summary>
        [ObservableProperty]
        private List<Axis> _xAxes;

        /// <summary>
        /// 图表纵坐标设置
        /// </summary>
        [ObservableProperty]
        private List<Axis> _yAxes;

        /// <summary>
        /// 图表数据
        /// </summary>
        [ObservableProperty]
        private List<ISeries> _series;

        /// <summary>
        /// 当月调用次数
        /// </summary>
        [ObservableProperty]
        private int _monthCount;

        /// <summary>
        /// 平均每日调用次数
        /// </summary>
        [ObservableProperty]
        private double _averageCount;

        /// <summary>
        /// 数据历史类型
        /// week - month - all
        /// </summary>
        [ObservableProperty]
        private DataHistoryType _historyType = DataHistoryType.Month;
        #endregion

        #region [Commands]
        [RelayCommand]
        private async Task Left()
        {
            switch (HistoryType)
            {
                case DataHistoryType.Week:
                    HistoryType = DataHistoryType.All;
                    break;
                case DataHistoryType.Month:
                    HistoryType = DataHistoryType.Week;
                    break;
                default:
                    HistoryType = DataHistoryType.Month;
                    break;
            }

            await UpdateData();
        }

        [RelayCommand]
        private async Task Right()
        {
            switch (HistoryType)
            {
                case DataHistoryType.Week:
                    HistoryType = DataHistoryType.Month;
                    break;
                case DataHistoryType.Month:
                    HistoryType = DataHistoryType.All;
                    break;
                default:
                    HistoryType = DataHistoryType.Week;
                    break;
            }

            await UpdateData();
        }

        #endregion

        public DataStatisticsPageViewModel()
        {
            WeakReferenceMessenger.Default.Register<object, string>(this,
                MessengerTokens.RefreshDataStatisticPageData, async (_, obj) => await this.UpdateData());

            this._db = App.Container.Resolve<ApiCallCountDb>();
            this.InitChart();
        }

        #region [Private Methods]
        private void InitChart()
        {
            this.YAxes = new List<Axis>()
            {
                new()
                {
                    TextSize = 10,
                    LabelsPaint = new SolidColorPaint(this._textColor),
                    TicksPaint = new SolidColorPaint()
                    {
                        Color = _lineColor,
                        StrokeThickness = 1
                    },
                    SeparatorsPaint = new SolidColorPaint
                    {
                        Color = _lineColor,
                        StrokeThickness = 0.3F,
                        PathEffect = new DashEffect(new float[] { 3, 3 })
                    },
                }
            };

            this.XAxes = new List<Axis>()
            {
                new()
                {
                    TextSize = 10,
                    LabelsPaint = new SolidColorPaint(this._textColor),
                    Padding = new Padding(5,15,5,5),
                    Labeler = value => value.AsDate().ToString("MM-dd"),
                    LabelsRotation = 45,
                    UnitWidth = TimeSpan.FromDays(1).Ticks,
                    MinStep = TimeSpan.FromDays(1).Ticks,
                    TicksPaint = new SolidColorPaint()
                    {
                        Color = _lineColor,
                        StrokeThickness = 1
                    },
                    SeparatorsPaint = new SolidColorPaint
                    {
                        Color = _lineColor,
                        StrokeThickness = 0.3F,
                        PathEffect = new DashEffect(new float[] { 3, 3 })
                    },
                }
            };
        }

        private async Task UpdateData()
        {
            DateTime? startDate = this.HistoryType switch
            {
                DataHistoryType.Week => DateTime.Now.Date.AddDays(-6),
                DataHistoryType.Month => new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                _ => null
            };

            var data = await _db.GetCounts(startDate);

            this.Series = new List<ISeries>()
            {
                new ColumnSeries<DateTimePoint>()
                {
                    Values = data.Select(t => new DateTimePoint(t.Date,t.Count))
                }
            };

            if (startDate == null)
            {
                startDate = data?.FirstOrDefault()?.Date;
            }

            if (startDate == null)
            {
                this.AverageCount = 0;
            }
            else
            {
                var days = (DateTime.Now - startDate).Value.Days;
                this.AverageCount = Math.Round((double)data.Sum(t => t.Count) / days, 1);
            }

            var month = await _db.GetCurrentMonthCounts();
            this.MonthCount = month.Sum(t => t.Count);
        }
        #endregion
    }
}
