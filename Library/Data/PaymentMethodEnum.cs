using System.ComponentModel;

namespace Library.Data
{
    public enum PaymentMethodEnum
    {
        [Description("Karta")]
        card = 1,
		[Description("Gotówka")]
		cash = 0,
		[Description("Blik")]
		blik = 2,
		None = 3,
		Exit = 4
	}
}
