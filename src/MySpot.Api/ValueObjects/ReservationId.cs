﻿using MySpot.Api.Exceptions;

namespace MySpot.Api.ValueObjects
{
    public sealed record ReservationId
    {
        public Guid Value { get; }

        public ReservationId(Guid value)
        {
            if (value == Guid.Empty)
                throw new InvalidEntityIdException(value);

            Value = value;
        }

        public static ReservationId Create() => new(Guid.Empty);

        public static implicit operator ReservationId(Guid value) => new(value);

        public static implicit operator Guid(ReservationId value) => value.Value;
    }
}
