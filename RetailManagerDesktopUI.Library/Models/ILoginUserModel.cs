using System;

namespace RetailManagerDesktopUI.Library.Models
{
    public interface ILoginUserModel
    {
        string Id { get; set; }
        string Token { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        DateTime CreateDate { get; set; }
    }
}