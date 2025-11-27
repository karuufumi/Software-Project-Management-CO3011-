
using backend.models;
using backend.repository;
using Microsoft.AspNetCore.Mvc;

namespace backend.repository
{
    public interface IBorrowRepository
    {
        Task BorrowBook(string userId, string bookId);
    }
}