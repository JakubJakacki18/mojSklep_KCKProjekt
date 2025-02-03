using Library.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Converter
{
	internal class EnumConverter
	{
		public static string GetEnumDescription(PaymentMethodEnum value)
		{
			var field = value.GetType().GetField(value.ToString());
			if (field == null)
				return value.ToString();
			var attribute = System.Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
			return attribute == null ? value.ToString() : attribute.Description;
		}
	}
}
