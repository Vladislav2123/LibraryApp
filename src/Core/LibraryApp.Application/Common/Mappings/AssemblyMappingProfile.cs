using AutoMapper;
using System.Reflection;

namespace LibraryApp.Application.Common.Mappings
{
	public class AssemblyMappingProfile : Profile
	{
		public AssemblyMappingProfile(Assembly assebly) => ApplyMappingsFromAssembly(assebly);

		private void ApplyMappingsFromAssembly(Assembly assembly)
		{
			Console.WriteLine("Assembly mapping profile");

			var types = assembly.GetExportedTypes()
				.Where(type => type.GetInterfaces()
				.Any(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IMapWith<>)))
				.ToList();

			foreach (var type in types)
			{
				var instance = Activator.CreateInstance(type);
				var method = type.GetMethod("CreateMap");
				method?.Invoke(instance, new object[] { this });
			}
		}
	}
}
