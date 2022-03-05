﻿using BackendGestionaleBar.DataAccessLayer.Entities;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BackendGestionaleBar.DataAccessLayer
{
    public interface IDatabase : IDisposable
    {
        Task<DataRow> GetClienteAsync(Guid idCliente);
        Task<bool> LoginAsync(string email, string password);
        Task<int> RegisterClienteAsync(Cliente cliente);
        Task<int> RegisterCredenzialiAsync(Credenziali credenziali, string role);
    }
}