using AutoMapper;

namespace LibraryApp.Application.Common.Mappings
{
	internal interface IMapWith<T>
	{
		void CrateMap(Profile profile) => profile.CreateMap(typeof(T), GetType());
	}
}
