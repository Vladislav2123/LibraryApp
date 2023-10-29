﻿namespace LibraryApp.Application.Common.Pagination
{
    public struct Page
    {
        public int number;
        public int size;

        public Page(int number, int size)
        {
            this.number = number;
            this.size = size;
        }
    }
}
