using Bookinist.DAL.Entities;

namespace Bookinist.Services.Interfaces;

internal interface IUserDialog
{
    bool Edit(Book book);

    bool ConfirmInformation(string information, string caption);
    bool ConfirmWarning(string warning, string caption);
    bool ConfirmError(string error, string caption);
}