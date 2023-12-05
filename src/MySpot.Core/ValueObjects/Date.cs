namespace MySpot.Core.ValueObjects
{
    public sealed record Date
    {
        public DateTimeOffset Value { get; }

        public Date(DateTimeOffset value)
            => Value = value;

        public Date AddDays(int days) => new(Value.AddDays(days));

        public static implicit operator DateTimeOffset(Date date) => date.Value;

        public static implicit operator Date(DateTimeOffset value) => new(value);

        public static bool operator <(Date date1, Date date2)
            => date1.Value.Date < date2.Value.Date;

        public static bool operator >(Date date1, Date date2)
            => date1.Value.Date > date2.Value.Date;

        public static bool operator <=(Date date1, Date date2)
            => date1.Value.Date <= date2.Value.Date;

        public static bool operator >=(Date date1, Date date2)
            => date1.Value.Date >= date2.Value.Date;

        public static Date Now => new(DateTimeOffset.Now);

        public override string ToString() => Value.ToString("d");
    }
}
