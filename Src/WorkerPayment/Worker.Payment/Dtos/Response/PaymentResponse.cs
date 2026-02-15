using System;
using System.Collections.Generic;
using System.Text;

namespace WorkerPayment.Dtos.Response
{
    public record PaymentResponse(bool Resultado, string Detalhe);
}
