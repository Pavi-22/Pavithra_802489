﻿using MOD_PaymentService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOD_PaymentService.Repositories
{
    public interface IPaymentRepository
    {
        void AddPayment(Payment item);
        List<Payment> GetAllPayment();
        void UpdatePayment(Payment item);
        void DeletePayment(int id);
    }
}
