using System;
using System.Collections.Generic;

namespace Sips.SipsModels;

public partial class PaymentNotification
{
    public string PaymentId { get; set; }

    public string? Amount { get; set; }

    public string? CurrencyCode { get; set; }

    public string? CurrencySymbol { get; set; }

    public string? PayerId { get; set; }

    public string? PayerFullName { get; set; }

    public string? CaptureId { get; set; }
}
