using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LibraryApp.DAL.ValueConverters
{
	public class DateOnlyNullableConverter : ValueConverter<DateOnly?, DateTime?>
	{
		public DateOnlyNullableConverter() : base(
			dateOnly => dateOnly.HasValue ? dateOnly.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
			dateTime => dateTime.HasValue ? DateOnly.FromDateTime(dateTime.Value) : (DateOnly?)null) { }
	}
}
