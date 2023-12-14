using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.DAL.ValueConverters;

public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
	public DateOnlyConverter() : base(
		dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue),
		dateTime => DateOnly.FromDateTime(dateTime)) { }
}
