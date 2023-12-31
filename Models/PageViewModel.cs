﻿namespace pos.Models
{
	public class PageViewModel<T>
	{
		public List<T> Items { get; set; }
		public int TotalItems { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }

		public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

		public bool HasPreviousPage => PageNumber > 1;

		public bool HasNextPage => PageNumber < TotalPages;

		//public PageModel(List<T> items, int totalItems, int pageNumber, int pageSize)
		//{
		//	Items = items;
		//	TotalItems = totalItems;
		//	PageNumber = pageNumber;
		//	PageSize = pageSize;
		//}

	}
}
