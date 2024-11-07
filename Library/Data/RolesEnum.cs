namespace Library.Data
{
    [Flags]
    public enum RolesEnum
    {
        Buyer = 1,
        Seller = 2,
        Admin = 4,
        PermissionBuyer = Buyer,
        PermissionSeller = Buyer | Seller,
        PermissionAdmin = Buyer | Seller | Admin

    }
}
