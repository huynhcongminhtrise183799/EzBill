﻿using EzBill.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Application.IService
{
    public interface ITripService
    {
        Task<bool> AddTrip(Trip trip);

    }
}
