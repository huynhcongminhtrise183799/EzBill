﻿using EzBill.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Domain.IRepository
{
    public interface IEventRepository
    {
        Task<bool> AddEvent(Event @event);
    }
}
