namespace Innovation.Api.Dispatching
{
    using System;
    using System.Diagnostics;

    public readonly struct ValueStopwatch
    {
        #region Fields

        private static readonly double timestampToTicks = TimeSpan.TicksPerSecond / (double)Stopwatch.Frequency;

        private readonly long startTimestamp;
        private ValueStopwatch(long startTimestamp) => this.startTimestamp = startTimestamp;

        #endregion Fields

        #region Methods

        public static ValueStopwatch StartNew() => new ValueStopwatch(GetTimestamp());

        public static long GetTimestamp() => Stopwatch.GetTimestamp();

        public static TimeSpan GetElapsedTime(long startTimestamp, long endTimestamp)
        {
            var timestampDelta = endTimestamp - startTimestamp;
            var ticks = (long)(timestampToTicks * timestampDelta);

            return new TimeSpan(ticks);
        }

        public TimeSpan GetElapsedTime() => GetElapsedTime(startTimestamp, GetTimestamp());

        #endregion Methods
    }
}
