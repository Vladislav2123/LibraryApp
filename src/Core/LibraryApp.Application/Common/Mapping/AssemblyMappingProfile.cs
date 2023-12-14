using AutoMapper;
using System.Reflection;

namespace LibraryApp.Application.Common.Mappings;

public class AssemblyMappingProfile : Profile
{
	public AssemblyMappingProfile(Assembly assembly)
	{
		var types = assembly.GetExportedTypes()
			.Where(type => type.GetInterfaces()
			.Any(type => type.IsInterface && type == typeof(IMappping)))
			.ToList();

		foreach (var type in types)
		{
			var instance = Activator.CreateInstance(type);
			var method = type.GetMethod("CreateMap");
			method?.Invoke(instance, new object[] { this });
		}
	}
}
