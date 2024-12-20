﻿namespace hippserver.Services.Interfaces
{
    public interface IBaseService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(string id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
    }
}