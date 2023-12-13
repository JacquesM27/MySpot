﻿using MySpot.Core.ValueObjects;

namespace MySpot.Core.Exceptions
{
    public sealed class NoReservationPolicyFoundException(JobTitle jobTitle) : CustomException($"No reservation policy for {jobTitle} has been found.")
    {
        public JobTitle JobTitle { get; } = jobTitle;
    }
}
