﻿using AutoMapper;
using LibraryApp.Application.Common.Mappings;
using LibraryApp.Domain.Enteties;

namespace LibraryApp.Application.Feauters.Books.Querries.Dto
{
	public class BookLookupDto : IMappping
	{
		public Guid Id { get; set; }
		public Guid AuthorId { get; set; }
		public string Name { get; set; }
		public string Year { get; set; }
		public int ReadUsersCount { get; set; }

		public void CreateMap(Profile profile)
		{
			profile.CreateMap<Book, BookLookupDto>()
				.ForMember(dest => dest.ReadUsersCount,
					opt => opt.MapFrom(src => src.ReadUsers.Select(user => user.Id)));
		}
	}
}