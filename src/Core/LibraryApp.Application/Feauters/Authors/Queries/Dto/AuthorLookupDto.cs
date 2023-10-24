﻿using AutoMapper;
using LibraryApp.Application.Common.Mappings;
using LibraryApp.Domain.Enteties;

namespace LibraryApp.Application.Feauters.Authors.Queries.Dto
{
	public class AuthorLookupDto : IMappping
	{
		public Guid Id { get; set; }
		public string Name { get; set; }

		public void CreateMap(Profile profile)
		{
			profile.CreateMap<Author, AuthorLookupDto>();
		}
	}
}