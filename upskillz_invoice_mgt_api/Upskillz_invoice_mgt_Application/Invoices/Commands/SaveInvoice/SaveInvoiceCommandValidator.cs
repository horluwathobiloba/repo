using FluentValidation;

namespace Upskillz_invoice_mgt_Application.Invoices.Commands.SaveInvoice
{
    public class SaveInvoiceCommandValidator : AbstractValidator<SaveInvoiceCommand>
    {
        public SaveInvoiceCommandValidator()
        {
            RuleFor(v => v.InvoiceAmount).NotEqual(0).GreaterThan(0).WithMessage("Amount must be greater than 0");
            RuleFor(v => v.InvoiceNumber).NotEmpty().WithMessage("This field is required");
        }
    }
}
